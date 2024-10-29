using BeeGood.Models;
using UnityEngine;

namespace BeeGood.Managers
{
    public class IdleBotContext : IManagerContext
    {
        public Transform PointToMove;

        public IdleBotContext(Transform pointToMove)
        {
            PointToMove = pointToMove;
        }
    }
    
    public class IdleBotManager : BaseBotManager<IdleBotContext>
    {
        private BotModel parentBotModel;
        
        public IdleBotManager(BotModel botModel)
        {
            parentBotModel = botModel;
        }
        
        public override bool IsSetContext()
        {
            var point = PatrolPointsManager.Instance.GetRandomPatrolPoint();
            var context = new IdleBotContext(point);
            SetContext(context);
            return true;
        }

        public override BotManagerState Evaluate()
        {
            return base.Evaluate();
        }
    }
}