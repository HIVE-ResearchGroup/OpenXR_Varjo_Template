using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    // Axel Bauer, Varjo Dev Team
// 2022
    public enum DeviceList
    {
        None,
        OpenXR,
        Varjo
    };

    public class DeviceManager : MonoBehaviour
    {
        public static DeviceManager Instance { get; private set;}

        [Header("Make sure the right device is set in XR-PluginManagement first!")]
        [HideInInspector]
        public static DeviceList staticUsedDevice;  // this public var should appear as a drop down

        [SerializeField]
        private DeviceList usedDevice;

        // Start is called before the first frame update
        void Start()
        {
            if (usedDevice == DeviceList.None)
            {
                Debug.LogError("DeviceManager: Please state the type of device your're developing with!");
            } else
            {
                staticUsedDevice = usedDevice;
                Debug.LogWarning("DeviceManager: You set " + staticUsedDevice + " as your VR-headset!");
                Debug.LogWarning("DeviceManager: Check if this setting of deviceManager matches your actual VR-headset and the setting in Project-Preferences -> XR-PluginManager");
            }
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
            }
        }
    }
}