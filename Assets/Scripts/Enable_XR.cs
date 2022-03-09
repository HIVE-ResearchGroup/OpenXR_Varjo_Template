using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.XR.Management;


public class Enable_XR : MonoBehaviour
{
    public class CubemapEvent : UnityEvent { }


    private DeviceList usedDevice;


    //Varjo devices - maybe reuse them for ZED?
    public Camera xrCamera;
    private HDAdditionalCameraData HDCameraData;
    private bool originalOpaqueValue;
    private bool metadataStreamEnabled = false;
    private Varjo.XR.VarjoCameraMetadataStream.VarjoCameraMetadataFrame metadataFrame;
    private HDRISky volumeSky = null;
    public Cubemap defaultSky = null;
    private Exposure volumeExposure = null;
    private VSTWhiteBalance volumeVSTWhiteBalance = null;
    private Varjo.XR.VarjoCameraSubsystem cameraSubsystem;
    public bool environmentReflections = false;
    public int reflectionRefreshRate = 30;
    private Varjo.XR.VarjoEnvironmentCubemapStream.VarjoEnvironmentCubemapFrame cubemapFrame;
    private bool defaultSkyActive = false;
    public CubemapEvent onCubemapUpdate = new CubemapEvent();
    private bool cubemapEventListenerSet = false;
    public VolumeProfile m_skyboxProfile = null;








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

            // TODO: There are some examples not in need of the following line - check, if there's another way around
            if (HDCameraData)
                HDCameraData.clearColorMode = HDAdditionalCameraData.ClearColorMode.Color;


            //Enable visible hands
            Varjo.XR.VarjoMixedReality.EnableDepthEstimation();


            //Enable hand interaction compability
            //xrCamera.gameObject.AddComponent<Leap.Unity.LeapXRServiceProvider>();
            //xrCamera.gameObject.GetComponent<Leap.Unity.LeapXRServiceProvider>().editTimePose = Leap.TestHandFactory.TestHandPose.HeadMountedB;


            //Enable RealTime Reflections
            if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
            {
                var loader = XRGeneralSettings.Instance.Manager.activeLoader as Varjo.XR.VarjoLoader;
                cameraSubsystem = loader.cameraSubsystem as Varjo.XR.VarjoCameraSubsystem;
            }

            if (cameraSubsystem != null)
            {
                cameraSubsystem.Start();
            }

            originalOpaqueValue = Varjo.XR.VarjoRendering.GetOpaque();
            Varjo.XR.VarjoRendering.SetOpaque(false);
            cubemapEventListenerSet = onCubemapUpdate.GetPersistentEventCount() > 0;
            HDCameraData = xrCamera.GetComponent<HDAdditionalCameraData>();
            //defaultSky; TODO add this here!
            //m_skyboxProfile TODO add this one here!



            /*if (!m_skyboxProfile.TryGet(out volumeSky))
            {
                volumeSky = m_skyboxProfile.Add<HDRISky>(true);
            }

            if (!m_skyboxProfile.TryGet(out volumeExposure))
            {
                volumeExposure = m_skyboxProfile.Add<Exposure>(true);
            }

            if (!m_skyboxProfile.TryGet(out volumeVSTWhiteBalance))
            {
                volumeVSTWhiteBalance = m_skyboxProfile.Add<VSTWhiteBalance>(true);
            }*/



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

        if (usedDevice == DeviceList.Varjo)
        {
            //UpdateEnvironmentReflections();
        }
    }

    void UpdateEnvironmentReflections()
    {
        if (Varjo.XR.VarjoMixedReality.environmentCubemapStream.IsSupported())
        {
            environmentReflections = Varjo.XR.VarjoMixedReality.environmentCubemapStream.Start();
        }

        if (!cameraSubsystem.IsMetadataStreamEnabled)
        {
            cameraSubsystem.EnableMetadataStream();
        }
        metadataStreamEnabled = cameraSubsystem.IsMetadataStreamEnabled;



        if (metadataStreamEnabled)
        {
            if (Varjo.XR.VarjoMixedReality.environmentCubemapStream.hasNewFrame && cameraSubsystem.MetadataStream.hasNewFrame)
            {
                cubemapFrame = Varjo.XR.VarjoMixedReality.environmentCubemapStream.GetFrame();

                metadataFrame = cameraSubsystem.MetadataStream.GetFrame();
                float exposureValue = (float)metadataFrame.metadata.ev + Mathf.Log((float)metadataFrame.metadata.cameraCalibrationConstant, 2f);
                volumeExposure.fixedExposure.Override(exposureValue);

                volumeSky.hdriSky.Override(cubemapFrame.cubemap);
                volumeSky.updateMode.Override(EnvironmentUpdateMode.Realtime);
                volumeSky.updatePeriod.Override(1f / (float)reflectionRefreshRate);
                defaultSkyActive = false;

                volumeVSTWhiteBalance.intensity.Override(1f);

                // Set white balance normalization values
                Shader.SetGlobalColor("_CamWBGains", metadataFrame.metadata.wbNormalizationData.wbGains);
                Shader.SetGlobalMatrix("_CamInvCCM", metadataFrame.metadata.wbNormalizationData.invCCM);
                Shader.SetGlobalMatrix("_CamCCM", metadataFrame.metadata.wbNormalizationData.ccm);

                if (cubemapEventListenerSet)
                {
                    onCubemapUpdate.Invoke();
                }
            }
        }
        else if (!defaultSkyActive)
        {
            volumeSky.hdriSky.Override(defaultSky);
            volumeSky.updateMode.Override(EnvironmentUpdateMode.OnChanged);
            volumeExposure.fixedExposure.Override(6.5f);
            volumeVSTWhiteBalance.intensity.Override(0f);
            defaultSkyActive = true;
        }
    }
}
