using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

// Axel Bauer
// 2022
namespace Core
{
    public enum XRmode
    {
        AR,
        VR
    };

    /**
     * Script that sets the xr mode. You might do so with the InputActions you can specify inside the editor
     */
    public class AR_VR_Toggle : MonoBehaviour
    {
        public XRmode selectedMode;
    
        public InputAction xrToggleAction;
        public UnityEvent toggleEvent;

        // Start is called before the first frame update
        void Start()
        {
            Debug.LogWarning("XRMode: You set the mode to " + selectedMode + "!");
        }

        // Update is called once per frame
        void Awake()
        {

            xrToggleAction.performed +=
                ctx =>
                {
                    switch (selectedMode)
                    {
                        case XRmode.AR:
                            selectedMode = XRmode.VR;
                            break;
                        case XRmode.VR:
                            selectedMode = XRmode.AR;
                            break;
                    }
                
                    toggleEvent.Invoke();

                };
        }

        private void OnEnable()
        {
            xrToggleAction.Enable();
        }

        private void OnDisable()
        {
            xrToggleAction.Disable();
        }

        private void OnValidate()
        {
            if (XRSceneManager.Instance && XRSceneManager.Instance.isARVRToggleActive)
                toggleEvent.Invoke();
        }

        public void SetMode(XRmode mode)
        {
            selectedMode = mode;
            toggleEvent.Invoke();
        }

        public void SetModeToAR()
        {
            selectedMode = XRmode.AR;
            toggleEvent.Invoke();
        }

        public void SetModeToVR()
        {
            selectedMode = XRmode.VR;
            toggleEvent.Invoke();
        }
    }
}