using BeeGood.Models;
using BeeGood.Views;

namespace BeeGood.Systems
{
    public class WeaponSystem : BaseSystem<WeaponModel, WeaponView>
    {
        private BulletSystem bulletSystem;
        public override bool HasUpdate() => false;

        public override void Initialize()
        {
            bulletSystem = EntrySystem.Instance.Get<BulletSystem>();
        }

        public override WeaponModel AddView(WeaponView view)
        {
            var weaponModel = new WeaponModel(view, null, bulletSystem);
            Models.Add(weaponModel);

            return weaponModel;
        }

        public WeaponModel GetModelByView(WeaponView view)
        {
            return Models.Find(m => m.View == view);
        }
    }
}