using UnityEngine;

namespace Core
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }
    
        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

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
