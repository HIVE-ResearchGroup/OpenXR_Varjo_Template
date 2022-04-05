using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public float inputDelay = 2.0f;
    private bool m_ToggleActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning("XRMode: You set the mode to " + selectedMode + "!");
    }

    // Update is called once per frame
    void Update()
    {

        // Better switch to event based in the future

        if (Input.GetKeyDown(KeyCode.T) && !m_ToggleActivated)
        {
            m_ToggleActivated = true;
            Toggle();
            StartCoroutine(ChangeTimer());
        }
    }

    void Toggle()
    {
        switch (selectedMode)
        {
            case XRmode.AR: selectedMode = XRmode.VR;
                break;
            case XRmode.VR: selectedMode = XRmode.AR;
                break;
        }

    }

    IEnumerator ChangeTimer()
    {
        yield return new WaitForSeconds(inputDelay);
        m_ToggleActivated = false;

    }
}
