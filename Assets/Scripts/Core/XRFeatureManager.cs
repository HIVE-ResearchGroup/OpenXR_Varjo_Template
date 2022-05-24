using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;
#if USING_URP
using UnityEngine.Rendering.Universal;
#endif


// Axel Bauer, Varjo Dev Team
// 2022

namespace Core
{
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
        void Start()
        {
            LoadDeviceFeatures();

            if (loadedDevice != null)
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
        void Update()
        {
            if (!_noDeviceLoaded)
                loadedDevice.XRUpdate();   
        }

        public void SetModeVariables()
        { 
            loadedDevice.SetModeVariables();
        }

        private void LoadDeviceFeatures()
        {
            switch (XRSceneManager.Instance.deviceManager.usedDevice)
            {
                // Set StartUp function
                case DeviceList.Varjo:
                    loadedDevice = devices[0];//hardcoded number, should be changed in the future
                    break;
                case DeviceList.OpenXR:
                    loadedDevice = devices[1];
                    break;
                case DeviceList.None:
                default:
                    Debug.LogWarning("EnableXR: No device specified - 'no-startup' invoked");
                    break;
            }
        }


    }
}