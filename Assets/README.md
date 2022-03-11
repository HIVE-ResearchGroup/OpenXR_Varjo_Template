# Useage

## DeviceManager Script

The deviceManager stores the selected headset and checks, if there is any controller offset that needs to be covered. This script could be used for external scripts to access the current headset.

### How to switch between headsets
To make sure the transition goes well, make sure that there is no headset running (plugged off the computer/electricity) and SteamVR and/or Varjo Base are closed. Then, go to Project Settings under XR Plug-in Management and check the needed SDK for your device. Even if that might not change a lot: Make sure to first uncheck the already ticked checkbox first.

#### Do not launch the program with both SDKs activated!

After, go to the deviceManager script and set the "Used Device" setting matching your headset and the already set SDK. Now you should be way to go! If needed, start SteamVR and/or Varjo Base and plug in your headset. If everything is running (and you didn't forget to re-connect your controllers), feel free to start the template!

## Enable_XR Script
This script is the most complicated script in the project. It loads the specific scripts and functionality for the headsets depending on the selected environment-type (see AR_VR_Toggle).

Notice: In order to use the "make ground invisible"-functionality, please make sure to name your ground like in the template.

## AR_VR_Toggle Script
Sets the environment-type and could be used by external scripts to change the environment during runtime. (Can also be used to change the environment for testing purposes during a live-preview.)


## How to adapt the controller
In order to extend the functionality of the controller, add a script to the controller in need, which checks the input like the following example (old input system - event based):
```
    Controller controller;

    void Start()
    {
        controller = GetComponent<Controller>();

    }

    // Update is called once per frame
    void Update()
    {
        if (controller.Primary2DAxisClick)
        {
            Debug.Log("Controller: Primary2DAxisClick");
        }
    }
```

This project supports both types of input-systems (new, Unity-style and old, event based), which is why a complete transition to the new input-system should be possible. (This, however, needs to be tested with the XR-Rig first).

A complete listing of the controller-controls can be found inside the script "Controller.cs".

# How to interact with the environment
- In order to teleport yourself, press onto the "primaryButton", which is located above the touchpad on a HTC Vive controller. Notice that this is only possible on a Varjo headset. Unfortunately, SteamVR has a bug that with the Input Actions - see the issue tracker on the GitHub page.
- In order to grab objects, use the Trigger-button (back) and release it in order to let the object fall.
- In order to shoot some cubes, press and let go on the touchpad. (Please notice that using the Varjo headset, you will also move/click with your mouse like that.)

# How to increase performance
In this template, there are three main factors 
- Forveated Rendering
- Single Pass / Multipass
- Reflection probe

