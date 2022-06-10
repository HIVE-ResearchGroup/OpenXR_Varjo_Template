# The new Input System (How to adapt the controller)
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

## General mentions
In order to convert your project to the new input system, it is necessary to both import the com.unity.xr.interaction.toolkit and the "XR Plugin Management" plugin. You then need to create a new object: XR -> "XR Origin (Action based)". Make sure to use the Action based variant as this would also use the new Input system. If you use the XRI samples, don't forget to add them on the top right corner of the Inspector when clicking on e.g. XRI Default Continous Turn and to add the "Input Action Manager" component script to your XR Rig.
