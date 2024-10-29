using System.Collections.Generic;
using BeeGood.Managers;
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
            SetupManagers(new List<IBotManager>{
                new IdleBotManager(this)
            });

        }

        public void SetupManagers(List<IBotManager> root)
        {
            botManagers.Clear();
            botManagers.AddRange(root);
        }

        public void UpdateBotManagers()
        {
            if (botManagers.Count < 0)
            {
                return;
            }

            for (int i = botManagers.Count - 1; i >= 0; i--)
            {
                var manager = botManagers[i];
                if(manager.IsSetContext())
                {
                    switch (manager.Evaluate())
                    {
                        case BotManagerState.Failed:
                            continue;
                        case BotManagerState.Success:
                            return;
                        case BotManagerState.Running:
                            return;
                        default:
                            Debug.LogError($"Unexpected state for this {nameof(manager)}. Skipping..");
                            return;
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