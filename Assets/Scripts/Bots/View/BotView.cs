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

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, 9f);
        }
    }
}