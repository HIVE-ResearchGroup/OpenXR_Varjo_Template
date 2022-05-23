using System;
using System.Collections.Generic;
using Core.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.XR.Management;
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
        public Camera xrCamera;
        
        public List<AbstractDeviceFeature> devices;

#if USING_HDRP
        private HDAdditionalCameraData _hdCameraData;
#endif
#if !USING_HDRP
        private Camera _hdCameraData;
#endif


        [Header("Leap Variables")] public bool enableLeapFunctionality = false;


        

        // Start is called before the first frame update
        void Start()
        {
#if USING_HDRP
            _hdCameraData = xrCamera.GetComponent<HDAdditionalCameraData>();
#endif

#if !USING_HDRP
            _hdCameraData = xrCamera.GetComponent<Camera>();
#endif

            // Load and set Mode Variables
            SetModeVariables();


            switch (DeviceManager.staticUsedDevice)
            {
                // Set StartUp function
                case DeviceList.Varjo:
                    //VarjoStartup();
                    break;
                case DeviceList.OpenXR:
                    //OpenXRStartup();
                    break;
                case DeviceList.None:
                default:
                    Debug.LogWarning("EnableXR: No device specified - 'no-startup' invoked");
                    break;
            }


            //Enable hand interaction compability
            if (!enableLeapFunctionality)
            {
                xrCamera.gameObject.GetComponent<Leap.Unity.LeapXRServiceProvider>().enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //get the device (which should be set in startup) and call update
            
        }

        public void SetModeVariables()
        { 
            //call mode variables of the current device
        }

        
    }
}