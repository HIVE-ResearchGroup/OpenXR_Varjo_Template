# Useage

## DeviceManager Script

The deviceManager stores the selected headset and checks, if there is any controller offset that needs to be covered. This script could be used for external scripts to access the current headset.

### How to switch between headsets
To make sure the transition goes well, make sure that there is no headset running (plugged off the computer/electricity) and SteamVR and/or Varjo Base are closed. Then, go to Project Settings under XR Plug-in Management and check the needed SDK for your device. Even if that might not change a lot: Make sure to first uncheck the already ticked checkbox first.

- <bold>Vive</bold>: First, go to SteamVR -> Settings -> Developer -> Set SteamVR as OpenXR runtime. Inside Unity, set the runtime to OpenXR. Notice that everytime you'll start another runtime (e.g Varjo) on your system, this setting might change on its own.
- <bold>Varjo</bold>: Make sure the runtime is set correctly and inside Unity, only the Varjo runtime is selected.

#### Do not launch the program with both SDKs activated!

After, go to the deviceManager script and set the "Used Device" setting matching your headset and the already set SDK. Now you should be way to go! If needed, start SteamVR and/or Varjo Base and plug in your headset. If everything is running (and you didn't forget to re-connect your controllers), feel free to start the template!

## Enable_XR Script
This script is the most complicated script in the project. It loads the specific scripts and functionality for the headsets depending on the selected environment-type (see AR_VR_Toggle). There is a setting called Eye offset which is set to 1 by default (see noticable mentions). Notice that there are cases where in VR, 0 might be a better choice.

## AR_VR_Toggle Script
Sets the environment-type and could be used by external scripts to change the environment during runtime. (Can also be used to change the environment for testing purposes during a live-preview.)
It's even possible to switch between AR and VR during runtime usind the "T" key.

## Controller Manager Script
This script makes sure that the controllers are setup correctly. It covers the controller offset between Varjo tracking and OpenXR tracking (Vive) as well as disabling the controllers when switching to AR mode.

## Load ARVR Objects to Mode Script
This script manages the display of objects related to the used display mode. You'll also find the option to turn the ground invisible (and act as a shadow catcher) with this script.


## The new Input System (How to adapt the controller)
This project uses the new Unity Input System in order to maintain functionality in future Unity releases. Please make sure your code complies to the following system.

There are four possible ways to handle input in the new input system but two are especially helpful to consider in this template. Those are the ones this short tutorial will present. For the others please follow the links.

- <bold>[Player Input](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.3/manual/QuickStartGuide.html#getting-input-indirectly-through-an-input-action)</bold>
- <bold>[State Input](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.3/manual/QuickStartGuide.html#getting-input-directly-from-an-input-device)</bold> (similar to the "old" Input system)

- <bold>Action Asset Input</bold>
    - This template uses one Action Asset in order to manage the movement or interaction profile of the HMD. You may find it in the XR Interaction Toolkit folder inside Samples folder.
    - In order to use an Action Asset, the Input Action Manager component has to be added to a game object. (In the template's case, this is the XR Origin (Rig))
    - Additionally to the Action Asset, a script has to define the actions inside the Action Asset. Have a look at [this](https://docs.unity3d.com/Packages/com.unity.inputsystem@0.9/manual/ActionAssets.html#using-input-action-assets) for an example. Note, that you can automatically create such a script inside the Action Asset object. Finally, you may access the input system doing so:
    ```
    private DefinedActionScript m_Controls;

    public void Awake() {
        m_Controls = new DefinedActionScript();
        m_Controls.actionMap.action.started += ctx => {
            // have a look on the Input Action version
        }
    }

    void OnEnable() {
        m_Controls.Enable();
    }

    void OnDisable() {
        m_Controls.Disable();
    }

    ```

- <bold>Action Input</bold>
    - Compared to the State Input, this variant makes is more easy to add additional controller mappings.
    - Quite more simple than with an Action Asset, you may use this variant in which you define the mapping locally at the game object where you assign your script. Yet, it also has a major drawback when it comes to redundancy as it can make it more frustrating than with just one Action Asset in case of many objects depending on the same actions. In order to access the actions in your script, do so by:
    ```
    public InputAction myAction;

    void Awake() {
        myAction.started += ctx => {
            // your code on "started" - will only execute once
            // have a look at the binding in the menu where you can define slow tap/hold etc. to find out/set the duration
        }

        myAction.performed += ctx => {
            // during the successfull press
            // you may check if the type of press by doing so:

            if (ctx.interaction is SlowTapInteraction) {
                // your code if a certain time limit has to be reached
            }
        }

        myAction.canceled += ctx => {
            // when button is released
        }
    }

    void OnEnable() {
        myAction.Enable();
    }

    void OnDisable() {
        myAction.Disable();
    }

    ```

In order to read a value (for example to access the trigger state):
```
action.ReadValue<T>();
```
In order to get an example useage file (of the Action Input variant), go to the "XR Interaction Manager" object inside the Hierachy and have a look at the "Shoot.cs"
For a more in-depth option, have a look at this [guide](https://gamedevbeginner.com/input-in-unity-made-easy-complete-guide-to-the-new-system/#input_system_explained).

### General mentions
In order to convert your project to the new input system, it is necessary to both import the com.unity.xr.interaction.toolkit and the "XR Plugin Management" plugin. You then need to create a new object: XR -> "XR Origin (Action based)". Make sure to use the Action based variant as this would also use the new Input system. If you use the XRI samples, don't forget to add them on the top right corner of the Inspector when clicking on e.g. XRI Default Continous Turn and to add the "Input Action Manager" component script to your XR Rig.

# How to interact with the environment
The new Input System needs an "executor" to fully function. In this templates case, this is the trigger. You can use this executor to apply the teleportation, select/grab etc.
- In order to teleport yourself, press onto the "primaryButton", which is located above the touchpad on a HTC Vive controller or at the "A" button on a Valve Index controller. 
- In order to grab objects, use the Trigger-button (back) and release it in order to let the object fall.
- In order to shoot some cubes with the Vive controller, press and let go on the touchpad or, on the Valve Index controller, press and let go of B. (Please notice that using the Varjo headset, you will also move/click with your mouse like that.)
- In order to show an (UI/object) pointer to interact with from a certain distance, gently rest your finger on the touchpad.

Note that movement-controls on the controller were removed because of the lack of useabilty. If you want to import those controls back again, go to the package manager, XR Interaction Package and reimport the package. Please keep in mind that you will have to set the teleportation controls on both controllers back to "trigger" inside the Interaction asset.

# How to increase performance
In this template, there are three main factors 
- Forveated Rendering: Have a look on "Noticable mentions".
- Single Pass / Multipass: Using Single Pass, parts of the image for both eyes are reused for the other eye.
- Reflection probe: In order to make use of the environment reflections, the reflection probe updates the reflection "realtime".

# How to make objects visible to either VR or AR
Please make use of the specific layers.
