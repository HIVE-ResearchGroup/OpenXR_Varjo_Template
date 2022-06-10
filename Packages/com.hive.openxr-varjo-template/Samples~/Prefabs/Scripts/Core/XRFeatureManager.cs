using System;
using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;
#if USING_URP
using UnityEngine.Rendering.Universal;
#endif


// Axel Bauer, Varjo Dev Team
// 2022

namespace Core
{
    /**
     * Manages all the device features and stores it inside the List<AbstractDeviceFeature>
     */
    public class XRFeatureManager : MonoBehaviour
    {
        //Varjo devices - maybe reuse them for ZED?
        [SerializeField]
        private Camera xrCamera;
        
        public List<AbstractDeviceFeature> devices;
        
        [HideInInspector]
        public AbstractDeviceFeature loadedDevice;

        [Header("Leap Variables")] public bool enableLeapFunctionality = false;

        private bool _noDeviceLoaded = true;
        
        // Start is called before the first frame update
        private void Start()
        {
            LoadDeviceFeatures();

            if (loadedDevice != null) //check if there is a loaded device, else there will be a null reference
            {
                // Load and set Mode Variables
                SetModeVariables();
                loadedDevice.XRStart();
                _noDeviceLoaded = false;
            }

            //Enable hand interaction compability
            if (!enableLeapFunctionality)
            {
                xrCamera.gameObject.GetComponent<Leap.Unity.LeapXRServiceProvider>().enabled = false;
            }
        }

        // Update is called once pe r frame
        private void Update()
        {
            if (!_noDeviceLoaded)
                loadedDevice.XRUpdate();   
        }

        public void SetModeVariables()
        {             
            if (!_noDeviceLoaded)
                loadedDevice.SetModeVariables();
        }

        private void LoadDeviceFeatures()
        {
            if (XRSceneManager.Instance.isDeviceManagerActive && devices.Count == Enum.GetNames(typeof(DeviceList)).Length-1) // -1, because 'None' is also inside the DeviceList
            {
                switch (XRSceneManager.Instance.deviceManager.usedDevice)
                {
                    // Set StartUp function
                    case DeviceList.Varjo:
                        loadedDevice = devices[0];// TODO hardcoded number, should be changed in the future
                        break;
                    case DeviceList.OpenXR:
                        loadedDevice = devices[1];
                        break;
                    case DeviceList.None:
                    default:
                        Debug.LogWarning("XRFeatureManager: No device specified - 'no-startup' invoked");
                        break;
                }
            }
            else
            {
                Debug.LogError("XRFeatureManager: Number of scripts don't match the number of devices - " +
                               "probably you might need to add a device into the DeviceList inside GO 'DeviceManager' " +
                               "or you forgot to add a script in GO 'XRFeatureManager'!");
            }

        }


    }
}