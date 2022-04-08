using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Axel Bauer
// 2022
public class LoadARVRObjectsToMode : MonoBehaviour
{
    [Header("Ground Settings")]
    public GameObject ground;
    public Material shadowCatcher;
    public bool setTransparentInAR = true;
    public bool setGroundTransparent;
    

    private AR_VR_Toggle avt;
    private XRmode m_XrMode;
    private XRmode m_storedXrMode;
    private bool m_storedGroundTransparent;
    private bool m_storedTransparentInAR;

    private Material m_InitialMaterial;

    private GameObject[] m_arObjects;
    private GameObject[] m_vrObjects;

    // Start is called before the first frame update
    void Start()
    {
        avt = this.gameObject.GetComponent<AR_VR_Toggle>();
        m_XrMode = avt.selectedMode;
        m_storedXrMode = m_XrMode;
        m_storedTransparentInAR = setGroundTransparent;

        m_storedGroundTransparent = setGroundTransparent;

        if (!avt)
        {
            Debug.LogError("LoadARVRObjectsToMode: Couldn't find AR_VR_Toggle script on this gameObject!");
        }

        groundInit();
        setGround(setGroundTransparent);
        m_arObjects = FindGameObjectsWithLayer(7);// hardcoded number - not a good solution and might change it later
        m_vrObjects = FindGameObjectsWithLayer(6);
        updateObjects();
    }

    // Update is called once per frame
    void Update()
    {
        m_XrMode = avt.selectedMode;



        //set ground
        if (setGroundTransparent != m_storedGroundTransparent) // Check if this value changed regarding ground
        {

            setGround(setGroundTransparent);
            m_storedGroundTransparent = setGroundTransparent;

        } else if (m_storedTransparentInAR != setTransparentInAR) // Check if this value changed regarding ground
        {
            setGround(setGroundTransparent);
            m_storedTransparentInAR = setTransparentInAR;
        } 
        
        else if (m_storedXrMode != m_XrMode) //set objects
        {
            updateObjects();
            setGround(setGroundTransparent);
            m_storedGroundTransparent = setGroundTransparent;

            m_storedXrMode = m_XrMode;
        }
    }

    void groundInit()
    {
        if (!shadowCatcher)
        {
            Debug.LogError("LoadARVRObjectsToMode: Please assign the according material in order to set the ground transparent!");
        }

        if (!ground)
        {
            ground = GameObject.Find("Ground");

            if (!ground)
            {
                Debug.LogError("LoadARVRObjectsToMode: Couldn't find GameObject with the name ground!");

            }
        }

        m_InitialMaterial = ground.GetComponent<Renderer>().material;
    }

    GameObject[] FindGameObjectsWithLayer(int layer) {

        GameObject[] goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var goList = new List<GameObject>();

        for (int i = 0; i < goArray.Length; i++) { 
            if (goArray[i].layer == layer) { 
                goList.Add(goArray[i]); 
            } 
        } 
        
        if (goList.Count == 0) { 
            return null; 
        } 

        return goList.ToArray(); 
    }

    void setGround(bool state)
    {
        if (state || setTransparentInAR && m_XrMode == XRmode.AR)
        {
            ground.GetComponent<Renderer>().material = shadowCatcher;
        }
        else
        {
            ground.GetComponent<Renderer>().material = m_InitialMaterial;
        }
    }

    // Checks which updates should be updated
    void updateObjects()
    {
        if (m_XrMode == XRmode.AR)
        {
            setObjects(m_arObjects, true);
            setObjects(m_vrObjects, false);

        }
        else if (m_XrMode == XRmode.VR)
        {
            setObjects(m_arObjects, false);
            setObjects(m_vrObjects, true);
        }

    }

    // Updates the objects regarding the mode
    void setObjects(GameObject[] goArray, bool state)
    {
        if (goArray != null)
        {
            for (uint i = 0; i < goArray.Length; i++)
            {
                goArray[i].SetActive(state);
            }
        }

    }
}