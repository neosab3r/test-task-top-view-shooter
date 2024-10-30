using BeeGood.Managers.Contexts;
using UnityEngine;

namespace BeeGood.Managers
{
    public class MoveBotManager : BaseBotManager<TransformContext>
    {
        public override BotManagerState Evaluate()
        {
            var searchBotManagerContext = Parent.Parent.GetManager<CheckSearchZoneBotManager>().Context;
            if (searchBotManagerContext == null)
            {
                Debug.LogWarning($"{nameof(MoveBotManager)} Evaluate Failed because {nameof(CheckSearchZoneBotManager)} context not found.");
                State = BotManagerState.Failed;
                return State;
            }
            
            Context = new TransformContext(searchBotManagerContext.Transform);
            var point = Context.Transform;
            OwnerBotModel.SetMovePoint(point);
            
            if (Vector3.Distance(OwnerBotModel.CachedTransform.position, Context.Transform.position) < 3f)
            {
                Debug.LogError($"Bot has reached Enemy.");
                Context = null;
                OwnerBotModel.SetMovePoint(null);
                return BotManagerState.Running;
            }
            
            return BotManagerState.Running;
        }
    }
}