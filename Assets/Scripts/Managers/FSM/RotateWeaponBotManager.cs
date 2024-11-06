using BeeGood.Managers.Contexts;
using UnityEngine;

namespace BeeGood.Managers
{
    public class RotateWeaponBotManager : BaseBotManager<EmptyContext>
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

            var context = searchBotManagerContext;
            if (context.IsVisible == false)
            {
                Debug.LogError($"Not Rotate TO Player");
                State = BotManagerState.Running;
                return State;
            }
            
            var handTransform = OwnerBotModel.CachedHandTransform;
            var handDir = context.HandToPlayerDirection;
            
            var handLookRotation = Quaternion.LookRotation(handDir, Vector3.up);
            
            handTransform.rotation = Quaternion.Slerp(handTransform.rotation, handLookRotation, OwnerBotModel.Data.RotateWeaponSpeed * Time.deltaTime);
            Debug.LogError($"Rotate TO Player");

            return BotManagerState.Running;
        }
    }
}