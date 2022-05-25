using System;
using Core;
using Core.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.XR.Management;

namespace DeviceFeatures
{
    public class VarjoFeature : AbstractDeviceFeature
    {
        [Serializable]
        public class CubemapEvent : UnityEvent
        {
        }
        
        public Camera xrCamera;
        
        [Header("Varjo Variables")] [Range(0f, 1.0f)]
        public float vrEyeOffset = 0.0f;
        
        [Header("Depthtesting is only modified on startup")]
        public bool enableDepthTesting;

        
#if USING_HDRP
        [SerializeField]
        private bool enableEnvironmentReflections;
#endif
        
        private bool _originalOpaqueValue;
        private bool _storedVideoSeeThrough;
        private bool _videoSeeThrough;
        
        
#if USING_HDRP
        private HDAdditionalCameraData _hdCameraData;
#endif
#if !USING_HDRP
        private Camera _hdCameraData;
#endif

#if USING_HDRP
        private bool _storedEnvironmentReflections;
        private bool _metadataStreamEnabled;
        private Varjo.XR.VarjoCameraMetadataStream.VarjoCameraMetadataFrame _metadataFrame;
        private HDRISky _volumeSky;
        private Exposure _volumeExposure;
        private VSTWhiteBalance _volumeVSTWhiteBalance;
        private Varjo.XR.VarjoCameraSubsystem _cameraSubsystem;
        private Varjo.XR.VarjoEnvironmentCubemapStream.VarjoEnvironmentCubemapFrame _cubemapFrame;
        private bool _defaultSkyActive;
        private bool _cubemapEventListenerSet;

        [Header("Reflection Variables")] public Cubemap defaultSky;
        public VolumeProfile skyboxProfile;
        public int reflectionRefreshRate = 30;
        public CubemapEvent onCubemapUpdate = new CubemapEvent();
#endif
        
        public override void XRStart()
        {

            if (xrCamera == null)
            {
                Debug.LogError("VarjoFeature: There is no main camera set!");
                return;
            }
            
#if USING_HDRP
            _hdCameraData = xrCamera.GetComponent<HDAdditionalCameraData>();
#endif

#if !USING_HDRP
            _hdCameraData = xrCamera.GetComponent<Camera>();
#endif
            
            //Enabling AR mode
            if (_videoSeeThrough)
            {
                //videoSeeThrough = true;
                Varjo.XR.VarjoMixedReality.StartRender();
                Varjo.XR.VarjoRendering.SetOpaque(false);

                // TODO: There are some examples not in need of the following line - check, if there's another way around
#if USING_HDRP
                if (_hdCameraData)
                    _hdCameraData.clearColorMode = HDAdditionalCameraData.ClearColorMode.Color;
#endif
#if !USING_HDRP
                if (_hdCameraData)
                    _hdCameraData.clearFlags = CameraClearFlags.SolidColor;
#endif
            }

            //Enable visible hands
            if (enableDepthTesting)
            {
                Varjo.XR.VarjoMixedReality.EnableDepthEstimation();
                Varjo.XR.VarjoRendering.SetSubmitDepth(true);
                Varjo.XR.VarjoRendering.SetDepthSorting(true);
            }

            // Set Eye Offset
            Varjo.XR.VarjoMixedReality.SetVRViewOffset(vrEyeOffset);
            
            
#if USING_HDRP
            //Enable RealTime Reflections
            _cubemapEventListenerSet = onCubemapUpdate.GetPersistentEventCount() > 0;
            if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
            {
                var loader = XRGeneralSettings.Instance.Manager.activeLoader as Varjo.XR.VarjoLoader;

                if (loader == null)
                {
                    Debug.LogError("EnableXR: Couldn't load VarjoSubSystem. Make sure the Plugin is installed and activated inside Project Preferences!");
                }
                else
                {
                    _cameraSubsystem = loader.cameraSubsystem as Varjo.XR.VarjoCameraSubsystem;
                }
            }

            _cameraSubsystem?.Start();

            _originalOpaqueValue = Varjo.XR.VarjoRendering.GetOpaque();
            Varjo.XR.VarjoRendering.SetOpaque(false);
            _cubemapEventListenerSet = onCubemapUpdate.GetPersistentEventCount() > 0;

            //TODO check how this will react in SRP or URP
            if (!skyboxProfile.TryGet(out _volumeSky))
            {
                _volumeSky = skyboxProfile.Add<HDRISky>(true);
            }

            if (!skyboxProfile.TryGet(out _volumeExposure))
            {
                _volumeExposure = skyboxProfile.Add<Exposure>(true);
            }

            if (!skyboxProfile.TryGet(out _volumeVSTWhiteBalance))
            {
                _volumeVSTWhiteBalance = skyboxProfile.Add<VSTWhiteBalance>(true);
            }
#endif
            
        }

        public override void XRUpdate()
        {
            UpdateVideoSeeThrough();
#if USING_HDRP
            UpdateEnvironmentReflections();
#endif
        }
        
#if USING_HDRP
        public void SetEnvironmentReflections(bool state)
        {
            enableEnvironmentReflections = state;
        }
#endif

