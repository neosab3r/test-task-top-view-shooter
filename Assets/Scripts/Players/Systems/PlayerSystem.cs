using BeeGood.Extensions;
using BeeGood.Models;
using BeeGood.Views;
using UnityEngine;

namespace BeeGood.Systems
{
    public class PlayerSystem : BaseSystem<PlayerModel, PlayerView>
    {
        private WeaponSystem weaponSystem;
        public override bool HasUpdate() => true;
        public override void Initialize()
        {
            weaponSystem = EntrySystem.Instance.Get<WeaponSystem>();
        }
        
        public override void Update(float dt)
        {
            GetInputActions();
            
            if (Input.GetMouseButtonDown(0))
            {
                foreach (var model in Models)
                {
                    model.TryShoot();
                }
            }
        }

        public override PlayerModel AddView(PlayerView view)
        {
            var weaponModel = weaponSystem.GetModelByView(view.GetWeaponView);
            var playerModel = new PlayerModel(view)
            {
                WeaponModel = weaponModel
            };
            weaponModel.OwnerPlayer = playerModel;
            Models.Add(playerModel);

            return playerModel;
        }

        private void GetInputActions()
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var mousePosition = MouseExtension.GetMousePosition();
            foreach (var model in Models)
            {
                model.Move(horizontal, vertical);
                model.Look(mousePosition);
            }
        }
    }
}