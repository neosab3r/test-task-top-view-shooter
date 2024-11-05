using TMPro;
using UnityEngine;

namespace BeeGood.UI
{
    public class ScoreManagerView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private void Awake()
        {
            scoreText.text = $"0:0";
        }

        public void SetScore(int playerScore, int botScore)
        {
            scoreText.text = $"{botScore}:{playerScore}";
        }
    }
}