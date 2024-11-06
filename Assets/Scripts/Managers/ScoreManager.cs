using BeeGood.UI;
using UnityEngine;

namespace BeeGood.Managers
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private ScoreManagerView scoreManagerView;
        public static ScoreManager Instance;
        public int Stage { get; private set; }
        public int PlayerScore { get; private set; }
        public int BotScore { get; private set; }
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            Stage = 1;
            PlayerScore = 0;
            BotScore = 0;
            Instance = this;
        }

        public void IncreaseStage(bool isPlayerWin)
        {
            Stage++;

            if (isPlayerWin)
            {
                PlayerScore++;
            }
            else
            {
                BotScore++;
            }

            scoreManagerView.SetScore(PlayerScore, BotScore);
        }
    }
}