using UnityEngine;

namespace BeeGood.Managers
{
    public class MoveBotManager : BaseBotManager<EmptyContext>
    {
        private const float MinEnemyEndDistance = 3f;
        private const float MinEndDistance = 0.3f;

        public override BotManagerState Evaluate()
        {
            var searchBotManager = Parent.Parent.GetManager<CheckSearchZoneBotManager>();
            if (searchBotManager == null)
            {
                Debug.LogWarning($"{nameof(MoveBotManager)} Evaluate Failed because {nameof(CheckSearchZoneBotManager)} context not found.");
                State = BotManagerState.Failed;
                return State;
            }
            
            var context = searchBotManager.Context;
            var point = context.Transform;
            var minDistance = context.IsVisible ? MinEnemyEndDistance : MinEndDistance;
            OwnerBotModel.SetMovePoint(point, minDistance);
            
            if (Vector3.Distance(OwnerBotModel.CachedTransform.position, point.position) <= minDistance)
            {
                Debug.LogError($"Bot has reached Enemy.");
                Context = null;
                OwnerBotModel.SetMovePoint(null, minDistance);
                return BotManagerState.Running;
            }
            
            return BotManagerState.Running;
        }
    }
}