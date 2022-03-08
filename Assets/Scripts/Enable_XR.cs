using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.XR.Management;


public class Enable_XR : MonoBehaviour
{
    private DeviceList usedDevice;


    //Varjo devices - maybe reuse them for ZED?
    public Camera xrCamera;
    private HDAdditionalCameraData HDCameraData;
    private bool originalOpaqueValue;



    // Start is called before the first frame update
    void Start()
    {
        usedDevice = this.GetComponent<DeviceManager>().usedDevice;

        if (usedDevice == DeviceList.Varjo)
        {
            HDCameraData = xrCamera.GetComponent<HDAdditionalCameraData>();

            //Start into XR Mode
            Varjo.XR.VarjoMixedReality.StartRender();
            Varjo.XR.VarjoRendering.SetOpaque(false);

            // TODO: There are some examples not in need of the following line
            if (HDCameraData)
                HDCameraData.clearColorMode = HDAdditionalCameraData.ClearColorMode.Color;

            //here comes additional code
            //e.g. some depth testing or environment reflection
        } 

        else if (usedDevice == DeviceList.OpenXR_ZED)
        {
            //XR-Code for Vive / ZED Mini
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
