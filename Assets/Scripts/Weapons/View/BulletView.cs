using System;
using UnityEngine;

namespace BeeGood.Views
{
    public class BulletView : BaseView, IDisposable
    {
        [SerializeField] private BulletData bulletData;
        private event Action<Collision> OnCollisionEnterEvent;
        
        public BulletData BulletData => bulletData;
        
        private void OnCollisionEnter(Collision collision)
        {
            OnCollisionEnterEvent?.Invoke(collision);
        }

        public void SubscribeOnTriggerEnterEvent(Action<Collision> action)
        {
            OnCollisionEnterEvent += action;
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