using System.Collections.Generic;
using BeeGood.Models;
using UnityEngine;

namespace BeeGood.Managers
{
    public class BotSelectorManager : BaseBotManager<EmptyContext>
    {
        public BotSelectorManager(BotModel botModel, List<IBotManager> managers) : base(botModel, managers) {}

        public override BotManagerState Evaluate()
        {
            foreach (var manager in ChildManagers)
            {
                switch (manager.Evaluate())
                {
                    case BotManagerState.Failed:
                        continue;
                    case BotManagerState.Success:
                        State = BotManagerState.Success;
                        return State;
                    case BotManagerState.Running:
                        State = BotManagerState.Running;
                        return State;
                    default:
                        Debug.LogError($"{nameof(BotSelectorManager)} has Unexpected state for this manager {nameof(manager)}. Skipping..");
                        return BotManagerState.Failed;
                }
            }

            State = BotManagerState.Failed;
            return State;
        }
    }
}