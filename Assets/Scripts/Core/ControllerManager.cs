using System.Collections.Generic;
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

        private List<MeshRenderer> leftControllerMeshes;
        private List<MeshRenderer> rightControllerMeshes;


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

            leftControllerScript = leftController.GetComponent<ActionBasedController>();
            rightControllerScript = rightController.GetComponent<ActionBasedController>();

            leftControllerMeshes = new List<MeshRenderer>();
            rightControllerMeshes = new List<MeshRenderer>();

            //setControllerOffset();
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
                    ApplyVisibility(tempVisibility, leftControllerMeshes);//first child in order to get the object and not the parent
                }

                if (rightController.activeSelf)
                {
                    ApplyVisibility(tempVisibility, rightControllerMeshes);//first child in order to get the object and not the parent
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
            foreach (MeshRenderer mr in list)
            {
                mr.shadowCastingMode = !state ? 
                    UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly : 
                    UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }


        private void FetchControllerMeshes(Transform outpoint, List<MeshRenderer> meshes)
        {
            // Check if the current parent object has a meshrenderer - if so, add it
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
            if (setInvisibleInAR && XRSceneManager.Instance.arVRToggle.selectedMode == XRmode.AR)
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
                FetchControllerMeshes(leftControllerScript.model.GetChild(0), leftControllerMeshes);//first child in order to get the object and not the parent
                Debug.Log("Left Hand --- " + device.name + " connected!");
            }

            if (device.characteristics.HasFlag(InputDeviceCharacteristics.Right))
            {
                SetDevice(device, rightController);
                FetchControllerMeshes(rightControllerScript.model.GetChild(0), rightControllerMeshes);//first child in order to get the object and not the parent
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
