﻿using BeeGood.Models;
using BeeGood.View;

namespace BeeGood.Systems
{
    public class BotSystem : BaseSystem<BotModel, BotView>
    {
        private WeaponSystem weaponSystem;
        private BulletSystem bulletSystem;
        public override bool HasUpdate() => true;

        public override void Initialize()
        {
            weaponSystem = EntrySystem.Instance.Get<WeaponSystem>();
            bulletSystem = EntrySystem.Instance.Get<BulletSystem>();
        }

        public override BotModel AddView(BotView view)
        {
            var weaponModel = weaponSystem.AddView(view.GetWeaponView());
            var botModel = new BotModel(view, bulletSystem, weaponModel);
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

        public BotModel GetBotModel(BotView botView)
        {
            foreach (var botModel in Models)
            {
                if (botModel.View == botView)
                {
                    return botModel;
                }
            }

            return default;
        }
        public void KillBot(BotModel botModel)
        {
            botModel.Dispose();
            RemoveModel(botModel);
        }

        public bool IsRemainingBots()
        {
            return Models.Count > 0;
        }
        
        public override void Dispose()
        {
            for (var i = Models.Count - 1; i >= 0; i--)
            {
                var botModel = Models[i];
                botModel.Dispose();
                RemoveModel(botModel);
            }
        }

    }
}