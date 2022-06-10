using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class Controller_XRI : MonoBehaviour
    {
        [Header("Select hand")]
        public InputAction triggerAction;
        public InputAction triggerButtonAction;
        public InputAction gripButtonAction;
        public InputAction primary2DAxisTouchAction;
        public InputAction primary2DAxisClickAction;
        public InputAction primaryButtonAction;

        [Header("Controllers parts")]
        public GameObject controller;
        public GameObject bodyGameobject;
        public GameObject touchPadGameobject;
        public GameObject menubuttonGameobject;
        public GameObject triggerGameobject;
        public GameObject systemButtonGameobject;
        public GameObject gripButtonGameobject;

        [Header("Controller material")]
        public Material controllerMaterial;

        [Header("Controller button highlight material")]
        public Material buttonPressedMaterial;
        public Material touchpadTouchedMaterial;

        [Header("Visible only for debugging")]
        public bool triggerButton;
        public bool gripButton;
        public bool primary2DAxisTouch;
        public bool primary2DAxisClick;
        public bool primaryButton;
        public float trigger;

        //private List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();
        //private UnityEngine.XR.InputDevice device;

        private Vector3 triggerRotation; // Controller trigger rotation
        private bool checkTriggerValue = false;

        public bool TriggerButton { get { return triggerButton; } }

        public bool GripButton { get { return gripButton; } }

        public bool Primary2DAxisTouch { get { return primary2DAxisTouch; } }

        public bool Primary2DAxisClick { get { return primary2DAxisClick; } }

        public bool PrimaryButton { get { return primaryButton; } }


        public float Trigger { get { return trigger; } }

        void OnEnable()
        {
            triggerAction.Enable();
            triggerButtonAction.Enable();
            primary2DAxisClickAction.Enable();
            primary2DAxisTouchAction.Enable();
            gripButtonAction.Enable();
            primaryButtonAction.Enable();
        }

        private void OnDisable()
        {

            triggerAction.Disable();
            triggerButtonAction.Disable();
            primary2DAxisClickAction.Disable();
            primary2DAxisTouchAction.Disable();
            gripButtonAction.Disable();
            primaryButtonAction.Disable();

        }

        void Update()
        {
            if (checkTriggerValue)
            {
                trigger = triggerAction.ReadValue<float>();
                triggerRotation.Set(trigger * -30f, 0, 0);
                triggerGameobject.transform.localRotation = Quaternion.Euler(triggerRotation);
            }
        }

        private void Awake()
        {

            // Trigger
            triggerAction.started += ctx =>
            {
                checkTriggerValue = true;
            };

            triggerAction.canceled += ctx =>
            {
                checkTriggerValue = false;
            };

            // Trigger Button
            triggerButtonAction.started += ctx =>
            {
                triggerButton = true;
                triggerGameobject.GetComponent<MeshRenderer>().material = buttonPressedMaterial;
            };

            triggerButtonAction.canceled += ctx =>
            {
                triggerButton = false;
                triggerGameobject.GetComponent<MeshRenderer>().material = controllerMaterial;
            };

            // Grip Button
            gripButtonAction.started += ctx =>
            {
                gripButton = true;
                gripButtonGameobject.GetComponent<MeshRenderer>().material = buttonPressedMaterial;
            };

            gripButtonAction.canceled += ctx =>
            {
                gripButton = false;
                gripButtonGameobject.GetComponent<MeshRenderer>().material = controllerMaterial;
            };

            // Primary Button
            primaryButtonAction.started += ctx =>
            {
                primaryButton = true;
                menubuttonGameobject.GetComponent<MeshRenderer>().material = buttonPressedMaterial;
            };

            primaryButtonAction.canceled += ctx =>
            {
                primaryButton = false;
                menubuttonGameobject.GetComponent<MeshRenderer>().material = controllerMaterial;
            };

            // Primary Axis Touch
            primary2DAxisTouchAction.started += ctx =>
            {
                primary2DAxisTouch = true;
                touchPadGameobject.GetComponent<MeshRenderer>().material = touchpadTouchedMaterial;
            };

            primary2DAxisTouchAction.canceled += ctx =>
            {
                primary2DAxisTouch = false;
                touchPadGameobject.GetComponent<MeshRenderer>().material = controllerMaterial;

            };

            // Primary Axis Click
            primary2DAxisClickAction.started += ctx =>
            {
                primary2DAxisClick = true;
                touchPadGameobject.GetComponent<MeshRenderer>().material = buttonPressedMaterial;
            };

            primary2DAxisClickAction.canceled += ctx =>
            {
                primary2DAxisClick = false;
                touchPadGameobject.GetComponent<MeshRenderer>().material = touchpadTouchedMaterial;
            };
        }
    }
}
