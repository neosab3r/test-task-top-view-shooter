using UnityEngine;

namespace BeeGood.Managers
{
    public class GameManager : MonoBehaviour
    {
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
            
        }
    }
}