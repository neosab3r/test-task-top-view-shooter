using BeeGood.Extensions;
using BeeGood.Managers;
using BeeGood.Systems;
using BeeGood.View;
using BeeGood.Views;
using UnityEngine;

namespace BeeGood.Models
{
    public class BulletModel : BaseModel<BulletView>
    {
        public bool ReadyToDestroy { get; private set; }
        public Transform CachedViewTransform { get; private set; }
        public string TargetTagEntity = "";
        public IModel OwnerPlayer;

        private BulletData cachedBulletData;
        private Vector3 cachedDirection;
        private int countRicochets = 0;

        public Vector3 GetDirection() => cachedDirection;
        
        public BulletModel(BulletView view) : base(view)
        {
            CachedViewTransform = view.transform;
            cachedDirection = CachedViewTransform.forward;
            cachedBulletData = view.BulletData;
            view.SubscribeOnCollisionEvent(OnCollision);
        }

        public void TryMove()
        {
            if (ReadyToDestroy == false)
            {
                var worldForward = CachedViewTransform.position + cachedDirection;
                Debug.DrawLine(CachedViewTransform.position, worldForward, Color.yellow);
                CachedViewTransform.position += cachedDirection * cachedBulletData.bulletSpeed * Time.deltaTime;
                //cachedViewTransform.Translate( * Time.deltaTime * cachedBulletData.bulletSpeed);
            }
        }

        public void SetDirection(Vector3 direction)
        {
            cachedDirection = direction;
        }

        public string GetOwnerTag()
        {
            return TargetTagEntity == TagExtension.BotTag ? TagExtension.PlayerTag : TagExtension.BotTag;
        }
        
        private void OnCollision(Collision collision)
        {
            var tagGameObject = collision.gameObject.tag;
            var ownerTag = GetOwnerTag();
            
            Debug.LogError($"Owner: {ownerTag}");
            if (string.IsNullOrEmpty(tagGameObject))
            {
                Debug.LogError($"{nameof(BulletModel)} -- Collision GameObject with name {collision.gameObject.name} has no tag. BulletModel Owner -- {nameof(OwnerPlayer)}");
            }

            if (ReadyToDestroy)
            {
                return;
            }

            if (string.Equals(tagGameObject, ownerTag))
            {
                return;
            }

            if (string.Equals(tagGameObject, TagExtension.BulletTag))
            {
                return;
            }
            
            if (string.Equals(tagGameObject, TagExtension.BorderTag))
            {
                SetReadyToDestroy();
                return;
            }
            
            if (string.Equals(tagGameObject, TargetTagEntity))
            {
                var isPlayerWin = tagGameObject == TagExtension.BotTag;
                if (isPlayerWin)
                {
                    var botSystem = EntrySystem.Instance.Get<BotSystem>();
                    var botView = collision.gameObject.GetComponent<BotView>();
                    var botModel = botSystem.GetBotModel(botView);
                    botSystem.KillBot(botModel);
                    if (botSystem.IsRemainingBots() == false)
                    {
                        GameManager.Instance.EndRound(true);
                    }
                }
                else
                {
                    GameManager.Instance.EndRound(false);
                }
                SetReadyToDestroy();
            }
            else
            {
                var remainRicochets = cachedBulletData.maxRicochets - countRicochets;
                if (remainRicochets > 0)
                {
                    ReflectDirection(collision);
                    return;
                }
            }

            SetReadyToDestroy();
        }

        private void ReflectDirection(Collision collision)
        {
            countRicochets++;
            cachedDirection = Vector3.Reflect(cachedDirection, collision.contacts[0].normal);
        }

        private void SetReadyToDestroy()
        {
            ReadyToDestroy = true;
        }

        public void DestroyBulletView()
        {
            View.Dispose();
            Object.Destroy(View.gameObject);
            CachedViewTransform = null;
            View = null;
        }

        public override void Dispose()
        {
            OwnerPlayer = null;
        }
    }
}