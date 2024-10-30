using System.Collections.Generic;
using BeeGood.Models;
using UnityEngine;

namespace BeeGood.Managers
{
    public class BotSequenceManager : BaseBotManager<EmptyContext>
    {
        public BotSequenceManager(BotModel botModel, List<IBotManager> managers) : base(botModel, managers) { }

        public override BotManagerState Evaluate()
        {
            var anyChildIsRunning = false;

            foreach (var manager in ChildManagers)
            {
                switch (manager.Evaluate())
                {
                    case BotManagerState.Failed:
                        State = BotManagerState.Failed;
                        return State;
                    case BotManagerState.Success:
                        continue;
                    case BotManagerState.Running:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        Debug.LogError($"{nameof(BotSequenceManager)} has an unexpected state after evaluating manager: {manager.GetType().Name}. Return Success state..");
                        State = BotManagerState.Success;
                        return State;
                }
            }

            State = anyChildIsRunning ? BotManagerState.Running : BotManagerState.Success;
            return State;
        }
    }
}