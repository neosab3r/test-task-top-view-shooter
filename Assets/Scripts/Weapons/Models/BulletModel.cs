using BeeGood.Extensions;
using BeeGood.Views;
using UnityEngine;

namespace BeeGood.Models
{
    public class BulletModel : BaseModel<BulletView>
    {
        public bool ReadyToDestroy { get; private set; }
        public string TargetTagEntity = "";
        public IModel OwnerPlayer;

        private Transform cachedViewTransform;
        private BulletData cachedBulletData;
        private Vector3 cachedDirection;
        private int countRicochets = 0;

        public BulletModel(BulletView view) : base(view)
        {
            cachedViewTransform = view.transform;
            cachedDirection = cachedViewTransform.forward;
            cachedBulletData = view.BulletData;
            view.SubscribeOnTriggerEnterEvent(OnCollision);
        }

        public void TryMove()
        {
            if (ReadyToDestroy == false)
            {
                cachedViewTransform.position += cachedDirection * cachedBulletData.bulletSpeed * Time.deltaTime;
            }
        }

        public void SetDirection(Vector3 direction)
        {
            cachedDirection = direction;
        }

    private void OnCollision(Collision collision)
        {
            var tagGameObject = collision.gameObject.tag;
            if (string.IsNullOrEmpty(tagGameObject))
            {
                Debug.LogError($"{nameof(BulletModel)} -- Collision GameObject with name {collision.gameObject.name} has no tag. BulletModel Owner -- {nameof(OwnerPlayer)}");
            }

            if (ReadyToDestroy)
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
                Debug.LogWarning("Damage to Enemy Entity");
                //Damage
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
            cachedViewTransform = null;
            View = null;
        }

        public override void Dispose()
        {
            OwnerPlayer = null;
        }
    }
}