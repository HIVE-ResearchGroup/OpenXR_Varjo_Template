using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Controller
{
    /// <summary>
    /// Checks for button input on an input action
    /// </summary>
    public class OnButtonPress : MonoBehaviour
    {
        [Tooltip("Actions to check")]
        public InputAction action = null;

        // When the button is pressed
        public UnityEvent onPress = new UnityEvent();

        // When the button is released
        public UnityEvent onRelease = new UnityEvent();

        private void Awake()
        {
            action.started += Pressed;
            action.canceled += Released;
        }

        private void OnDestroy()
        {
            action.started -= Pressed;
            action.canceled -= Released;
        }

        private void OnEnable()
        {
            action.Enable();
        }

        private void OnDisable()
        {
            action.Disable();
        }

        private void Pressed(InputAction.CallbackContext context)
        {
            onPress.Invoke();
        }

        private void Released(InputAction.CallbackContext context)
        {
            onRelease.Invoke();
        }
    }
}
