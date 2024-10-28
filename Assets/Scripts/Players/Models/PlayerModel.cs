using BeeGood.Extensions;
using BeeGood.Views;
using UnityEngine;

namespace BeeGood.Models
{
    public class PlayerModel : BaseModel<PlayerView>
    {
        public WeaponModel WeaponModel;

        private Transform cachedTransform;
        private Transform cachedHandTransform;
        private const float MoveSpeed = 5f;

        public PlayerModel(PlayerView view) : base(view)
        {
            cachedTransform = view.transform;
            cachedHandTransform = view.GetHandTransform;
        }

        public void Move(float horizontal, float vertical)
        {
            cachedTransform.position += (-cachedTransform.forward * horizontal + cachedTransform.right * vertical) * Time.fixedDeltaTime * MoveSpeed;
        }

        public void Look(Vector3 position)
        {
            var direction = position - cachedHandTransform.position;
            direction.y = 0;
            cachedHandTransform.forward = direction;
        }

        public void TryShoot()
        {
            WeaponModel.Shoot(TagExtension.BotTag);
        }
    }
}