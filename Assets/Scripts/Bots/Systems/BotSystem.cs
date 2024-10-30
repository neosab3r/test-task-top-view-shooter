using BeeGood.Models;
using BeeGood.View;

namespace BeeGood.Systems
{
    public class BotSystem : BaseSystem<BotModel, BotView>
    {
        private WeaponSystem weaponSystem;

        public override bool HasUpdate() => true;

        public override void Initialize()
        {
            weaponSystem = EntrySystem.Instance.Get<WeaponSystem>();
        }

        public override BotModel AddView(BotView view)
        {
            var weaponModel = weaponSystem.AddView(view.GetWeaponView());
            var botModel = new BotModel(view, weaponModel);
            weaponModel.SetOwnerPlayer(botModel);
            botModel.CreateManagers();
            
            Models.Add(botModel);
            return botModel;
        }

        public override void Update(float dt)
        {
            for (var i = Models.Count - 1; i >= 0; i--)
            {
                var bot = Models[i];
                bot.UpdateBotManagers();
            }
        }
    }
}