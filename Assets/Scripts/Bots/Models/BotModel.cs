using System.Collections.Generic;
using BeeGood.View;
using UnityEngine;

namespace BeeGood.Models
{
    public class BotModel : BaseModel<BotView>
    {
        private Transform cachedTransform;
        private List<IBotManager> botManagers = new();

        private bool isDeath;
        public BotModel(BotView view) : base(view)
        {
            cachedTransform = view.transform;
        }

        public void UpdateBotManagers()
        {
            foreach (var manager in botManagers)
            {
                if(manager.IsSetContext())
                {
                    switch (manager.Evaluate())
                    {
                        case BotManagerState.Failed
                    }
                }
            }
        }

        public override void Dispose()
        {
            botManagers.Clear();
        }
    }
}