using UnityEngine;

namespace Core
{ 
    // Axel Bauer
    // 2022
    public enum DeviceList
    {
        None,
        OpenXR,
        Varjo
    };

    /**
     * Script that sets the used device
     */
    public class DeviceManager : MonoBehaviour
    {
        [Header("Make sure the right device is set in XR-PluginManagement first!")]
        public DeviceList usedDevice;

        // Start is called before the first frame update
        private void Start()
        {
            if (usedDevice == DeviceList.None)
            {
                Debug.LogError("DeviceManager: Please state the type of environment you're developing with!");
            } else
            {
                Debug.LogWarning("DeviceManager: You set " + usedDevice + " as your environment!");
                Debug.LogWarning("DeviceManager: Check if this setting of deviceManager matches your actual " +
                                 "VR-headset and the setting in Project-Preferences -> XR-PluginManager");
            }
        }
    }
}