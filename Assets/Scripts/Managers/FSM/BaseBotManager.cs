using System.Collections.Generic;
using UnityEngine;

namespace BeeGood.Managers
{
    public abstract class BaseBotManager<T> : IBotManager where T : IManagerContext
    {
        public T Context { get; protected set; }
        public abstract bool IsSetContext();
        public IBotManager Parent { get; set; }
        public BotManagerState State { get; protected set; }
        protected List<IBotManager> ChildManagers = new();

        protected BaseBotManager()
        {
            Parent = null;
        }

        protected BaseBotManager(List<IBotManager> managers)
        {
            foreach (var childManager in managers)
            {
                childManager.Parent = this;
                ChildManagers.Add(childManager);
            }
        }

        public void SetContext(IManagerContext context)
        {
            Context = (T)context;
        }

        public virtual BotManagerState Evaluate() => BotManagerState.Failed;
    }

    public class EmptyContext : IManagerContext
    {
    }

    public class BotSequenceManager : BaseBotManager<EmptyContext>
    {
        public override bool IsSetContext()
        {
            return true;
        }

        public override BotManagerState Evaluate()
        {
            foreach (var manager in ChildManagers)
            {
                if (manager.IsSetContext())
                {
                    switch (manager.Evaluate())
                    {
                        case BotManagerState.Failed:
                            State = BotManagerState.Failed;
                            return State;
                        case BotManagerState.Success:
                            State = BotManagerState.Success;
                            continue;
                        case BotManagerState.Running:
                            State = BotManagerState.Running;
                            return State;
                    }
                }

                State = BotManagerState.Failed;
                return State;
            }

            Debug.LogError($"No managers set for this SequenceManager. Return Failed State");
            State = BotManagerState.Failed;
            return State;
        }
    }
}

public enum BotManagerState
{
    Failed,
    Success,
    Running
}

public interface IManagerContext
{

}

public interface IBotManager
{
    IBotManager Parent { get; set; }
    bool IsSetContext();
    void SetContext(IManagerContext context);
    BotManagerState Evaluate();
}