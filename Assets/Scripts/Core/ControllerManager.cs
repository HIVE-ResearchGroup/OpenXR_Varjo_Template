using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.XR;


using UnityEngine;

// Axel Bauer, Varjo Dev Team
// 2022
public enum ControllerVisible
{
   True,
   False
};

public class ControllerManager : MonoBehaviour
{
    public GameObject leftController;
    public GameObject rightController;
    public ControllerVisible currentlyVisible;
    [Header("Overrides visibilityHandler:")]
    public bool setInvisibleInAR = true;

    private bool m_controllersVisible = true; //initial true because all mesh renderers are also set true by default
    private XRmode m_xrMode;
    private DeviceList m_usedDevice;
    private DeviceManager m_dm;
    private AR_VR_Toggle m_avt;

    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;


    // Start is called before the first frame update
    void Start()
    {
        m_dm = this.gameObject.GetComponent<DeviceManager>();
        m_avt = this.gameObject.GetComponent<AR_VR_Toggle>();

        if (!m_dm)
        {
            Debug.LogError("ControllerManager: DeviceManager not found on this object!");
        } else
        {
            m_usedDevice = m_dm.usedDevice;
        }


        if (!m_avt)
        {
            Debug.LogError("ControllerManager: AR_VR_Toggle not found on this object!");
        }


        if (!leftController)
        {
            Debug.LogError("ControllerManager: Please assign the leftController object!");
        } 

        if (!rightController)
        {
            Debug.LogError("ControllerManager: Please assign the rightController object!");
        }


        //setControllerOffset();
    }

    private void OnEnable()
    {
        InputDevices.deviceDisconnected += DeviceDisconnected;
        InputDevices.deviceConnected += DeviceConnected;
    }

    private void OnDisable()
    {
        InputDevices.deviceDisconnected -= DeviceDisconnected;
        InputDevices.deviceConnected -= DeviceConnected;
    }

    private void DeviceConnected(InputDevice device)
    {
        if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
        {
            SetDevice(device, leftController);
            Debug.Log("Left Hand --- " + device.name + " connected!");
        }

        if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
        {
            SetDevice(device, rightController);
            Debug.Log("Right Hand --- " + device.name + " connected!");
        }
    }

    private void DeviceDisconnected(InputDevice device)
    {
        if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
        {
            SetDevice(device, leftController);
            Debug.Log("Left Hand --- " + device.name + " disconnected!");
        }

        if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
        {
            SetDevice(device, rightController);
            Debug.Log("Right Hand --- " + device.name + " disconnected!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_xrMode = m_avt.selectedMode;

        bool tempVisibility = checkVisibilty();

        // Check if there is a visibility state change
        if (m_controllersVisible != tempVisibility)
        {
            if (leftController.activeSelf)
            {
                setVisibility(tempVisibility, leftController);
            }

            if (rightController.activeSelf)
            {
                setVisibility(tempVisibility, rightController);
            }

            m_controllersVisible = tempVisibility;
        }
    }

    void setControllerOffset()
    {
        // Covering controller offset
        if (m_usedDevice == DeviceList.Varjo)
        {

            // new position
            leftController.transform.GetChild(1).GetChild(0).localPosition = new Vector3(0, 0, 0);
            rightController.transform.GetChild(1).GetChild(0).localPosition = new Vector3(0, 0, 0);

            // updated boxCollider -> physics
            leftController.GetComponent<BoxCollider>().center = new Vector3(0.0f, -0.01f, -0.09f);
            rightController.GetComponent<BoxCollider>().center = new Vector3(0.0f, -0.01f, -0.09f);

            // updated SphereCollider -> grabbing
            leftController.GetComponent<SphereCollider>().center = new Vector3(0f, 0f, 0f);
            rightController.GetComponent<SphereCollider>().center = new Vector3(0f, 0f, 0f);
        }
        else if (m_usedDevice == DeviceList.OpenXR)// probably for all OpenXR devices
        {
            // new position
            leftController.transform.GetChild(1).GetChild(0).localPosition = new Vector3(0, 0, 0.08f);
            rightController.transform.GetChild(1).GetChild(0).localPosition = new Vector3(0, 0, 0.08f);

            // updated boxCollider -> physics
            leftController.GetComponent<BoxCollider>().center = new Vector3(0.0f, -0.01f, -0.01f);
            rightController.GetComponent<BoxCollider>().center = new Vector3(0.0f, -0.01f, -0.01f);

            // updated SphereCollider -> grabbing
            leftController.GetComponent<SphereCollider>().center = new Vector3(0.0f, 0f, 0.08f);
            rightController.GetComponent<SphereCollider>().center = new Vector3(0.0f, 0f, 0.08f);
        }
    }

    bool checkVisibilty()
    {
        if (setInvisibleInAR && m_xrMode == XRmode.AR)
        {
            return false;
        }

        // check user input
        if (currentlyVisible == ControllerVisible.True)
        {
            return true;
        }
        else if (currentlyVisible == ControllerVisible.False)
        {
           return false;
        }

        return false;
    }

    void setVisibility(bool state, GameObject controller)
    {

        Transform outpoint = controller.transform.GetChild(1).GetChild(0).GetChild(0);

        if (!state)
        {
            // NOTE: This code only works with the current XRRig setup! You might need to adapt it, when you change the XRRig prefab!
            for (int i = 0; i < outpoint.childCount; i++)
            {
                Transform x = outpoint.GetChild(i);
                if (x.gameObject.GetComponent<MeshRenderer>())
                {
                    x.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            }
        } else
        {
            for (int i = 0; i < outpoint.childCount; i++)
            {
                Transform x = outpoint.GetChild(i);
                if (x.gameObject.GetComponent<MeshRenderer>())
                {
                    x.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
            }
        }
    }

    void SetDevice(InputDevice ip, GameObject controller)
    {
        if (!ip.isValid)
        {
            Debug.Log("Set " + controller.name + " (" + ip.name + ") to false");
            controller.SetActive(false);
        }
        else
        {
            Debug.Log("Set " + controller.name + " (" + ip.name + ") to true");
            controller.SetActive(true);
        }
    }
}
