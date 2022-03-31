using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.XR.Management;


// Axel Bauer, Varjo Dev Team
// 2022

public class Enable_XR : MonoBehaviour
{
    [Serializable]
    public class CubemapEvent : UnityEvent { }


    private DeviceList usedDevice;
    private XRmode xrMode;
    private XRmode selectedXrMode;

    //Varjo devices - maybe reuse them for ZED?
    public Camera xrCamera;
    public bool setGroundTransparent;
    public Material shadowCatcher;

    [Header("Varjo Variables")]
    [Range(0f, 1.0f)]
    public float VREyeOffset = 0.0f;
    public bool enableDepthTesting;
    public bool enableEnvironmentReflections;
    public bool enableLeapFunctionality;

    [Header("SteamVR Variables")]
    public GameObject SteamVRPrefab;

    [Header("ZEDMini Variables")]
    public GameObject ZedMiniPrefab;

    private bool videoSeeThrough;
    private HDAdditionalCameraData HDCameraData;
    private bool originalOpaqueValue;
    private bool metadataStreamEnabled = false;
    private Varjo.XR.VarjoCameraMetadataStream.VarjoCameraMetadataFrame metadataFrame;
    private HDRISky volumeSky = null;
    private Exposure volumeExposure = null;
    private VSTWhiteBalance volumeVSTWhiteBalance = null;
    private Varjo.XR.VarjoCameraSubsystem cameraSubsystem;
    private bool environmentReflections;
    private Varjo.XR.VarjoEnvironmentCubemapStream.VarjoEnvironmentCubemapFrame cubemapFrame;
    private bool defaultSkyActive = false;
    private bool cubemapEventListenerSet = false;

    [Header("Reflection Variables")]
    public Cubemap defaultSky = null;
    public VolumeProfile m_skyboxProfile = null;
    public int reflectionRefreshRate = 30;
    public CubemapEvent onCubemapUpdate = new CubemapEvent();


    //[Header("Vive Variables")]





    // Start is called before the first frame update
    void Start()
    {
        usedDevice = this.GetComponent<DeviceManager>().usedDevice;
        HDCameraData = xrCamera.GetComponent<HDAdditionalCameraData>();
        xrMode = this.GetComponent<AR_VR_Toggle>().selectedMode;
        selectedXrMode = xrMode;


        SetGroundTransparent();


        if (usedDevice == DeviceList.Varjo)
        {
            VarjoStartup();
        } 

        else if (usedDevice == DeviceList.OpenXR_ZED)
        {
            ViveZedStartup();
        }

    }

    // Update is called once per frame
    void Update()
    {
        xrMode = this.GetComponent<AR_VR_Toggle>().selectedMode;

        if (usedDevice == DeviceList.Varjo)
        {
            UpdateVarjoFeatures();
        }
    }

    void SetGroundTransparent()
    {
        //Replacing ground with transparent layer
        if (setGroundTransparent)
        {
            if (shadowCatcher != null)
            {
                GameObject ground = GameObject.Find("Ground");
                if (ground != null)
                {
                    ground.GetComponent<Renderer>().material = shadowCatcher;
                }
                else
                {
                    Debug.LogError("Enable_XR: Couldn't find GameObject with the name ground!");
                }
            }
            else
            {
                Debug.LogError("Enable_XR: Please assign the according material in order to set the ground transparent!");
            }
        }
    }

    void UpdateVarjoFeatures()
    {
        UpdateVideoSeeThrough();
        UpdateEnvironmentReflections();
    }

    void VarjoStartup()
    {

        //Enabling AR mode
        if (xrMode == XRmode.AR)
        {
            videoSeeThrough = true;
            Varjo.XR.VarjoMixedReality.StartRender();
            Varjo.XR.VarjoRendering.SetOpaque(false);

            // TODO: There are some examples not in need of the following line - check, if there's another way around
            if (HDCameraData)
                HDCameraData.clearColorMode = HDAdditionalCameraData.ClearColorMode.Color;

        }

        //Enable visible hands
        if (enableDepthTesting)
        {
            Varjo.XR.VarjoMixedReality.EnableDepthEstimation();
            Varjo.XR.VarjoRendering.SetSubmitDepth(true);
            Varjo.XR.VarjoRendering.SetDepthSorting(true);
        }

        //Enable hand interaction compability
        if (enableLeapFunctionality)
        {
            //xrCamera.gameObject.AddComponent<Leap.Unity.LeapXRServiceProvider>();
            //xrCamera.gameObject.GetComponent<Leap.Unity.LeapXRServiceProvider>().editTimePose = Leap.TestHandFactory.TestHandPose.HeadMountedB;
        }




        //Enable RealTime Reflections
            /*cubemapEventListenerSet = onCubemapUpdate.GetPersistentEventCount() > 0;
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
            */


            if (!m_skyboxProfile.TryGet(out volumeSky))
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
            }

        // Set Eye Offset
        Varjo.XR.VarjoMixedReality.SetVRViewOffset(VREyeOffset);
    }

    void UpdateVideoSeeThrough()
    {
        if (xrMode != selectedXrMode)
        {
            if (xrMode == XRmode.AR && videoSeeThrough)
            {
                Varjo.XR.VarjoMixedReality.StartRender();
                if (HDCameraData)
                    HDCameraData.clearColorMode = HDAdditionalCameraData.ClearColorMode.Color;
            }
            else if (xrMode == XRmode.VR || !videoSeeThrough)
            {
                Varjo.XR.VarjoMixedReality.StopRender();
                if (HDCameraData)
                    HDCameraData.clearColorMode = HDAdditionalCameraData.ClearColorMode.Sky;
            }
            selectedXrMode = xrMode;
        }
 
    }

    void UpdateEnvironmentReflections()
    {
        if (environmentReflections != enableEnvironmentReflections && xrMode == XRmode.AR)
        {
            if (enableEnvironmentReflections)
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
            }
            else
            {
                Varjo.XR.VarjoMixedReality.environmentCubemapStream.Stop();
                cameraSubsystem.DisableMetadataStream();
            }
            environmentReflections = enableEnvironmentReflections;
        }

        if (enableEnvironmentReflections && metadataStreamEnabled && xrMode == XRmode.AR)
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


    void ViveZedStartup()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        if (players.Length > 0)
            for (uint i = 0; i < players.Length; i++)
                Destroy(players[i]);
        
        if (xrMode == XRmode.VR)
        {
            GameObject.Instantiate(SteamVRPrefab);
        } else if (xrMode == XRmode.AR)
        {
            GameObject.Instantiate(ZedMiniPrefab);
        }
    }

    void OnDisable()
    {

        if (usedDevice == DeviceList.Varjo)
        {
            enableDepthTesting = false;
            enableEnvironmentReflections = false;
            videoSeeThrough = false;
            Varjo.XR.VarjoRendering.SetOpaque(originalOpaqueValue);
            UpdateVarjoFeatures();
        }
    }
}
