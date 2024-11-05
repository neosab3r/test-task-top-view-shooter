using BeeGood.Extensions;
using BeeGood.Views;
using UnityEngine;

namespace BeeGood.Models
{
    public class PlayerModel : BaseModel<PlayerView>
    {
        public WeaponModel WeaponModel { get; protected set; }

        public Transform CachedTransform { get; protected set; }
        private Transform cachedHandTransform;
        private const float MoveSpeed = 5f;

        public PlayerModel(PlayerView view, WeaponModel weaponModel) : base(view)
        {
            CachedTransform = view.transform;
            cachedHandTransform = view.GetHandTransform();
            WeaponModel = weaponModel;
        }

        public void Move(float horizontal, float vertical)
        {
            CachedTransform.position += (-CachedTransform.forward * horizontal + CachedTransform.right * vertical) * Time.deltaTime * MoveSpeed;
        }

        public void Look(Vector3 position)
        {
            var direction = position - cachedHandTransform.position;
            direction.y = 0;
            cachedHandTransform.forward = direction;
        }

        public void TryShoot()
        {
            if (WeaponModel.IsReloading())
            {
                return;
            }
            
            WeaponModel.Shoot(TagExtension.BotTag, Vector3.zero);
        }

        public override void Dispose()
        {
            WeaponModel = null;
            CachedTransform = null;
            cachedHandTransform = null;
            if (View.gameObject != null)
            {
                Object.Destroy(View.gameObject);
            }
            View = null;
        }
    }
}