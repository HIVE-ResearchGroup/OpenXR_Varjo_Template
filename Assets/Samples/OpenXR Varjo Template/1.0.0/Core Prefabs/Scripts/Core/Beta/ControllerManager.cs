using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// Axel Bauer
// 2022
// This script doesn't work because OpenXR loads the controller prefabs just when needed and thus, there are no meshes to fetch onStartup to improve performance
namespace Core.Beta
{
    /**
     * Manages the controller display behaviour
     */
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
        private ActionBasedController _leftControllerScript;
        private ActionBasedController _rightControllerScript;

        private List<MeshRenderer> _leftControllerMeshes;
        private List<MeshRenderer> _rightControllerMeshes;

        // Start is called before the first frame update
        private void Start()
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

            _leftControllerMeshes = new List<MeshRenderer>();
            _rightControllerMeshes = new List<MeshRenderer>();
        }

        // Update is called once per frame
        private void Update()
        {
            var tempVisibility = CheckVisibility();

            // Check if there is a visibility state change
            if (_controllersVisible != tempVisibility)
            {
                if (leftController.activeSelf)
                {
                    ApplyVisibility(
                        tempVisibility, _leftControllerMeshes);//first child in order to get the object and not the parent
                }

                if (rightController.activeSelf)
                {
                    ApplyVisibility(
                        tempVisibility, _rightControllerMeshes);//first child in order to get the object and not the parent
                }

                _controllersVisible = tempVisibility;
            }
        }

        // ----------------------------------------------------------------------------------- CHANGE VISIBILITY OF CONTROLLERS

        public void SetVisibility(bool state) //a public function that can be used externally to modify the visibility
        {
            setVisible = state;
        }

        private void ApplyVisibility(bool state, List<MeshRenderer> list) //performs the visibility action
        {
            _leftControllerScript.modelParent.gameObject.SetActive(state);//basic disabling and enabling - shadow catcher profile not possibly necessary?
            _rightControllerScript.modelParent.gameObject.SetActive(state);

            foreach (MeshRenderer mr in list)
            {
                mr.shadowCastingMode = !state ? 
                    UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly : 
                    UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }


        private void FetchControllerMeshes(Transform outpoint, List<MeshRenderer> meshes)
        {
            // Check if the current parent object has a mesh-renderer - if so, add it
            if (outpoint.gameObject.GetComponent<MeshRenderer>())
            {
                meshes.Add(outpoint.gameObject.GetComponent<MeshRenderer>());
            }
            
            //check for every child and its children
            if (outpoint.childCount > 0)
            {
                for (int i = 0; i < outpoint.childCount; i++)
                {
                    FetchControllerMeshes(outpoint.GetChild(i), meshes);
                }
            }
        }
        

        // ----------------------------------------------------------------------------------- COMMON FUNCTION
        private bool CheckVisibility() // since the variable 'setVisible' is not
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
                SetDevice(device, leftController);
                FetchControllerMeshes(_leftControllerScript.model.GetChild(0), _leftControllerMeshes);//first child in order to get the object and not the parent
                Debug.Log("Left Hand --- " + device.name + " connected!");
            }

            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
            {
                SetDevice(device, rightController);
                FetchControllerMeshes(_rightControllerScript.model.GetChild(0), _rightControllerMeshes);//first child in order to get the object and not the parent
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
