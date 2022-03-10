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

    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning("XRMode: You set the mode to " + selectedMode + "!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
