using System;
using System.Collections.Generic;
using BeeGood.Models;
using UnityEngine;

namespace BeeGood.Managers
{
    public abstract class BaseBotManager<T> : IBotManager where T : IManagerContext
    {
        public T Context { get; protected set; }
        public List<IBotManager> ChildManagers { get; protected set; } = new();
        public IBotManager Parent { get; set; }
        public BotModel OwnerBotModel { get; set; }
        public BotManagerState State { get; protected set; }

        protected BaseBotManager()
        {
            Parent = null;
        }

        protected BaseBotManager(BotModel botModel, List<IBotManager> managers)
        {
            OwnerBotModel = botModel;
            foreach (var childManager in managers)
            {
                childManager.Parent = this;
                childManager.OwnerBotModel = botModel;
                ChildManagers.Add(childManager);
            }
        }

        public void SetContext(IManagerContext context)
        {
            Context = (T)context;
        }

        public virtual BotManagerState Evaluate() => BotManagerState.Failed;
        
        public TK GetManager<TK>()
        {
            foreach (var childManager in ChildManagers)
            {
                if (childManager is TK botManager)
                {
                    return botManager;
                }
            }

            Debug.LogError($"Cannot get child manager in Manager: {GetType().Name}. List has not contains manager: {typeof(TK)}");
            return default;
        }

        public virtual void Dispose()
        {
            foreach (var childManager in ChildManagers)
            {
                childManager.Dispose();
            }
            ChildManagers.Clear();
            ChildManagers = null;
            OwnerBotModel = null;
            Context = default;
            Parent = null;
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

public interface IBotManager : IDisposable
{
    List<IBotManager> ChildManagers { get; }
    IBotManager Parent { get; set; }
    BotModel OwnerBotModel { get; set; }
    void SetContext(IManagerContext context);
    BotManagerState Evaluate();
    TK GetManager<TK>();
}