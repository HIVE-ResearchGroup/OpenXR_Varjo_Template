using UnityEngine;

namespace Core
{
    /**
     * Singleton for player
     */
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
    
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
