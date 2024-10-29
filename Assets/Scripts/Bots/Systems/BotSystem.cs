using BeeGood.Models;
using BeeGood.View;

namespace BeeGood.Systems
{
    public class BotSystem : BaseSystem<BotModel, BotView>
    {
        public override bool HasUpdate() => true;

        public override void Initialize()
        {
        }

        public override BotModel AddView(BotView view)
        {
            var model = new BotModel(view);
            Models.Add(model);
            return model;
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