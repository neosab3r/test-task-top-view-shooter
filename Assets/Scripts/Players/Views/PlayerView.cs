using BeeGood.Systems;
using UnityEngine;

namespace BeeGood.Views
{
    public class PlayerView : BaseView
    {
        [SerializeField] private Transform handTransform;
        [SerializeField] private WeaponView weaponView;
        
        public Transform GetHandTransform() => handTransform;
        public WeaponView GetWeaponView() => weaponView;

        private void Start()
        {
            BaseEntrySystems.SubscribeOnAllSystemsInitialized(() =>
            {
                EntrySystem.Instance.Get<PlayerSystem>().AddView(this);
            });
        }
    }
}