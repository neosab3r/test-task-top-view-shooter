using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEntrySystems : MonoBehaviour
{
    private readonly List<ISystem> systems = new();
    private static event Action OnAllSystemsInitializedEvent;
    private static bool isInitialized;
    protected virtual void Initialize()
    {
        InitializeSystems();
        isInitialized = true;
        OnAllSystemsInitializedEvent?.Invoke();
        OnAllSystemsInitializedEvent = null;
    }
    
    private void InitializeSystems()
    {
        foreach (var system in systems)
        {
            system.Initialize();
        }
    }

    public virtual void PostInitSystems()
    {
        foreach (var system in systems)
        {
            
        }
    }

    public void Update()
    {
        var dt = Time.deltaTime;
        foreach (var system in systems)
        {
            if (system.HasUpdate())
            {
                system.Update(dt);
            }
        }
    }

    protected void AddSystem(ISystem system)
    {
        if (systems.Contains(system))
        {
            Debug.LogError($"[{nameof(BaseEntrySystems)}] has already contains {nameof(system)}");
            return;
        }
        
        systems.Add(system);
    }
    
    public TK Get<TK>() where TK : ISystem
    {
        foreach (var system in systems)
        {
            if (system is TK tSystem)
            {
                return tSystem;
            }
        }

        Debug.LogError($"[{nameof(BaseEntrySystems)}] cannot get system. Cached list has not contains {typeof(TK)}");
        return default;
    }

    public static void SubscribeOnAllSystemsInitialized(Action callback)
    {
        if (isInitialized)
        {
            callback?.Invoke();
        }
        OnAllSystemsInitializedEvent += callback;
    }

    private void OnDestroy()
    {
        foreach (var system in systems)
        {
            system.Dispose();
        }
    }
}

public abstract class BaseSystem<T, TK>: ISystem where T: BaseModel<TK>
where TK: BaseView
{
    protected List<T> Models = new();
    public abstract void Initialize();
    public abstract T AddView(TK view);

    public virtual void RemoveModel(T model)
    {
        Models.Remove(model);
    }
    public abstract bool HasUpdate();
    public virtual void Update(float dt) { }

    public virtual void Dispose() { }
}

public interface ISystem: IDisposable
{
    bool HasUpdate();
    void Initialize();
    void Update(float dt);
}

public class BaseModel<T>: IModel, IDisposable where T : BaseView
{
    public T View { get; protected set; }

    protected BaseModel(T view)
    {
        View = view;
    }

    public virtual void Dispose() { }
}

public interface IModel
{
    
}

public abstract class BaseView: MonoBehaviour 
{
    
}
