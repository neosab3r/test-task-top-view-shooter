using BeeGood.Extensions;
using BeeGood.Managers.Contexts;
using BeeGood.Models;
using BeeGood.Systems;
using UnityEngine;

namespace BeeGood.Managers
{
    public class AttackContext : IManagerContext
    {
        public bool IsVisible;

        public AttackContext(bool isVisible)
        {
            IsVisible = isVisible;
        }
    }
    
    public class AttackBotManager : BaseBotManager<TransformContext>
    {
        private WeaponModel weaponModel;
        public AttackBotManager(WeaponModel weaponModel)
        {
            this.weaponModel = weaponModel;
        }

        public override BotManagerState Evaluate()
        {
            if (weaponModel.IsReloading())
            {
                State = BotManagerState.Running;
                return State;
            }

            var searchBotManagerContext = Parent.GetManager<CheckSearchZoneBotManager>().Context;
            if (searchBotManagerContext == null)
            {
                Debug.LogWarning($"{nameof(AttackBotManager)} Evaluate Failed because {nameof(CheckSearchZoneBotManager)} context not found.");
                State = BotManagerState.Failed;
                return State;
            }

            var player = searchBotManagerContext.Transform;
            var botTransform = OwnerBotModel.CachedTransform;
            var handTransform = OwnerBotModel.CachedHandTransform;
            var bulletToPlayerDirection = searchBotManagerContext.BulletToPlayerDirection;
            var isVisible = searchBotManagerContext.IsVisible;
            
            var inAttackAngle = false;
            var attackDot = Vector3.Dot(handTransform.forward, Vector3.Normalize(bulletToPlayerDirection));
            if (attackDot >= 0.9)
            {
                inAttackAngle = true;
            }

            if (isVisible == false || inAttackAngle == false)
            {
                State = BotManagerState.Failed;
                return State;
            }

            weaponModel.Shoot(TagExtension.PlayerTag, Vector3.Normalize(bulletToPlayerDirection));
            State = BotManagerState.Success;
            return State;
        }

        public override void Dispose()
        {
            weaponModel = null;
            base.Dispose();
        }
    }
}