# How to create a new device feature

## 1. Create a new script for your feature
This script has to use the "AbstractDeviceFeature.cs" as a base class. You may have to implement the methods XRStart() and XRUpdate().

## 2. Create a new GameObject and add your script.
Go to the GameObject "XR Scene Manager" and look for the "XR Feature Manager". Create an empty GameObject as a child. You might need to name it accordingly to the device you want to add.
Don't forget to add your script to the empty GameObject you created.

## 3. Add your script to the list
Inside Unity, go once again to the GameObject "XR Scene Manager" and look its attached script. Under "devices", you'll find a list where you should add the GameObject you've just created.

## 4. Add your device to the DeviceManager script
You have to adjust the list in the "Device Manager" GameObject, in order that you may select your specific device. You may open the likewise called script and add it inside the "enum" object at the top.

## 5. Adjust the XRFeatureManager script
Finally, you have to go to the "XR Feature Manager" GameObject once again and open up its script. Inside, at the bottom, you find the "LoadDeviceFeatures()" method. At the switch statement, add your script according to the index of the list (have a look at step 3). (e.g. devices[yourIndex])
