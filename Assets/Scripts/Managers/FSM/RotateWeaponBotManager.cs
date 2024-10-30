using BeeGood.Managers.Contexts;
using UnityEngine;

namespace BeeGood.Managers
{
    public class RotateWeaponBotManager : BaseBotManager<TransformContext>
    {
        public override BotManagerState Evaluate()
        {
            var searchBotManagerContext = Parent.Parent.GetManager<CheckSearchZoneBotManager>().Context;
            if (searchBotManagerContext == null)
            {
                Debug.LogWarning($"{nameof(RotateWeaponBotManager)} Evaluate Failed because {nameof(CheckSearchZoneBotManager)} context not found.");
                State = BotManagerState.Failed;
                return State;
            }
            
            Context = new TransformContext(searchBotManagerContext.Transform);
            
            var player = Context.Transform;
            //var botTransform = OwnerBotModel.CachedTransform;
            var handTransform = OwnerBotModel.CachedHandTransform;
            
            //var botDir = player.position - botTransform.position;
            var handDir = player.position - handTransform.position;
            var handLookRotation = Quaternion.LookRotation(handDir, Vector3.up);
            
            handTransform.rotation = Quaternion.Slerp(handTransform.rotation, handLookRotation, 2f * Time.deltaTime);
            
            //var isVisible = Physics.Raycast(botTransform.position, botDir, out var hit, Mathf.Infinity);
            
            var attackDot = Vector3.Dot(handTransform.forward, Vector3.Normalize(handDir));
            if (attackDot >= 0.9)
            {
                
            }

            return BotManagerState.Running;
        }
    }
}