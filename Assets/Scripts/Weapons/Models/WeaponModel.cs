using BeeGood.Systems;
using BeeGood.Views;
using UnityEngine;
using UnityEngine.Assertions;

namespace BeeGood.Models
{
    public class WeaponModel : BaseModel<WeaponView>
    {
        public BulletSystem BulletSystem;
        public IModel OwnerPlayer;
        private WeaponData weaponData;
        private BulletView cachedBulletPrefab;
        public WeaponModel(WeaponView view) : base(view)
        {
            weaponData = view.WeaponData;
            cachedBulletPrefab = view.BulletPrefab;
        }

        public void Shoot(string tagEntity)
        {
            Assert.AreNotEqual(tagEntity, "", $"{nameof(WeaponModel)} in {OwnerPlayer.GetType().Name} has empty EnemyTagEntity. The bullet won't do any damage");
            BulletSystem.CreateBulletView(cachedBulletPrefab, View.GetBulletStartPosition, tagEntity, OwnerPlayer);
        }
    }
}