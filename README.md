# OpenXR_Varjo_Template

## Introduction
This project focuses on combining two workflows (OpenXR and Varjo SDK) into one project and making it easy to switch between those two headset just as by ticking two checkboxes.
It features:
- OpenXR Rig
  - Direct- and rayinteractors (for interacting with far away objects as well as grabbing objects),
  - Teleportation
  - Ultraleap Handtracking support
  - Automatic controller disabling/activating
- ARobjects/VRobjects toggling (by using layers)
- The new [Unity XRI](https://docs.unity3d.com/Manual/xr_input.html) (as well as demo scripts),
- Varjo features (AR mode, environment reflections)


## Dependencies
- Varjo SDK
    - In order to use the template, the Varjo SDK has to be imported into your project. This template does not need the Sample folder to be imported anymore and thus can be operated without. However, you are free to use the Samples in order to get in touch with other Varjo features such as Markers or "Windows". If you search for a version of the template that might be more flexible, have a look at the version release of [0.2.0.](https://github.com/HIVE-ResearchGroup/OpenXR_Varjo_Template/releases/tag/v0.2.0).
- HDRP Pipeline
- Unity 2021.3.2*

### What about other render pipelines?
It is possible to use the scripts and prefabs inside this template to use some of the Varjo features also in your SRP/URP project. Note, that you will only have support for AR mode and Depth testing. Please be aware that you will have to set your own settings for lights, shaders, materials, reflections. You now need to switch the materials with the ones in the "Materials"/"BiRP" folder. Those materials are made with the Built-In renderpipeline in mind (BiRP), yet you can easily [upgrade](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@14.0/manual/features/rp-converter.html) them to URP.
Note that there is no replacement for the ShadowCatcher material (the one being transparent on the floor and just displaying the shadows) as this is an HDRP only feature.
You also need to change the colour of the Camera Background flag inside the camera object to Color -> black.

Please don't forget to create the used layers (ARObjects [layer 6], VRObjects [layer 7]) and the used tags (Fracture, Pickable), as well to set the Opaque value inside the Varjo SDK to false.
Also, note that using the Volume objects are also restricted to HDRP which is why you should delete them when switching.

Important notice: If you choose to "Update all materials to HDRP", the materials inside the BiRP folders will also update and not be able to be used with the BiRP!

## Structure
There is one main file managing this transition: The deviceManager object, containing following scripts:
- Device Manager 
- AR_VR_Toggle
- Load ARVR Objects to Mode Script
- XR Feature Manager
  - Varjo Feature
  - OpenXR Feature
- Controller Manager Script


## Useage
For more information about the usage, please have a look at the [README](USEAGE.md).

## Use the Unity project
If you intend not to start from scratch, download this project and use it as a starting point. The project already uses the package and the project/input settings are already set - no further configuration needed.

## How to install the package
If you want to use the features in an already existing project or want to custom-build your scenes, you might want to go with just installing the package.

1. Make sure to install the Varjo OpenXR (tutorial) and the Ultraleap (tutorial) plugins first. 
2. Import the samples of the Ultraleap plugin. (You don't need to import the Varjo Samples but can, if you want/need to, since this package uses some of the materials/models of the package.) If you use the HDRP pipeline, you might need to update the materials of the sample folders (only those materials, see "What about other pipelines").
3. Don't forget to import the Sample folder of the Interaction Toolkit Plugin (which was installed automatically - if it doesn't show up in the Package Manager, install it by typing "com.unity.xr.interaction.toolkit") as well. With this object, you need to add all the different components to the assets and then change (with both the XRI LeftHand Interaction and XRI RightHand Interaction) "Select" (to triggerPressed), "Select Value" (to trigger).
4. After this, go this package and download the samples you need.
In order to just use the prefabs, download the "Core Prefabs" samples. If you want to get an example scene, download the "Simple Scene" sample. There are also some Asset models you might want to use.
5. Add the layers (6: VRObjects, 7: ARObjects, 8: Raycasts) and the "Fraction", "Pickable" tag, if it doesn't exist already.
6. Go to either RightHand or LeftHand Ray and to the Raycast object, go to "Interaction Layer Mask", tick "Add layer" and add "Raycasts" as Interaction Layer.

Note: You might want to switch the Input System to "Both" inside Project Settings -> Player -> Others, if you want to use scripts that use the old Input System.


### If you want to start from scratch (with the prefabs and without the scene)
Drag and drop the following prefabs into your scene:
- XR Scene Manager (have a look at the [scripts](SCRIPTSDESCRIPTION.md) to find out more)
- XR Origin (XR Rig with Camera)
- XR Interaction Manager
- Post Processing Volume (optional but good advice if you want to save time fixing the eye gazing with the Varjo)

The other prefabs ("Controller Left" and "Controller Right") are only needed to be added inside the XR Rig -> Left/RightHand Controller -> XRController script -> Model Prefab.

Add the relations to the Prefab:
- Add the LeftHand and RightHand Controller of the XR Origin to the Controller Manager of the XR Scene Manager.
- Assign the "Main Camera" object for the XR Feature Manager and Varjo Feature.
- Add the "Low Poly Hands" Gameobject to the AR_VR_Toggle object (LoadARVRObjectsToMode.cs)
- Add the Ground GameObject to the AR_VR_Toggle object (LoadARVRObjectsToMode.cs)



## Noticeable mentions
- Sky and Fog Volume: I tweaked the it slightly in order to match the requirements of the Varjo SDK. (Now, the focus-rectangle shouldn't be visible). If the scene doesn't match your lighting conditions, you may turn the exposure down inside the Sky and Fog Volume.
- If there's a slight flickering around your focus displays on your Varjo headset, try adding/changing the override regarding "motion blur" in your project. If this still doesn't help out and don't have the time to further testing, you may deactivate "Forveated Rendering" (Project Settings -> XR Plug-in Management -> Varjo). This option controls if the focus of the display should be restricted on a small rectangle (checked - increases performance) or should cover the whole display (unchecked). You may uncheck it for a smoother experience but maybe get not the best performance.
- By default, the Eye Offset is set to 1 because otherwise, there would be a noticeable object drift of your objects inside your AR scene. If it happens that you notice this behaviour, make sure to check this setting first.
- Since this project uses the HDRP pipeline, you probably should get used in using the physical based lighting setup (have a look at this [tutorial](https://www.youtube.com/watch?v=yqCHiZrgKzs)). Right now, the Exposure (inside the Post Process Volume) is set (statically because of VR quality reasons) to 6.5 which resembles a sunset brightness and the Sun object light being set to 1300 lux which however should be set to 111,000 lux to resemble a real sun. This was done to resemble an indoor lighting situation.
- When setting the shadow resolution of the directional light to "Ultra" the transparent ground material will be visible.


### Known Issues
- It's possible that SteamVR (regarding input and tracking using the Varjo headset, even outside the project) might have some issues after switching headsets. In this case, either restart the headset using the Varjo Base Program or/and restart SteamVR. If it won't appear, try starting SteamVR from inside the Varjo-Base-Program. If that won't do, restart your computer.
- In some cases, the output to an OpenXR headset might not work on some machines. In this cases, try switching to MultiPass rendering (Project Settings -> XR Plug-in Management -> OpenXR). Note that in this mode, the CPU and power consumption will be higher.
- <bold>There are interesting differences between the Varjo package and the OpenXR package regarding input! Please have a look at the other [README](USEAGE.md#differences-between-varjo-and-openxr-package) in this regard!</bold>
- In OpenXR mode, there is a bug which prevents the controller to disconnect properly which results into the controllers not being able to disable correctly (ControllerManager.cs)
- AR_VR_Toggle: Since there is a delay in switching the depth testing on and off, the implementation is made that depth testing is enabled on startup when checked. This leads to an error in the Varjo SDK that depth testing is also enabled inside VR. This should resolve when switching to AR and back again to VR. It is thus mostly recommended to start your scene in AR when using depth testing or deactivate it when using VR. Of course you might also use this behaviour in VR to interact with your real hands but not recommended as it could be fixed in a future release.

#### There is an tracking issue with the Varjo system currently - further information will be added into this documentation.

- Refer to the [GitHub](https://github.com/HIVE-ResearchGroup/OpenXR_Varjo_Template/issues) page for further information about current issues.