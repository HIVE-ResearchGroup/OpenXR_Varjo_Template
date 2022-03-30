
using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEditor;
using UnityEditor.XR.Management;
using UnityEngine;

using UnityEngine.XR.Management;
using Varjo.XR;

public class pluginManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var generalSettings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(BuildTargetGroup.Standalone);
        var settingsManager = generalSettings.Manager;

        // Get example loaders as XRLoaders
        //var openVRLoader = new OpenVRLoader() as XRLoader;
        //var varjoLoader = new VarjoLoader() as XRLoader;

        // Adding new loaders
        // Append the new FooLoader
        /*if (!settingsManager.TryAddLoader(openVRLoader))
            Debug.Log("Adding new Foo Loader failed! Refer to the documentation for additional information!");

        // Insert the new BarLoader at the start of the list
        if (!settingsManager.TryAddLoader(varjoLoader, 0))
            Debug.Log("Adding new Bar Loader failed! Refer to the documentation for additional information!");

        // Removing loaders
        if (!settingsManager.TryRemoveLoader(openVRLoader))
            Debug.Log("Failed to remove the fooLoader! Refer to the documentation for additional information!");
        */

        // Modifying the loader list order
        var readonlyCurrentLoaders = settingsManager.activeLoaders;

        // Copy the returned read only list
        var currentLoaders = new List<XRLoader>(readonlyCurrentLoaders);

        // Reverse the list
        currentLoaders.Reverse();

        if (!settingsManager.TrySetLoaders(currentLoaders))
            Debug.Log("Failed to set the reordered loader list! Refer to the documentation for additional information!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}