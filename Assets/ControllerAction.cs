using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ControllerAction : MonoBehaviour
{

    public InputAction showTeleporter;
    public InputAction showPointer;

    private XRRayInteractor xri;

    // Start is called before the first frame update
    void Start()
    {
        xri = this.GetComponent<XRRayInteractor>();
    }
    void Awake()
    {
        showPointer.started += ctx =>
        {
            Debug.Log("showPointer started");
            //Code for turining the ray interactor into pointer settings
            xri.lineType = XRRayInteractor.LineType.StraightLine;
            xri.enableUIInteraction = true;

            xri.interactionLayers = InteractionLayerMask.GetMask("Default");

            //xri.raycastMask;
        };

        showPointer.canceled += ctx =>
        {
            Debug.Log("showPointer canceled");
        };

        showTeleporter.started += ctx =>
        {
            Debug.Log("showTeleporter started");
            //Code for turnign the ray interactor into teleporter settings
            xri.lineType = XRRayInteractor.LineType.ProjectileCurve;
            xri.enableUIInteraction = false;

            xri.interactionLayers = InteractionLayerMask.GetMask("Raycasts");
            //xri.raycastMask;
        };

        showTeleporter.canceled += ctx =>
        {
            Debug.Log("showTeleporter canceled");
        };
    }

    private void OnEnable()
    {
        showTeleporter.Enable();
        showPointer.Enable();

    }

    private void OnDisable()
    {
        showTeleporter.Disable();
        showPointer.Disable();
    }
}
