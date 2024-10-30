using System;
using UnityEngine;

namespace BeeGood.Views
{
    public class WeaponView : BaseView
    {
        public WeaponData WeaponData => weaponData;
        public BulletView BulletPrefab => bulletPrefab;
        public Transform GetBulletStartPosition => bulletStartPosition;
        
        [SerializeField] private BulletView bulletPrefab;
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private Transform bulletStartPosition;
    }
}

[Serializable]
public class WeaponData
{
    public float FireRate;
}