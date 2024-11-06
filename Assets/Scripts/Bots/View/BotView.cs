using System;
using BeeGood.Systems;
using BeeGood.UI;
using BeeGood.Views;
using Pathfinding;
using UnityEngine;

namespace BeeGood.View
{
    [Serializable]
    public class BotData
    {
        public DifficultType DifficultType;
        public float MaxEvasionSpeed;
        public float DefaultSpeed;
        public float SearchZone;
        public float EvasionZone;
        public float AttackAngle;
        public float RotateWeaponSpeed;
    }
    
    public class BotView : BaseView
    {
        [SerializeField] private SphereCollider evasionCollider;
        [SerializeField] private AIDestinationSetter aiDestinationSetter;
        [SerializeField] private WeaponView weaponView;
        [SerializeField] private Transform handTransform;
        [SerializeField] private AIPath aiPath;

        [SerializeField] private BotData easyBotData;
        [SerializeField] private BotData hardBotData;
        private BotData currentBotData;
        private event Action<Collider> OnTriggerEnterEvent;
        private event Action<Collider> OnTriggerExitEvent;

        public BotData GetBotData() => currentBotData;
        public AIPath GetAIPath() => aiPath;
        public Transform GetHandTransform() => handTransform;
        public AIDestinationSetter GetAIDestinationSetter() => aiDestinationSetter;
        public WeaponView GetWeaponView() => weaponView;
        
        public void Setup(DifficultType type)
        {
            currentBotData = type == DifficultType.Easy ? easyBotData : hardBotData;
            aiPath.maxSpeed = currentBotData.DefaultSpeed;
            evasionCollider.radius = currentBotData.EvasionZone;
            BaseEntrySystems.SubscribeOnAllSystemsInitialized(() =>
            {
                EntrySystem.Instance.Get<BotSystem>().AddView(this);
            });
        }
        
        public void SubscribeOnTriggerEnterEvent(Action<Collider> action)
        {
            OnTriggerEnterEvent += action;
        }
        
        public void SubscribeOnTriggerExitEvent(Action<Collider> action)
        {
            OnTriggerExitEvent += action;
        }

        private void OnTriggerEnter(Collider other)
        {
            OnTriggerEnterEvent?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            OnTriggerExitEvent?.Invoke(other);
        }
    }
}