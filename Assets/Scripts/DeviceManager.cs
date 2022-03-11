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

    [Header("Covering Controller offset:")]
    public GameObject leftController;
    public GameObject rightController;

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


        if (usedDevice == DeviceList.Varjo)
        {
 
            //new position
            leftController.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
            rightController.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);

            //updated boxCollider -> physics
            leftController.GetComponent<BoxCollider>().center = new Vector3(0.0f, -0.01f, -0.09f);
            rightController.GetComponent<BoxCollider>().center = new Vector3(0.0f, -0.01f, -0.09f);

            //updated SphereCollider -> grabbing
            leftController.GetComponent<SphereCollider>().center = new Vector3(0f, 0f, 0f);
            rightController.GetComponent<SphereCollider>().center = new Vector3(0f, 0f, 0f);
        }
        else if (usedDevice == DeviceList.OpenXR_ZED)
        {
            //new position
            leftController.transform.GetChild(0).localPosition = new Vector3(0, 0, 0.08f);
            rightController.transform.GetChild(0).localPosition = new Vector3(0, 0, 0.08f);

            //updated boxCollider -> physics
            leftController.GetComponent<BoxCollider>().center = new Vector3(0.0f, -0.01f,- 0.01f);
            rightController.GetComponent<BoxCollider>().center = new Vector3(0.0f, -0.01f, -0.01f);

            //updated SphereCollider -> grabbing
            leftController.GetComponent<SphereCollider>().center = new Vector3(0.0f, 0f, 0.08f);
            rightController.GetComponent<SphereCollider>().center = new Vector3(0.0f, 0f, 0.08f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
