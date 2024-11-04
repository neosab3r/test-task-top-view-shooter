using UnityEngine;

namespace BeeGood.Managers
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager Instance;
        public int Stage { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }

            Stage = 0;
            Instance = this;
        }

        public void IncreaseStage()
        {
            Stage++;
        }
    }
}