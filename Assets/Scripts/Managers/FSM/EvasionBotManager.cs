using System.Linq;
using BeeGood.Models;
using Pathfinding;
using UnityEngine;

namespace BeeGood.Managers
{
    public class EvasionBotManager : BaseBotManager<EmptyContext>
    {
        private ABPath currentPath = null;
        private BulletModel currentBullet = null;
        private const float MinEndDistance = 0.1f;
        private const float MaxEvasionSpeed = 4f;
        private const float DefaultEvasionSpeed = 2f;
        public override BotManagerState Evaluate()
        {
            var aiPath = OwnerBotModel.View.GetAIPath();
            if (OwnerBotModel.NearBulletModels.Count == 0)
            {
                TryClearData(aiPath);

                State = BotManagerState.Failed;
                return State;
            }
            
            var botTransform = OwnerBotModel.CachedTransform; 
            if (currentBullet != null && currentBullet.ReadyToDestroy == false)
            {
                var dirBulletToBot = botTransform.position - currentBullet.CachedViewTransform.position;
                var dot = Vector3.Dot(currentBullet.GetDirection().normalized, dirBulletToBot.normalized);
                if (dot > 0.6)
                {
                    State = BotManagerState.Running;
                    return State;
                }
            }
            
            currentBullet = GetNearBulletModelByDot(botTransform);
            if (currentBullet == null)
            {
                TryClearData(aiPath);
                
                State = BotManagerState.Failed;
                return State;
            }
            
            var bulletTransform = currentBullet.CachedViewTransform;
            var dirBotToBullet = bulletTransform.position - botTransform.position;

            var cross = Vector3.Cross(dirBotToBullet, dirBotToBullet + Vector3.up);
            
            var right = ABPath.Construct(botTransform.position, botTransform.position + new Vector3(cross.x, 0, cross.z));
            AstarPath.StartPath(right);
            right.BlockUntilCalculated();
                   
            var left = ABPath.Construct(botTransform.position, botTransform.position - new Vector3(cross.x, 0, cross.z));
            AstarPath.StartPath(left);
            left.BlockUntilCalculated();

            currentPath = right.GetTotalLength() > left.GetTotalLength() ? right : left;

            if (currentPath != null)
            {
                for (int j = 0; j < currentPath.vectorPath.Count - 1; j++) {
                    Debug.DrawLine(currentPath.vectorPath[j], currentPath.vectorPath[j + 1], Color.red, 10);
                }

                aiPath.maxSpeed = MaxEvasionSpeed;
                aiPath.endReachedDistance = MinEndDistance;
                aiPath.canSearch = false;
                aiPath.SetPath(currentPath);
                State = BotManagerState.Running;
                return State;
            }
            
            State = BotManagerState.Failed;
            return State;
        }

        private void TryClearData(AIPath aiPath)
        {
            if (currentPath != null)
            {
                Debug.LogError("STOP EXIT");
                aiPath.maxSpeed = DefaultEvasionSpeed;
                aiPath.canSearch = true;
                currentPath = null;
                currentBullet = null;
            }
        }

        private BulletModel GetNearBulletModelByDot(Transform botTransform)
        {
            return OwnerBotModel.NearBulletModels.Values.FirstOrDefault(bulletModel =>
            {
                if (bulletModel.ReadyToDestroy)
                {
                    return false;
                }
                var dirBulletToBot = botTransform.position - bulletModel.CachedViewTransform.position;
                var dot = Vector3.Dot(bulletModel.GetDirection().normalized, dirBulletToBot.normalized);
                return dot > 0.9;
            });
        }
    }
}