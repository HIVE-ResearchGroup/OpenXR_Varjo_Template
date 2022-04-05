# OpenXR_Varjo_Template

## Introduction
This project focuses on combining two workflows (OpenXR and Varjo SDK) into one project and making it easy to switch between those two headset just as by ticking two checkboxes.


## Structure
There is one main file managing this transition: The deviceManager object, containing three scripts:
- Device Manager
- Enable_XR
- AR_VR_Toggle


## Useage
For more information about the useage, please have a look at the [README](./Assets/README.md) inside the Asset-folder.


## Noticeable mentions

- It's possible that SteamVR (regarding input and tracking using the Varjo headset, even outside the project) might have some issues after switching headsets. In this case, either restart the headset using the Varjo Base Program or/and restart SteamVR. If it won't appear, try starting SteamVR from inside the Varjo-Base-Program. If that won't do, restart your computer.
- Interestingly, there is an offset between the input tracking of the Varjo SDK and OpenXR. This offset (as you might've read above) is managed insidethe AR_VR_Toggle-Script. If ever the offset is getting changed in the future, these values should be adjusted inside this script.
- Sky and Fog Volume: I tweaked the it slightly in order to match the requirements of the Varjo SDK. (Now, the focus-rectangle shouldn't be visible). If the scene doesn't match your lighting conditions, you may turn the exposure down inside the Sky and Fog Volume.
- When watching closely, there is a slight flicker when jump-focusing your eyes on different object with the Varjo headset. It seems that the headset is responsible for this behaviour, as the focus-display either is not balanced enough with the rest of the display or (more likely) Unity. Unfortunately, I haven't found a solution to this problem yet but one Varjo setting (Project Settings -> XR Plug-in Management -> Varjo), in order to activate/deactivate "Forveated Rendering". This option controls if the focus of the display should be restricted on a small rectangle (checked - increases performance) or should cover the whole display (unchecked). You may uncheck it for a smoother experience but maybe not the best performance.

#### There is an tracking issue with the Varjo system currently - further information will be added into this documentation.
