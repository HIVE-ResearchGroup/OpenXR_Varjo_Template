## Which scripts does this template use to switch between AR/VR?

Have a look at [this guide](SCRIPTSDESCRIPTION.md) where the functionality of every script is explained.

## How to interact with the environment
<details><summary>Click to expand!</summary>

The new Input System needs an "executor" to fully function. In this templates case, this is the trigger. You can use this executor to apply the teleportation, select/grab etc.
- In order to teleport yourself, press onto the "primaryButton", which is located above the touchpad on a HTC Vive controller or at the "B" button on a Valve Index controller. 
- In order to grab objects, use the Trigger-button (back) and release it in order to let the object fall.
- In order to shoot some cubes with the Vive controller, press and let go on the touchpad or, on the Valve Index controller, press and let go of "A". (Please notice that using the Varjo headset, you will also move/click with your mouse like that.)
- In order to show an (UI/object) pointer to interact with from a certain distance, gently rest your finger on the touchpad.

Note that movement-controls on the controller were removed because of the lack of useabilty. If you want to import those controls back again, go to the package manager, XR Interaction Package and reimport the package. Please keep in mind that you will have to set the teleportation controls on both controllers back to "trigger" inside the Interaction asset.

<bold>In order to grab an object, you will have to add the "XR Grab Interactable" component and a "Pickable" tag to preferred object.</bold>

### How it works
As the controller should have both the functionality to teleport, point and grab objects directly, the "XR Origin" has both two objects with the "Controller"-ending, as well as two objects with the "Ray"-ending. The Controller objects implement the direct controls (which is why there is a sphere collider - on trigger) and the controller model prefabs (which also have a collider, in order to physically move objects with your controller in your scene), while the Ray objects implement the raycasting (pointer and teleport). Both are activated simultaneously. Since they both are triggered by the "Select" property inside the XRI, they both work the same. 

In order to show/render the line (either pointer or teleporter), an additional script "On Button Press" is added the both Controller-objects. <bold>Note that the actions stated here must contain both the actions for toggling the pointer and the teleporter as it only specifies that the line has to be rendered.</bold>

Talking about raycasters:
In order for the code to switch between the teleporter line and the pointer line, there is a "Controller Action" script added to the Ray-objects. Here, you can specify which actions should be used in order to change to the teleporter line (and raycast) or the pointer line (and raycast). <bold>Note that you have to make sure that these actions are identically to those stated in the controller objects (OnButtonPress.cs)!</bold>
Notice that these also only manage the line to be rendered. You still have to use the "Select" action stated in the XRI Action Asset to perform the action.

### Differences between Varjo and OpenXR package
Most importantly, there is no drawback in the functionality of both system - just a difference in the way of handling it. 

- As said above, the touchpad is only supported with the Vive controller. This only applies when developing on OpenXR. It works normally when used with the Varjo package.
- In OpenXR, the gripPressed with the Index controller is too sensitive. This is not the case with the Varjo package. Also, the secondaryButton (A on an Index controlelr) is not implemented with the Varjo package, yet when pressed, it behaves like a gripPressed.

That means, you can shoot the box demo normally with both controllers using the touchpad when using the Varjo package and instead, switch between AR/VR using the A-button or the gripButtons on both controllers.

Also note that the Varjo package doesn't support feedback impuls on hover yet.

</details>

## How to use the new Input System (XRI)
Have a look at [this explanation](INPUTSYSTEM.md).

## How to extend the template with a new device
Have a look at this [short tutorial](CREATEFEATURE.md).

## How to increase performance
<details><summary>Click to expand!</summary>

In this template, there are some main factors 
- Forveated Rendering: Have a look on [Noticable mentions](README.md#noticeable-mentions).
- Single Pass / Multipass: Using Single Pass, parts of the image for both eyes are reused for the other eye.
- Delete Reflection probe: In order to make use of the environment reflections, the reflection probe updates the reflection "realtime".
- Set ShadowResolution of the "Sun" object to other than "Ultra" and/or go to ProjectSettings -> HDRP -> Quality -> Shadows -> Filtering Quality and set the value to Medium or Low.

</details>

## How to make objects visible to either VR or AR
Please make use of the specific layers. (Select GameObject -> Layer (Inspector, located at the upper right).)

## How to use hand interactions
<details>
<summary>Click to expand!</summary>

Enable the specific checkbox inside the "XR Feature Manager" (XRFeatureManager.cs) and make sure that your device supports hand tracking. 
In order for an object to support hand tracking/interactions, add the "Pickable" tag to it. Note, that this will add a leap-script to the object on startup which lets the user grab/interact with the object.

<bold>However, doing so doesn't mean that you also can interact with it with your controller. You still need to add the "XR Grab Interactable" script for standard interaction.</bold>

In order to touch UI buttons, those elements need to have two box colliders, a rigidbody and a (leap) "Interaction Button" script. One box collider needs to have "is trigger" activated for the Raycasting (controller interaction) to work and one box collider needs to have a disabled "is trigger" for the hand interaction to work. 
Note that you also have to implement Event handling TWO times: "on click" (for the controller useage) AND "on press" (for the hand interaction) - ideally with the same event.

You can also change between AR and VR by raising the left palm of your hand and clicking on one of each buttons. This is achieved with the "Attachment Hands" object.

### OpenXR
There is a OpenXR branch of UltraLeaps handtracking plugin which is in development. In future releases, it could be interesting to add this approach to the template.s

</details>

## How to use the button
<details>
<summary>Click to expand!</summary>
The important script in order to use the button is located at the child object "Push" and is called "PhysicsButton". This button works by pushing the button physically which is why it's possible to do so with your hands as well as your controller or any other Rigidbody object.


Only use the OnPressed function inside the "PhysicsButton" script! This is the one checking if the button was pressed.
If you want to use your ray casting controller to trigger the button, check the checkbox called "Toggle With Ray".
This will activate the XR Simple Interaction script which calls a method inside "PhysicsButton.cs" if triggered.

You may also use another type of trigger with Unitys new Input System. To do so, find the "External Trigger" bar at the bottom of the "Phsyicsbutton" script and add an Input binding. This can also be useful for debugging purposes.
</details>

## How to add another asset to the package / create a new pickable object
<details>
<summary>Click to expand!</summary>

- Add the model inside the Samples->Asset->Models folder
- Inside Unity, extract the Texture and Material (click on the model, go to Inspector->Materials and click the button) to the Materials/Texture folder
- Create a new Prefab
    - Add a box collider/rigidbody
    - Add a Grab Interactable
    - Add the tag "Pickable"
    - Add the layer ARObject or VRObject if the object should be visible in either of one modes or nothing/any other layer if it should be visible in both modes.

</details>