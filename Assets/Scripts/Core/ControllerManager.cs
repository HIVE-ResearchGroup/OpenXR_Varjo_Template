using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// Axel Bauer
// 2022


namespace Core
{
    public class ControllerManager : MonoBehaviour
    {
        public GameObject leftController;
        public GameObject rightController;
        
        [SerializeField]
        private bool setVisible = true;
        
        [Header("Overrides visibilityHandler:")]
        [SerializeField]
        private bool setInvisibleInAR = true;

        private bool _controllersVisible = true; //initial true because all mesh renderers are also set true by default
        private ActionBasedController leftControllerScript;
        private ActionBasedController rightControllerScript;

        // Start is called before the first frame update
        void Start()
        {

            if (!leftController)
            {
                Debug.LogError("ControllerManager: Please assign the leftController object!");
            } 

            if (!rightController)
            {
                Debug.LogError("ControllerManager: Please assign the rightController object!");
            }

            leftControllerScript = leftController.GetComponent<ActionBasedController>();
            rightControllerScript = rightController.GetComponent<ActionBasedController>();

            //setControllerOffset();
        }

        // Update is called once per frame
        void Update()
        {
            bool tempVisibility = checkVisibilty();

            // Check if there is a visibility state change
            if (_controllersVisible != tempVisibility)
            {
                if (leftController.activeSelf)
                {
                    setVisibility(tempVisibility, leftControllerScript.model.GetChild(0));//first child in order to get the object and not the parent
                }

                if (rightController.activeSelf)
                {
                    setVisibility(tempVisibility, rightControllerScript.model.GetChild(0));//first child in order to get the object and not the parent
                }

                _controllersVisible = tempVisibility;
            }
        }

        // ----------------------------------------------------------------------------------- CHANGE VISIBILTY OF CONTROLLERS

        public void setVisibility(bool state) //a public function that can be used externally to modify the visibilty
        {
            setVisible = state;
        }

        private void setVisibility(bool state, Transform outpoint) //performs the visibiltiy action
        {
            // set visibility to state
            if (outpoint.gameObject.GetComponent<MeshRenderer>())
            {
                outpoint.gameObject.GetComponent<MeshRenderer>().shadowCastingMode = !state ? 
                    UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly : 
                    UnityEngine.Rendering.ShadowCastingMode.On;
            }

            // check if there are children who need to be set as well - consider storing this information in startup to reduce transition times
            if (outpoint.childCount > 0)
            {
                for (int i = 0; i < outpoint.childCount; i++)
                {
                    setVisibility(state, outpoint.GetChild(i));
                }
            }
        }

        // ----------------------------------------------------------------------------------- COMMON FUNCTION
        private bool checkVisibilty()
        {
            if (setInvisibleInAR && XRSceneManager.Instance.arVRToggle.selectedMode == XRmode.AR)
            {
                return false;
            }

            // check user input
            return setVisible;
        }

        // ----------------------------------------------------------------------------------- DEACTIVATE / ACTIVATE CONTROLLERS

    
        private void setDevice(InputDevice ip, GameObject controller)
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
            _controllersVisible = !_controllersVisible;// trigger a check for visibility
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
                setDevice(device, leftController);
                Debug.Log("Left Hand --- " + device.name + " connected!");
            }

            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
            {
                setDevice(device, rightController);
                Debug.Log("Right Hand --- " + device.name + " connected!");
            }
        }

        private void DeviceDisconnected(InputDevice device)
        {
            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Left))
            {
                setDevice(device, leftController);
                Debug.Log("Left Hand --- " + device.name + " disconnected!");
            }

            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
            {
                setDevice(device, rightController);
                Debug.Log("Right Hand --- " + device.name + " disconnected!");
            }
        }

        /*private void setControllerOffset()//this needs not to be used anymore
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
}*/
    }
}
