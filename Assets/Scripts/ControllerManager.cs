using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.XR;


using UnityEngine;


public enum ControllerVisible
{
   True,
   False
};

public class ControllerManager : MonoBehaviour
{
    public GameObject leftController;
    public GameObject rightController;
    public ControllerVisible visiblityHandler;
    [Header("Overrides visibilityHandler:")]
    public bool setInvisibleInAR = true;

    private bool controllersVisible = true; //initial true because all mesh renderers are also set true by default
    private XRmode xrMode;
    private DeviceList usedDevice;

    XRNode XRNodeLeftHand = XRNode.LeftHand;
    XRNode XRNodeRightHand = XRNode.RightHand;

    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;


    // Start is called before the first frame update
    void Start()
    {
        usedDevice = this.gameObject.GetComponent<DeviceManager>().usedDevice;

        if (!leftController)
        {
            Debug.LogError("ControllerManager: Please assign the leftController object!");
        } 

        if (!rightController)
        {
            Debug.LogError("ControllerManager: Please assign the rightController object!");
        }


        // Covering controller offset
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
            leftController.GetComponent<BoxCollider>().center = new Vector3(0.0f, -0.01f, -0.01f);
            rightController.GetComponent<BoxCollider>().center = new Vector3(0.0f, -0.01f, -0.01f);

            //updated SphereCollider -> grabbing
            leftController.GetComponent<SphereCollider>().center = new Vector3(0.0f, 0f, 0.08f);
            rightController.GetComponent<SphereCollider>().center = new Vector3(0.0f, 0f, 0.08f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        xrMode = this.gameObject.GetComponent<AR_VR_Toggle>().selectedMode;
        

        // Check if wether a controller needs to be completely deactivated
        GetDevice(XRNodeLeftHand, leftController);
        GetDevice(XRNodeRightHand, rightController);

        bool tempVisibility = checkVisibilty();

        // Check if there is a visibility state change
        if (controllersVisible != tempVisibility)
        {
            setVisibility(tempVisibility);
            controllersVisible = tempVisibility;
        }
    }

    bool checkVisibilty()
    {
        if (setInvisibleInAR && xrMode == XRmode.AR)
        {
            return false;
        }

        //check user input
        if (visiblityHandler == ControllerVisible.True)
        {
            return true;
        }
        else if (visiblityHandler == ControllerVisible.False)
        {
           return false;
        }

        return false;
    }

    void setVisibility(bool state)
    {
        Transform outpointLeft = leftController.transform.GetChild(0);
        Transform outpointRight = rightController.transform.GetChild(0);

        if (!state)
        {
            // NOTE: This code only works with the current XRRig setup! You might need to adapt it, when you change the XRRig prefab!
            for (int i = 0; i < outpointLeft.childCount; i++)
            {
                Transform x = outpointLeft.GetChild(i);
                if (x.gameObject.GetComponent<MeshRenderer>())
                {
                    x.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }

                Transform y = outpointRight.GetChild(i);
                if (y.gameObject.GetComponent<MeshRenderer>())
                {
                    y.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            }
        } else
        {
            for (int i = 0; i < outpointLeft.childCount; i++)
            {
                Transform x = outpointLeft.GetChild(i);
                if (x.gameObject.GetComponent<MeshRenderer>())
                {
                    x.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }

                Transform y = outpointRight.GetChild(i);
                if (y.gameObject.GetComponent<MeshRenderer>())
                {
                    y.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                }
            }
        }
    }

    void GetDevice(XRNode Hand, GameObject controller)
    {
        InputDevices.GetDevicesAtXRNode(Hand, devices);
        device = devices.FirstOrDefault();
        if (!device.isValid)
        {
            controller.SetActive(false);
        }
        else
        {
            controller.SetActive(true);
        }
    }
}
