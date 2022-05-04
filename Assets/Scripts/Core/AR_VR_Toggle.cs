using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


// Axel Bauer, Varjo Dev Team
// 2022
public enum XRmode
{
    AR,
    VR
};

public class AR_VR_Toggle : MonoBehaviour
{
    public XRmode selectedMode;
    public InputAction XRToggleAction;

    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning("XRMode: You set the mode to " + selectedMode + "!");
    }

    // Update is called once per frame
    void Awake()
    {

        XRToggleAction.performed +=
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

            };
    }

    private void OnEnable()
    {
        XRToggleAction.Enable();
    }

    private void OnDisable()
    {
        XRToggleAction.Disable();
    }

    public void SetMode(XRmode mode)
    {
        selectedMode = mode;
    }

    public void SetModeToAR()
    {
        selectedMode = XRmode.AR;
    }

    public void SetModeToVR()
    {
        selectedMode = XRmode.VR;
    }
}
