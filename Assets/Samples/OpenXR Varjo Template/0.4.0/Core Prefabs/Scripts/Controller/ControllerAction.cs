using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace Controller
{
    public class ControllerAction : MonoBehaviour
    {

        public InputAction showTeleporter;
        public InputAction showPointer;

        private XRRayInteractor xri;
        private bool inputHandlerPointer = false;
        private bool inputHandlerTeleporter = false;

        // Start is called before the first frame update
        void Start()
        {
            xri = this.GetComponent<XRRayInteractor>();
        }
        void Awake()
        {
            showPointer.started += ctx =>
            {
                if (!inputHandlerTeleporter)
                {
                    inputHandlerPointer = true;
                    //Code for turining the ray interactor into pointer settings
                    xri.lineType = XRRayInteractor.LineType.StraightLine;
                    xri.enableUIInteraction = true;

                    xri.interactionLayers = InteractionLayerMask.GetMask("Default");

                    //xri.raycastMask;
                }

            };

            showPointer.canceled += ctx =>
            {
                inputHandlerPointer = false;
            };

            showTeleporter.started += ctx =>
            {
                if (!inputHandlerPointer)
                {
                    inputHandlerTeleporter = true;
                    //Code for turning the ray interactor into teleporter settings
                    xri.lineType = XRRayInteractor.LineType.ProjectileCurve;
                    xri.enableUIInteraction = false;

                    xri.interactionLayers = InteractionLayerMask.GetMask("Raycasts");
                    //xri.raycastMask;
                }
            };

            showTeleporter.canceled += ctx =>
            {
                inputHandlerTeleporter = false;
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
}
