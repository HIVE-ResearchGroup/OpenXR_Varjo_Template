using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeviceList // your custom enumeration
{
    Vive,
    Varjo
};

public class DeviceManager : MonoBehaviour
{
    [Header("Make sure the right device is set in XR-PluginManagement first!")]
    public DeviceList usedDevice;  // this public var should appear as a drop down

    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning("Notice: You set " + usedDevice + " as your VR-headset!");
        Debug.Log("Check if this setting of deviceManager matches your actual VR-headset and the setting in Project-Preferences -> XR-PluginManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
