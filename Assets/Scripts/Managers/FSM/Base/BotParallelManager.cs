

using System.Collections.Generic;
using BeeGood.Models;
using UnityEngine;

namespace BeeGood.Managers
{
    public class BotParallelManager : BaseBotManager<EmptyContext> 
    {
        public BotParallelManager(BotModel botModel, List<IBotManager> managers) : base(botModel, managers) { }

        public override BotManagerState Evaluate()
        {
            var anyChildIsRunning = false;
            var hasChildIsFailed = false;

            foreach (var manager in ChildManagers)
            {
                switch (manager.Evaluate())
                {
                    case BotManagerState.Failed:
                        hasChildIsFailed = true;
                        continue;
                    case BotManagerState.Success:
                    case BotManagerState.Running:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        Debug.LogError($"{nameof(BotParallelManager)} has an unexpected state after evaluating manager: {manager.GetType().Name}. Return Success state..");
                        State = BotManagerState.Success;
                        return State;
                }
            }

            State = hasChildIsFailed ? BotManagerState.Failed : BotManagerState.Success;
            State = anyChildIsRunning ? BotManagerState.Success : State;
            Debug.LogError($"[{nameof(BotParallelManager)}] returned {State}.");
            return State;
        }
    }
}