        public override void SetModeVariables()
        {
            if (XRSceneManager.Instance.isARVRToggleActive && XRSceneManager.Instance.arVRToggle.selectedMode == XRmode.AR) //if Mode set to AR
            {
                _videoSeeThrough = true;
#if USING_HDRP
                enableEnvironmentReflections = true;
#endif
            }
            else // if Mode set to VR
            {
                _videoSeeThrough = false;
#if USING_HDRP
                enableEnvironmentReflections = false;
#endif
            }
        }

        private void UpdateVideoSeeThrough()
        {
            if (_videoSeeThrough != _storedVideoSeeThrough)
            {
                if (_videoSeeThrough)
                {
                    Varjo.XR.VarjoMixedReality.StartRender();
                    Varjo.XR.VarjoRendering.SetOpaque(false);

#if USING_HDRP
                    if (_hdCameraData)
                        _hdCameraData.clearColorMode = HDAdditionalCameraData.ClearColorMode.Color;
#endif

#if !USING_HDRP
                    if (_hdCameraData)
                        _hdCameraData.clearFlags = CameraClearFlags.SolidColor;
#endif
                }
                else
                {
                    Varjo.XR.VarjoMixedReality.StopRender();
                    Varjo.XR.VarjoRendering.SetOpaque(true);

#if USING_HDRP
                    if (_hdCameraData)
                        _hdCameraData.clearColorMode = HDAdditionalCameraData.ClearColorMode.Sky;
#endif

#if !USING_HDRP
                    if (_hdCameraData)
                        _hdCameraData.clearFlags = CameraClearFlags.Skybox;
#endif
                }

                _storedVideoSeeThrough = _videoSeeThrough;
            }
        }

#if USING_HDRP
        private void UpdateEnvironmentReflections()
        {
            if (_storedEnvironmentReflections != enableEnvironmentReflections)
            {
                if (enableEnvironmentReflections)
                {
                    if (Varjo.XR.VarjoMixedReality.environmentCubemapStream.IsSupported())
                    {
                        _storedEnvironmentReflections = Varjo.XR.VarjoMixedReality.environmentCubemapStream.Start();
                    }

                    if (!_cameraSubsystem.IsMetadataStreamEnabled)
                    {
                        _cameraSubsystem.EnableMetadataStream();
                    }

                    _metadataStreamEnabled = _cameraSubsystem.IsMetadataStreamEnabled;
                }
                else
                {
                    Varjo.XR.VarjoMixedReality.environmentCubemapStream.Stop();
                    _cameraSubsystem.DisableMetadataStream();
                }

                _storedEnvironmentReflections = enableEnvironmentReflections;
            }

            if (enableEnvironmentReflections && _metadataStreamEnabled)
            {
                if (Varjo.XR.VarjoMixedReality.environmentCubemapStream.hasNewFrame &&
                    _cameraSubsystem.MetadataStream.hasNewFrame)
                {
                    _cubemapFrame = Varjo.XR.VarjoMixedReality.environmentCubemapStream.GetFrame();

                    _metadataFrame = _cameraSubsystem.MetadataStream.GetFrame();
                    float exposureValue = (float)_metadataFrame.metadata.ev +
                                          Mathf.Log((float)_metadataFrame.metadata.cameraCalibrationConstant, 2f);
                    _volumeExposure.fixedExposure.Override(exposureValue);

                    _volumeSky.hdriSky.Override(_cubemapFrame.cubemap);
                    _volumeSky.updateMode.Override(EnvironmentUpdateMode.Realtime);
                    _volumeSky.updatePeriod.Override(1f / (float)reflectionRefreshRate);
                    _defaultSkyActive = false;

                    _volumeVSTWhiteBalance.intensity.Override(1f);

                    // Set white balance normalization values
                    Shader.SetGlobalColor("_CamWBGains", _metadataFrame.metadata.wbNormalizationData.wbGains);
                    Shader.SetGlobalMatrix("_CamInvCCM", _metadataFrame.metadata.wbNormalizationData.invCCM);
                    Shader.SetGlobalMatrix("_CamCCM", _metadataFrame.metadata.wbNormalizationData.ccm);

                    if (_cubemapEventListenerSet)
                    {
                        onCubemapUpdate.Invoke();
                    }
                }
            }
            else if (!_defaultSkyActive)
            {
                _volumeSky.hdriSky.Override(defaultSky);
                _volumeSky.updateMode.Override(EnvironmentUpdateMode.OnChanged);
                _volumeExposure.fixedExposure.Override(6.5f);
                _volumeVSTWhiteBalance.intensity.Override(0f);
                _defaultSkyActive = true;
            }
        }
#endif
        private void OnDisable()
        {
            enableDepthTesting = false;
#if USING_HDRP
            enableEnvironmentReflections = false;
#endif
            _videoSeeThrough = false;
            Varjo.XR.VarjoRendering.SetOpaque(_originalOpaqueValue);
        }
    }
    
}