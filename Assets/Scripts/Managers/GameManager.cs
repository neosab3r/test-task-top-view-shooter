using BeeGood.Extensions;
using BeeGood.View;
using BeeGood.Views;
using Cinemachine;
using UnityEngine;

namespace BeeGood.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera mainCamera;
        [SerializeField] private PlayerView playerPrefab;
        [SerializeField] private Transform playerSpawnPoint;

        [SerializeField] private BotView botPrefab;
        [SerializeField] private Transform botSpawnPoint;
        public static GameManager Instance;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            
            DontDestroyOnLoad(this);
            Instance = this;
        }

        public void StartGame()
        {
            EntrySystem.Instance.StopUpdateSystems();
            Debug.LogError("StopSystems");
            var playerView = InstantiateExtension.Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
            mainCamera.Follow = playerView.transform;
            
            var botView = InstantiateExtension.Instantiate(botPrefab, botSpawnPoint.position, Quaternion.identity);

            EntrySystem.Instance.StartUpdateSystems();
            Debug.LogError("StartSystems");
        }

        public void EndRound(bool isPlayerWin)
        {
            EntrySystem.Instance.StopUpdateSystems();
            EntrySystem.Instance.DisposeSystems();
            
            StartGame();
        }
    }
}