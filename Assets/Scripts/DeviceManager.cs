using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Axel Bauer, Varjo Dev Team
// 2022
public enum DeviceList
{
    None,
    OpenXR_ZED,
    Varjo
};

public class DeviceManager : MonoBehaviour
{
    [Header("Make sure the right device is set in XR-PluginManagement first!")]
    public DeviceList usedDevice;  // this public var should appear as a drop down

    // Start is called before the first frame update
    void Start()
    {
        if (usedDevice == DeviceList.None)
        {
            Debug.LogError("DeviceManager: Please state the type of device your're developing with!");
        } else
        {
            Debug.LogWarning("DeviceManager: You set " + usedDevice + " as your VR-headset!");
            Debug.LogWarning("DeviceManager: Check if this setting of deviceManager matches your actual VR-headset and the setting in Project-Preferences -> XR-PluginManager");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
