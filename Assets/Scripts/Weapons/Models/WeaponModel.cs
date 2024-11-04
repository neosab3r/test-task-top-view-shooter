using BeeGood.Systems;
using BeeGood.Views;
using UnityEngine;
using UnityEngine.Assertions;

namespace BeeGood.Models
{
    public class WeaponModel : BaseModel<WeaponView>
    {
        public IModel OwnerPlayer { get; protected set; }
        private BulletSystem BulletSystem;
        private WeaponData weaponData;
        private BulletView cachedBulletPrefab;
        private bool isReloading;
        private float reloadTime;
        public WeaponModel(WeaponView view, IModel ownerPlayer, BulletSystem bulletSystem) : base(view)
        {
            weaponData = view.WeaponData();
            cachedBulletPrefab = view.BulletPrefab();
            OwnerPlayer = ownerPlayer;
            BulletSystem = bulletSystem;
        }

        public void SetOwnerPlayer(IModel ownerPlayer)
        {
            OwnerPlayer = ownerPlayer;
        }

        public bool IsReloading()
        {
            return isReloading;
        }
        
        public void Shoot(string tagEntity, Vector3 dir)
        {
            Assert.AreNotEqual(OwnerPlayer, null, $"{nameof(WeaponModel)} has empty OwnerPlayer.");
            Assert.AreNotEqual(tagEntity, "", $"{nameof(WeaponModel)} in {OwnerPlayer.GetType().Name} has empty EnemyTagEntity. The bullet won't do any damage");
            var bulletModel = BulletSystem.CreateBulletView(cachedBulletPrefab, View.GetBulletStartPosition(), OwnerPlayer);
            bulletModel.TargetTagEntity = tagEntity;
            if (dir != Vector3.zero)
            {
                bulletModel.SetDirection(dir);
            }

            SetReload(weaponData.FireRate);
        }

        private void SetReload(float fireRate)
        {
            reloadTime = 60 / fireRate;
            isReloading = true;
        }

        public void Update(float dt)
        {
            if (isReloading)
            {
                reloadTime -= dt;
                if (reloadTime <= 0)
                {
                    Debug.LogError("Reloaded");
                    isReloading = false;
                }
            }
        }
    }
}