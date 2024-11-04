using System;
using UnityEngine;

namespace BeeGood.Views
{
    public class BulletView : BaseView, IDisposable
    {
        [SerializeField] private BulletData bulletData;
        private event Action<Collision> OnCollisionEnterEvent;
        
        public BulletData BulletData => bulletData;
        
        public void SubscribeOnCollisionEvent(Action<Collision> action)
        {
            OnCollisionEnterEvent += action;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            OnCollisionEnterEvent?.Invoke(collision);
        }
        
        public void Dispose()
        {
            OnCollisionEnterEvent = null;
        }
    }

    [Serializable]
    public class BulletData
    {
        public int maxRicochets;
        public float bulletSpeed;
    }
}