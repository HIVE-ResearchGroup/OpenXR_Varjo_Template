# OpenXR_Varjo_Template

## Introduction
This project focuses on combining two workflows (OpenXR and Varjo SDK) into one project and making it easy to switch between those two headset just as by ticking two checkboxes.


## Dependencies
- Varjo SDK
    - In order to use the template, the Varjo SDK has to be imported into your project. This template does not need the Sample folder to be imported anymore and thus can be operated without. However, you are free to use the Samples in order to get in touch with other Varjo features such as Markers or "Windows". If you search for a version of the template that might be more flexible, have a look at the version release of [0.1.0.](https://github.com/HIVE-ResearchGroup/OpenXR_Varjo_Template/releases/tag/v0.2.0).
- HDRP Pipeline
- Unity 2021.2.12*

### What about other render pipelines?
<bold>Release 0.1.0 and 0.2.0: This led to an error in the building which is why only HDRP is supported!</bold>

It is possible to use the scripts and prefabs inside this template to use some of the Varjo features also in your SRP/URP project. Note, that you will only have support for AR mode and Depth testing. Please be aware that you will have to set your own settings for lights, shaders, materials, reflections. If you still want to continue, please export the prefabs and the scripts (scenes are also possible) from the template as an Asset and import it in your preferred environment. Notice that with the current release, you will have to import the Varjo samples in your project as well in order to get the latest version of the controller. Doing that, you might have to delete some scripts inside the Sample folder as they not support other pipelines. Furthermore, you might will have to create a new ShadowCatcher material for the ground as the one used in the template is created for HDRP.

Please don't forget to create the used layers (ARObjects, VRObjects) and the used tags (Fracture, Pickable), as well to set the Opaque value inside the Varjo SDK to false.

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

- It's possible that SteamVR (regarding input and tracking using the Varjo headset, even outside the project) might have some issues after switching headsets. In this case, either restart the headset using the Varjo Base Program or/and restart SteamVR. If it won't appear, try starting SteamVR from inside the Varjo-Base-Program. If that won't do, restart your computer.
- Interestingly, there is an offset between the input tracking of the Varjo SDK and OpenXR. This offset (as you might've read above) is managed insidethe AR_VR_Toggle-Script. If ever the offset is getting changed in the future, these values should be adjusted inside this script.
- Sky and Fog Volume: I tweaked the it slightly in order to match the requirements of the Varjo SDK. (Now, the focus-rectangle shouldn't be visible). If the scene doesn't match your lighting conditions, you may turn the exposure down inside the Sky and Fog Volume.
- If there's a slight flickering around your focus displays on your Varjo headset, try adding/changing the override regarding "motion blur" in your project. If this still doesn't help out and don't have the time to further testing, you may deactivate "Forveated Rendering" (Project Settings -> XR Plug-in Management -> Varjo). This option controls if the focus of the display should be restricted on a small rectangle (checked - increases performance) or should cover the whole display (unchecked). You may uncheck it for a smoother experience but maybe get not the best performance.
- In some cases, the output to an OpenXR headset might not work on some machines. In this cases, try switching to MultiPass rendering (Project Settings -> XR Plug-in Management -> OpenXR). Note that in this mode, the CPU and power consumption will be higher.
- By default, the Eye Offset is set to 1 because otherwise, there would be a noticeable object drift of your objects inside your AR scene. If it happens that you notice this behaviour, make sure to check this setting first.
- <bold>There are interesting differences between the Varjo package and the OpenXR package regarding input! Please have a look at the other [README](./Assets/README.md#differences-between-varjo-and-openxr-package) in this regard!</bold>
#### There is an tracking issue with the Varjo system currently - further information will be added into this documentation.
