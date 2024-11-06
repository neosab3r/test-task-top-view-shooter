using BeeGood.Managers.Contexts;
using UnityEngine;

namespace BeeGood.Managers
{
    public class IdleBotManager : BaseBotManager<TransformContext>
    {
        private const float MinEndDistance = 0.3f;
        public override BotManagerState Evaluate()
        {
            var checkSearchManager = Parent.GetManager<BotSequenceManager>().GetManager<CheckSearchZoneBotManager>();
            if (checkSearchManager != null)
            {
                if (checkSearchManager.Context != null)
                {
                    State = BotManagerState.Failed;
                    return State;
                }
            }

            if (Context == null)
            {
                Debug.LogError($"[{nameof(IdleBotManager)}] -- Context is null. Setting new Context");
                var point = PatrolPointsManager.Instance.GetRandomPatrolPoint();
                var context = new TransformContext(point);
                SetContext(context);
            }

            Debug.LogError($"[{nameof(IdleBotManager)}] Try SetMovePoint");
            var pointToMove = Context.Transform;
            OwnerBotModel.SetMovePoint(pointToMove, MinEndDistance);
            
            var distance = Vector3.Distance(OwnerBotModel.CachedTransform.position, Context.Transform.position);
            if (distance <= MinEndDistance + 0.2)
            {
                Debug.LogError($"[{nameof(IdleBotManager)}] Bot has reached patrol point.");
                Context = null;
            }

            State = BotManagerState.Running;
            return State;
        }
    }
}