using BeeGood.Managers.Contexts;
using UnityEngine;

namespace BeeGood.Managers
{
    public class CheckManagerContext : TransformContext
    {
        public bool IsVisible;
        public Vector3 BotToPlayerDirection;
        public Vector3 HandToPlayerDirection;

        public CheckManagerContext(Transform transform, bool isVisible, Vector3 botToPlayerDirection, Vector3 handToPlayerDirection) : base(transform)
        {
            IsVisible = isVisible;
            BotToPlayerDirection = botToPlayerDirection;
            HandToPlayerDirection = handToPlayerDirection;
        }
    }

    public class CheckSearchZoneBotManager : BaseBotManager<CheckManagerContext>
    {
        public override BotManagerState Evaluate()
        {
            var colliders = new Collider[1];
            var size = Physics.OverlapSphereNonAlloc(OwnerBotModel.CachedTransform.position, 
                9f, colliders, 1 << LayerMask.NameToLayer("Player"));
            
            if (size > 0)
            {
                var botTransform = OwnerBotModel.CachedTransform;
                var playerTransform = colliders[0].gameObject.transform;
                var botToPlayerDirection = playerTransform.position - botTransform.position;
                var handToPlayerDirection = playerTransform.position - OwnerBotModel.CachedHandTransform.position;
                
                Physics.Raycast(botTransform.position, botToPlayerDirection, out var hit, Mathf.Infinity);
                var isVisible = playerTransform == hit.transform;

                var context = new CheckManagerContext(playerTransform, isVisible, botToPlayerDirection, handToPlayerDirection);
                
                SetContext(context);
                State = BotManagerState.Success;
                return State;
            }

            Context = null;
            State = BotManagerState.Failed;
            return State;
        }
    }
}