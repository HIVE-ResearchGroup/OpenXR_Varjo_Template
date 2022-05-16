# OpenXR_Varjo_Template

## Introduction
This project focuses on combining two workflows (OpenXR and Varjo SDK) into one project and making it easy to switch between those two headset just as by ticking two checkboxes.
It features:
- direct- and rayinteractors (for interacting with far away objects as well as grabbing objects),
- teleportation
- Ultraleap Handtracking support
- automatic controller disabling/activating
- ARobjects/VRobjects toggling (by using layers)
- the new [Unity XRI](https://docs.unity3d.com/Manual/xr_input.html) (as well as demo scripts),
- Varjo features (AR mode, environment reflections)


## Dependencies
- Varjo SDK
    - In order to use the template, the Varjo SDK has to be imported into your project. This template does not need the Sample folder to be imported anymore and thus can be operated without. However, you are free to use the Samples in order to get in touch with other Varjo features such as Markers or "Windows". If you search for a version of the template that might be more flexible, have a look at the version release of [0.2.0.](https://github.com/HIVE-ResearchGroup/OpenXR_Varjo_Template/releases/tag/v0.2.0).
- HDRP Pipeline
- Unity 2021.3.2*

### What about other render pipelines?
It is possible to use the scripts and prefabs inside this template to use some of the Varjo features also in your SRP/URP project. Note, that you will only have support for AR mode and Depth testing. Please be aware that you will have to set your own settings for lights, shaders, materials, reflections. If you still want to continue, please export the prefabs and the scripts (scenes are also possible) from the template as an Asset and import it in your preferred environment. (Or use the Asset coming with every release.) Furthermore, you might will have to create a new ShadowCatcher material for the ground as the one used in the template is created for HDRP and might need to change the colour of the Camera Background flag inside the camera object to black.

Please don't forget to create the used layers (ARObjects [layer 6], VRObjects [layer 7]) and the used tags (Fracture, Pickable), as well to set the Opaque value inside the Varjo SDK to false.

## Structure
There is one main file managing this transition: The deviceManager object, containing three scripts:
- Device Manager
- Enable_XR
- AR_VR_Toggle
- Controller Manager Script
- Load ARVR Objects to Mode Script


## Useage
For more information about the useage, please have a look at the [README](./Assets/README.md) inside the Asset-folder.


## Noticeable mentions
- Sky and Fog Volume: I tweaked the it slightly in order to match the requirements of the Varjo SDK. (Now, the focus-rectangle shouldn't be visible). If the scene doesn't match your lighting conditions, you may turn the exposure down inside the Sky and Fog Volume.
- If there's a slight flickering around your focus displays on your Varjo headset, try adding/changing the override regarding "motion blur" in your project. If this still doesn't help out and don't have the time to further testing, you may deactivate "Forveated Rendering" (Project Settings -> XR Plug-in Management -> Varjo). This option controls if the focus of the display should be restricted on a small rectangle (checked - increases performance) or should cover the whole display (unchecked). You may uncheck it for a smoother experience but maybe get not the best performance.
- By default, the Eye Offset is set to 1 because otherwise, there would be a noticeable object drift of your objects inside your AR scene. If it happens that you notice this behaviour, make sure to check this setting first.
- Since this project uses the HDRP pipeline, you probably should get used in using the physical based lighting setup (have a look at this [tutorial](https://www.youtube.com/watch?v=yqCHiZrgKzs)). Right now, the Exposure (inside the Post Process Volume) is set (statically because of VR quality reasons) to 6.5 which resembles a sunset brightness and the Sun object light being set to 1300 lux which however should be set to 111,000 lux to resemble a real sun. This was done to resemble an indoor lighting situation.
- When setting the shadow resolution of the directional light to "Ultra" the transparent ground material will be visible.


### Known Issues
- It's possible that SteamVR (regarding input and tracking using the Varjo headset, even outside the project) might have some issues after switching headsets. In this case, either restart the headset using the Varjo Base Program or/and restart SteamVR. If it won't appear, try starting SteamVR from inside the Varjo-Base-Program. If that won't do, restart your computer.
- In some cases, the output to an OpenXR headset might not work on some machines. In this cases, try switching to MultiPass rendering (Project Settings -> XR Plug-in Management -> OpenXR). Note that in this mode, the CPU and power consumption will be higher.
- Interestingly, there is an offset between the input tracking of the Varjo SDK and OpenXR. This offset (as you might've read above) is managed inside the AR_VR_Toggle-Script. If ever the offset is getting changed in the future, these values should be adjusted inside this script.
- <bold>There are interesting differences between the Varjo package and the OpenXR package regarding input! Please have a look at the other [README](./Assets/README.md#differences-between-varjo-and-openxr-package) in this regard!</bold>
- In OpenXR mode, there is a bug which prevents the controller to disconnect properly which results into the controllers not being able to disable correctly (ControllerManager.cs)
- AR_VR_Toggle: Since there is a delay in switching the depth testing on and off, the implementation is made that depth testing is enabled on startup when checked. This leads to an error in the Varjo SDK that depth testing is also enabled inside VR. This should resolve when switching to AR and back again to VR. It is thus mostly recommended to start your scene in AR when using depth testing or deactivate it when using VR. Of course you might also use this behaviour in VR to interact with your real hands but not recommended as it could be fixed in a future release.

#### There is an tracking issue with the Varjo system currently - further information will be added into this documentation.

- Refer to the [GitHub](https://github.com/HIVE-ResearchGroup/OpenXR_Varjo_Template/issues) page for further information about current issues.