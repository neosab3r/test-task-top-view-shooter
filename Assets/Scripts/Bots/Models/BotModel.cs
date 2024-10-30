using System.Collections.Generic;
using BeeGood.Managers;
using BeeGood.View;
using Pathfinding;
using UnityEngine;

namespace BeeGood.Models
{
    public class BotModel : BaseModel<BotView>
    {
        public WeaponModel WeaponModel { get; protected set; }
        public Transform CachedTransform { get; protected set; }
        public Transform CachedHandTransform { get; protected set; }

        private BotSelectorManager rootSelectorManager;
        private AIDestinationSetter AiDestinationSetter;
        private bool isDeath;

        public BotModel(BotView view, WeaponModel weaponModel): base(view)
        {
            CachedTransform = view.transform;
            CachedHandTransform = view.GetHandTransform();
            AiDestinationSetter = view.GetAIDestinationSetter();
            WeaponModel = weaponModel;
        }

        public void CreateManagers()
        {
            if (rootSelectorManager != null)
            {
                rootSelectorManager.Dispose();
                rootSelectorManager = null;
            }
            
            var managers = new List<IBotManager>
            {
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
            };

            rootSelectorManager = new BotSelectorManager(this, managers);
        }

        public void SetMovePoint(Transform target)
        {
            if (AiDestinationSetter.target != null && target == AiDestinationSetter.target)
            {
                return;
            }

            var targetName = target == null ? "Null" : target.gameObject.name;
            Debug.LogWarning($"Setting move point to {targetName}");
            AiDestinationSetter.target = target;
        }
        
        public void UpdateBotManagers()
        {
            if (rootSelectorManager.ChildManagers.Count < 0)
            {
                return;
            }

            rootSelectorManager.Evaluate();
        }

        public override void Dispose()
        {
            if (rootSelectorManager != null)
            {
                rootSelectorManager.Dispose();
                rootSelectorManager = null;
            }
        }
    }
}