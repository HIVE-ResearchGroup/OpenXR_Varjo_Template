using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

// Axel Bauer, Varjo Dev Team
// 2022
namespace Core
{
    public enum XRmode
    {
        AR,
        VR
    };

    public class AR_VR_Toggle : MonoBehaviour
    {
        [HideInInspector]
        public static XRmode staticSelectedMode;

        [SerializeField]
        private XRmode selectedMode;
    
        public InputAction xrToggleAction;
        public UnityEvent toggleEvent;

        // Start is called before the first frame update
        void Start()
        {
            staticSelectedMode = selectedMode;
            Debug.LogWarning("XRMode: You set the mode to " + staticSelectedMode + "!");
        }

        // Update is called once per frame
        void Awake()
        {

            xrToggleAction.performed +=
                ctx =>
                {
                    switch (staticSelectedMode)
                    {
                        case XRmode.AR:
                            staticSelectedMode = XRmode.VR;
                            break;
                        case XRmode.VR:
                            staticSelectedMode = XRmode.AR;
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

        public void SetMode(XRmode mode)
        {
            staticSelectedMode = mode;
            toggleEvent.Invoke();
        }

        public void SetModeToAR()
        {
            staticSelectedMode = XRmode.AR;
            toggleEvent.Invoke();
        }

        public void SetModeToVR()
        {
            staticSelectedMode = XRmode.VR;
            toggleEvent.Invoke();
        }
    
        void OnValidate()
        {
            staticSelectedMode = selectedMode;
        }
    }
}