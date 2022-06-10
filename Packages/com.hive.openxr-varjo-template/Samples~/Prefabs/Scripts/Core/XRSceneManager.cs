using UnityEngine;

namespace Core
{
    /**
     * Singleton that stores a reference to the different scripts.
     */
    public class XRSceneManager : MonoBehaviour
    {
        public static XRSceneManager Instance { get; private set;}
        
        public AR_VR_Toggle arVRToggle { get; private set; }
        public DeviceManager deviceManager { get; private set; }
        public LoadARVRObjectsToMode loadArvrObjectsToMode { get; private set; }
        public XRFeatureManager xrFeatureManager { get; private set; }
        public ControllerManager controllerManager { get; private set; }

        [HideInInspector]
        public bool isARVRToggleActive = true;

        [HideInInspector]
        public bool isDeviceManagerActive = true;

        [HideInInspector]
        public bool isFeatureManagerActive = true;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
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

                arVRToggle = GetComponentInChildren<AR_VR_Toggle>();
                deviceManager = GetComponentInChildren<DeviceManager>();
                loadArvrObjectsToMode = GetComponentInChildren<LoadARVRObjectsToMode>();
                xrFeatureManager = GetComponentInChildren<XRFeatureManager>();
                controllerManager = GetComponentInChildren<ControllerManager>();


                if (arVRToggle == null)
                {
                    isARVRToggleActive = false;
                }

                if (deviceManager == null)
                {
                    isDeviceManagerActive = false;
                }

                if (xrFeatureManager == null)
                {
                    isFeatureManagerActive = false;
                }
            }
        }
    }
}
