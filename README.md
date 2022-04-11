# OpenXR_Varjo_Template

## Introduction
This project focuses on combining two workflows (OpenXR and Varjo SDK) into one project and making it easy to switch between those two headset just as by ticking two checkboxes.


## Dependencies
- Varjo SDK
    - In order to use the template, the Varjo SDK has to be imported into your project. Make sure to not delete/move the "Samples" folder when porting/developing in/to your active directory. The template is quite flexible as it will always get the most up to date version of the controllers of the SDK. Consider that when updating the Varjo SDK.
- HDRP Pipeline
- Unity 2021.2.12



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

#### There is an tracking issue with the Varjo system currently - further information will be added into this documentation.
