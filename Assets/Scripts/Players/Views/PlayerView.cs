using BeeGood.Systems;
using UnityEngine;

namespace BeeGood.Views
{
    public class PlayerView : BaseView
    {
        public Transform GetHandTransform => handTransform;
        public WeaponView GetWeaponView => weaponView;

        [SerializeField] private Transform handTransform;
        [SerializeField] private WeaponView weaponView;

        private void Start()
        {
            BaseEntrySystems.SubscribeOnAllSystemsInitialized(() =>
            {
                EntrySystem.Instance.Get<PlayerSystem>().AddView(this);
            });
        }
    }
}