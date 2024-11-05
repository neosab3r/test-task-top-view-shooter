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
        [SerializeField] private int maxBotCounts;
        [SerializeField] private BotView botPrefab;
        [SerializeField] private Transform botSpawnPoint;
        public static GameManager Instance;

        private int botSpawnRadius;
        
        public static void Initialize()
        {
            if (Instance != null)
            {
                return;
            }

            Instance = FindObjectOfType<GameManager>();
            Instance.InternalInitialize();
        }

        private void InternalInitialize()
        {
            botSpawnRadius = maxBotCounts;
            DontDestroyOnLoad(this);
        }
        
        public void StartGame(int botCount, bool stopSystems = false)
        {
            if (stopSystems)
            {
                EntrySystem.Instance.StopUpdateSystems();
            }
            Debug.LogError("StopSystems");
            var playerView = InstantiateExtension.Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
            mainCamera.Follow = playerView.transform;

            for (var i = 0; i < botCount; i++)
            {
                var segment = 2 * Mathf.PI * i / botCount;
                var horizontal = Mathf.Cos(segment);
                var vertical = Mathf.Sin(segment);
                var direction = new Vector3(horizontal, 0, vertical);
                var worldPosition = botSpawnPoint.position + direction * botSpawnRadius;
                var botView = InstantiateExtension.Instantiate(botPrefab, worldPosition, Quaternion.identity);
                Debug.LogError($"Spawn Bot: {botView.name}");
            }

            EntrySystem.Instance.StartUpdateSystems();
            Debug.LogError("StartSystems");
        }

        public void EndRound(bool isPlayerWin)
        {
            EntrySystem.Instance.StopUpdateSystems();
            EntrySystem.Instance.DisposeSystems();
            
            ScoreManager.Instance.IncreaseStage(isPlayerWin);
            var botCounts = ScoreManager.Instance.PlayerScore + 1;
            Debug.LogError($"Botcounts: {botCounts}");
            
            if (!isPlayerWin)
            {
                botCounts -= 1;
            }

            if (botCounts < 1)
            {
                botCounts = 1;
            }

            if (botCounts > maxBotCounts)
            {
                botCounts = maxBotCounts;
            }
            Debug.LogError($"End botCount: {botCounts}");

            StartGame(botCounts);
        }
    }
}