using System;
using BeeGood.Systems;
using BeeGood.Views;
using Pathfinding;
using UnityEngine;

namespace BeeGood.View
{
    public class BotView : BaseView
    {
        [SerializeField] private AIDestinationSetter aiDestinationSetter;
        [SerializeField] private WeaponView weaponView;
        [SerializeField] private Transform handTransform;
        [SerializeField] private AIPath aiPath;
        private event Action<Collider> OnTriggerEnterEvent;
        private event Action<Collider> OnTriggerExitEvent;

        public AIPath GetAIPath() => aiPath;
        public Transform GetHandTransform() => handTransform;
        public AIDestinationSetter GetAIDestinationSetter() => aiDestinationSetter;
        public WeaponView GetWeaponView() => weaponView;
        
        private void Start()
        {
            BaseEntrySystems.SubscribeOnAllSystemsInitialized(() =>
            {
                EntrySystem.Instance.Get<BotSystem>().AddView(this);
            });
        }
        
        public void SubscribeOnTriggerEnterEvent(Action<Collider> action)
        {
            OnTriggerEnterEvent += action;
        }
        
        public void SubscribeOnTriggerExitEvent(Action<Collider> action)
        {
            OnTriggerExitEvent += action;
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitEvent?.Invoke(other);
        }

        private void OnDrawGizmos()
        {
            //Gizmos.DrawWireSphere(transform.position, 9f);
        }
    }
}