using System.Collections.Generic;
using System.Linq;
using BeeGood.Extensions;
using BeeGood.Managers;
using BeeGood.Systems;
using BeeGood.UI;
using BeeGood.View;
using BeeGood.Views;
using Pathfinding;
using UnityEngine;

namespace BeeGood.Models
{
    public class BotModel : BaseModel<BotView>
    {
        public WeaponModel WeaponModel { get; protected set; }
        public Transform CachedTransform { get; protected set; }
        public Transform CachedHandTransform { get; protected set; }
        public AIPath CachedAIPath { get; protected set; }
        public BotData Data { get; protected set; }
        public Dictionary<GameObject, BulletModel> NearBulletModels { get; protected set; } = new();
        private BulletSystem bulletSystem;
        private BotSelectorManager rootSelectorManager;
        private AIDestinationSetter AiDestinationSetter;
        private bool isDeath;

        public BotModel(BotView view, BulletSystem bulletSystem , WeaponModel weaponModel): base(view)
        {
            WeaponModel = weaponModel;
            Data = view.GetBotData();
            this.bulletSystem = bulletSystem;
            CachedTransform = view.transform;
            CachedHandTransform = view.GetHandTransform();
            AiDestinationSetter = view.GetAIDestinationSetter();
            CachedAIPath = view.GetAIPath();
            view.SubscribeOnTriggerEnterEvent(SetEnteredBullet);
            view.SubscribeOnTriggerExitEvent(DeleteBullet);
        }

        public void CreateManagers()
        {
            if (rootSelectorManager != null)
            {
                rootSelectorManager.Dispose();
                rootSelectorManager = null;
            }

            var managers = new List<IBotManager>();
            managers.AddRange(new List<IBotManager>
            {
                new EvasionBotManager(),
                new BotSequenceManager(this, new List<IBotManager>
                {
                    new CheckSearchZoneBotManager(),
                    new BotParallelManager(this, new List<IBotManager>
                    {
                        new MoveBotManager(),
                        new RotateWeaponBotManager()
                    }),
                    new AttackBotManager(WeaponModel)
                }),

                new IdleBotManager()
            });
            
            rootSelectorManager = new BotSelectorManager(this, managers);
        }

        public void SetMovePoint(Transform target, float endDistance)
        {
            if (AiDestinationSetter.target != null && target == AiDestinationSetter.target)
            {
                return;
            }

            var targetName = target == null ? "Null" : target.gameObject.name;
            Debug.LogWarning($"Setting move point to {targetName}");
            AiDestinationSetter.target = target;
            CachedAIPath.endReachedDistance = endDistance;
        }
        
        public void UpdateBotManagers()
        {
            if (rootSelectorManager.ChildManagers.Count < 0)
            {
                return;
            }

            rootSelectorManager.Evaluate();
        }

        private void SetEnteredBullet(Collider collider)
        {
            var ga = collider.gameObject;
            var tag = ga.tag;
            
            if (string.Equals(tag, TagExtension.BulletTag) == false)
            {
                return;
            }
            
            if (NearBulletModels.ContainsKey(ga))
            {
                Debug.LogError($"Bot already has this bullet in dictionary");
                return;
            }
            var bulletView = ga.GetComponent<BulletView>();
            var bulletModel = bulletSystem.GetBulletModelByView(bulletView);
            
            if (bulletModel.GetOwnerTag() == TagExtension.BotTag)
            {
                return;
            }
            
            Debug.LogError("Bullet entered");
            NearBulletModels.Add(ga, bulletModel);
        }

        private void DeleteBullet(Collider collider)
        {
            var ga = collider.gameObject;

            if (NearBulletModels.ContainsKey(ga))
            {
                NearBulletModels.Remove(ga);
            }
        }

        public override void Dispose()
        {
            WeaponModel = null;
            CachedTransform = null;
            CachedHandTransform = null;
            CachedAIPath = null;
            NearBulletModels.Clear();
            NearBulletModels = null;
            bulletSystem = null;
            AiDestinationSetter = null;
            if (rootSelectorManager != null)
            {
                rootSelectorManager.Dispose();
                rootSelectorManager = null;
            }

            if (View != null)
            {
                Object.Destroy(View.gameObject);
            }

            View = null;
        }
    }
}