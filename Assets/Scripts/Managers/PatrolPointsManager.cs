using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BeeGood.Managers
{
    public class PatrolPointsManager : MonoBehaviour
    {
        [SerializeField] private List<Transform> patrolPoints;
        public static PatrolPointsManager Instance;   
        public List<Transform> AllPatrolPoints => patrolPoints;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            
            Instance = this;
        }

        public Transform GetRandomPatrolPoint()
        {
            return patrolPoints[Random.Range(0, patrolPoints.Count)];
        }
    }
}