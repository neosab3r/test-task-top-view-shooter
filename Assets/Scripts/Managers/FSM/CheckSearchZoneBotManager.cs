using System;
using BeeGood.Managers.Contexts;
using UnityEngine;

namespace BeeGood.Managers
{
    public class CheckManagerContext : TransformContext
    {
        public bool IsVisible;
        public Vector3 BotToPlayerDirection;
        public Vector3 HandToPlayerDirection;
        public Vector3 BulletToPlayerDirection;

        public CheckManagerContext(Transform transform, bool isVisible, Vector3 botToPlayerDirection, Vector3 handToPlayerDirection, Vector3 bulletToPlayerDirection) : base(transform)
        {
            IsVisible = isVisible;
            BotToPlayerDirection = botToPlayerDirection;
            HandToPlayerDirection = handToPlayerDirection;
            BulletToPlayerDirection = bulletToPlayerDirection;
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
                var handTransform = OwnerBotModel.CachedHandTransform;
                var startBulletTransform = OwnerBotModel.WeaponModel.View.GetBulletStartPosition();
                var playerTransform = colliders[0].gameObject.transform;
                var botToPlayerDirection = playerTransform.position - botTransform.position;
                var handToPlayerDirection = playerTransform.position - handTransform.position;
                var bulletStartPositionToPlayerDirection = playerTransform.position - startBulletTransform.position;

                var isVisible = false;
                var hits = Physics.RaycastAll(handTransform.position, handToPlayerDirection);

                Array.Sort(hits, (x,y) => x.distance.CompareTo(y.distance));
                //Debug.DrawRay(handTransform.position, handToPlayerDirection, Color.green);
                
                if (hits.Length <= 0)
                {
                    State = BotManagerState.Running;
                    return State;
                }
                
                if (hits[0].collider.gameObject.transform == playerTransform)
                {
                    //Debug.DrawLine(handTransform.position, hits[0].point, Color.red);
                    Debug.LogError($"Hit: {hits[0].transform.name}");
                    isVisible = true;
                }
                
                var context = new CheckManagerContext(playerTransform, isVisible, botToPlayerDirection, handToPlayerDirection, bulletStartPositionToPlayerDirection);
                
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