using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// Axel Bauer, Varjo Dev Team
// 2022


namespace Core
{
    /**
     * Manages the controller display behaviour.
     * This script is not performant and should be replaced someday with the "Beta" version since it calls GetComponent
     * during runtime. However, OpenXR loads the model prefabs during runtime which is why loading the Meshes beforehand
     * is not possible. Most likely, you could simply solve this by not turning the MeshRenderers into "Shadows Only" and
     * setting the whole GameObject to "not active".
     */
    public class ControllerManager : MonoBehaviour
    {
        public GameObject leftController;
        public GameObject rightController;
        public bool setVisible = true;

        [Header("Overrides visibilityHandler:")]
        public bool setInvisibleInAR = true;

        private bool _controllersVisible = true; //initial true because all mesh renderers are also set true by default

        private ActionBasedController _leftControllerScript;
        private ActionBasedController _rightControllerScript;

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

            _leftControllerScript = leftController.GetComponent<ActionBasedController>();
            _rightControllerScript = rightController.GetComponent<ActionBasedController>();

            //setControllerOffset();
        }

        // Update is called once per frame
        void Update()
        {
            bool tempVisibility = CheckVisibility();

            // Check if there is a visibility state change
            if (_controllersVisible != tempVisibility)
            {
                if (leftController.activeSelf)
                {
                    ApplyVisibility(tempVisibility,
                        _leftControllerScript.model
                            .GetChild(0)); //first child in order to get the object and not the parent
                }

                if (rightController.activeSelf)
                {
                    ApplyVisibility(tempVisibility,
                        _rightControllerScript.model
                            .GetChild(0)); //first child in order to get the object and not the parent
                }

                _controllersVisible = tempVisibility;
            }
        }

        // ----------------------------------------------------------------------------------- CHANGE VISIBILTY OF CONTROLLERS

        public void SetVisibility(bool state) //a public function that can be used externally to modify the visibilty
        {
            setVisible = state;
        }

        private void ApplyVisibility(bool state, Transform outpoint) //performs the visibiltiy action
        {
            // set visibility to state
            if (outpoint.gameObject.GetComponent<MeshRenderer>())
            {
                if (!state)
                {
                    outpoint.gameObject.GetComponent<MeshRenderer>().shadowCastingMode =
                        UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
                else
                {
                    outpoint.gameObject.GetComponent<MeshRenderer>().shadowCastingMode =
                        UnityEngine.Rendering.ShadowCastingMode.On;
                }

            }

            // check if there are children who need to be set as well - consider storing this information in startup to reduce transition times
            if (outpoint.childCount > 0)
            {
                for (int i = 0; i < outpoint.childCount; i++)
                {
                    ApplyVisibility(state, outpoint.GetChild(i));
                }
            }
        }

        // ----------------------------------------------------------------------------------- COMMON FUNCTION
        private bool CheckVisibility()
        {
            if (setInvisibleInAR && XRSceneManager.Instance.isARVRToggleActive && XRSceneManager.Instance.arVRToggle.selectedMode == XRmode.AR)
            {
                return false;
            }

            // check user input
            return setVisible;
        }

        // ----------------------------------------------------------------------------------- DEACTIVATE / ACTIVATE CONTROLLERS

        private void SetDevice(InputDevice ip, GameObject controller)
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

            _controllersVisible = !_controllersVisible; // trigger a check for visibility
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
    }
}