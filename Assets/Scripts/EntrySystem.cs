using BeeGood.Managers;
using BeeGood.Systems;
using UnityEngine;

namespace BeeGood
{
    public class EntrySystem : BaseEntrySystems
    {
        public static EntrySystem Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError($"{nameof(EntrySystem)} already exists.");
                return;
            }
            Instance = this;
            
            Initialize();
            
            GameManager.Instance.StartGame();
        }

        protected override void Initialize()
        {
            AddSystem(new PlayerSystem());
            AddSystem(new WeaponSystem());
            AddSystem(new BulletSystem());
            AddSystem(new BotSystem());
            
            base.Initialize();
        }
    }
}

