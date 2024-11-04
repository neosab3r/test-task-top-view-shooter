using System;
using UnityEngine;

namespace BeeGood.Views
{
    public class WeaponView : BaseView
    {
        [SerializeField] private BulletView bulletPrefab;
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private Transform bulletStartPosition;
        
        public WeaponData WeaponData() => weaponData;
        public BulletView BulletPrefab() => bulletPrefab;
        public Transform GetBulletStartPosition() => bulletStartPosition;
    }
}

[Serializable]
public class WeaponData
{
    public float FireRate;
}