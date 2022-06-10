using UnityEngine;

namespace Core
{
    /**
     * Singleton for the Interaction Manager
     */
    public class XRIM : MonoBehaviour
    {

        public static XRIM Instance { get; private set; }
        
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
