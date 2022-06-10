using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;

// Axel Bauer
// 2022
namespace Core
{
    /**
     * Script that loads the object according to the set mode inside "AR_VR_Toggle"
     */
    public class LoadARVRObjectsToMode : MonoBehaviour
    {
        [HideInInspector] public static GameObject[] arObjects;
        [HideInInspector] public static GameObject[] vrObjects;


        [Header("Ground Settings")] [SerializeField]
        private GameObject ground;

        [SerializeField] private Material shadowCatcher;

        [SerializeField] private bool setTransparentInAR = true;

        [SerializeField] private bool setGroundTransparent;

        [Header("Leap Variables")] [SerializeField]
        private GameObject hands;

        [SerializeField] private bool setHandsTransparent;


        private bool _storedGroundTransparent;
        private bool _storedTransparentInAR;
        private bool _storedHandsTransparent;

        private bool _useGround = true;
        private Renderer _groundRenderer;
        private Material _initialMaterial;

        // Start is called before the first frame update
        private void Start()
        {
            _storedTransparentInAR = setGroundTransparent;
            _storedHandsTransparent = setHandsTransparent;

            GroundInit();

            if (_useGround) SetGround();

            SetHands();
            arObjects = FindGameObjectsWithLayer(7); // hardcoded number - not a good solution and might change it later
            vrObjects = FindGameObjectsWithLayer(6);
            UpdateObjects();
        }

        // Update is called once per frame
        private void Update()
        {
            if (_storedHandsTransparent != setHandsTransparent)
            {
                SetHands();
                _storedHandsTransparent = setHandsTransparent;
            }

            //set ground
            if (_useGround)
            {
                if (setGroundTransparent != _storedGroundTransparent) // Check if this value changed regarding ground
                {
                    SetGround();
                    _storedGroundTransparent = setGroundTransparent;
                }
                else if (_storedTransparentInAR != setTransparentInAR) // Check if this value changed regarding ground
                {
                    SetGround();
                    _storedTransparentInAR = setTransparentInAR;
                }
            }
        }


        public void SetModeToAll()
        {
            UpdateObjects();
            SetGround();
            SetHands();
        }


        public void SetGroundTransparent(bool state)
        {
            setGroundTransparent = state;
        }

        public void SetHandsTransparent(bool state)
        {
            setHandsTransparent = state;
        }

        private void GroundInit()
        {
            if (!shadowCatcher)
            {
                Debug.LogWarning(
                    "LoadARVRObjectsToMode: Please assign the according material in order to set the " +
                    "ground transparent! Continuing without ...");
                _useGround = false;
            }

            if (!ground)
            {
                ground = GameObject.Find("Ground");

                if (!ground)
                {
                    Debug.LogWarning("LoadARVRObjectsToMode: Couldn't find GameObject with the name ground! " +
                                     "Continuing without ...");
                    _useGround = false;
                }
            }

            _groundRenderer = ground.GetComponent<Renderer>();
            _initialMaterial = _groundRenderer.material;
        }

        private static GameObject[] FindGameObjectsWithLayer(int layer)
        {
            var goArray = FindObjectsOfType<GameObject>();
            var goList = new List<GameObject>();

            // iterate over all objects
            foreach (var gObject in goArray)
            {
                // check for layers
                if (gObject.layer == layer)
                {
                    goList.Add(gObject);
                }

                // gonna take advantage of this search and attach the leap scripts if needed
                if (gObject.CompareTag("Pickable") && XRSceneManager.Instance.isFeatureManagerActive &&
                    XRSceneManager.Instance.xrFeatureManager.enableLeapFunctionality)
                {
                    // TODO ---------------------------------------------------------------------------------------------------------------------------- gets selected two times
                    if (!gObject.GetComponent<InteractionBehaviour>())
                    {
                        gObject.AddComponent<InteractionBehaviour>();
                    }
                    
                    gObject.GetComponent<InteractionBehaviour>().allowMultiGrasp = true;
                }
            }

            // if there are no objects, return null
            if (goList.Count == 0)
            {
                return null;
            }

            return goList.ToArray();
        }

        private void SetGround()
        {
            if (_groundRenderer && _initialMaterial && shadowCatcher)
            {
                if (_useGround && 
                    (setGroundTransparent || setTransparentInAR && (XRSceneManager.Instance && XRSceneManager.Instance.isARVRToggleActive)
                                                                && XRSceneManager.Instance.arVRToggle.selectedMode == XRmode.AR))
                {
                    _groundRenderer.material = shadowCatcher;
                }
                else
                {
                    _groundRenderer.material = _initialMaterial;
                }
            }
            
        }

        // Checks which updates should be updated
        private void UpdateObjects()
        {
            if (XRSceneManager.Instance && XRSceneManager.Instance.isARVRToggleActive)
            {
                switch (XRSceneManager.Instance.arVRToggle.selectedMode)
                {
                    case XRmode.AR:
                        SetObjects(arObjects, true);
                        SetObjects(vrObjects, false);
                        break;

                    case XRmode.VR:
                    default:
                        SetObjects(arObjects, false);
                        SetObjects(vrObjects, true);
                        break;
                } 
            }
        }

        private void SetHands()
        {
            if (
                XRSceneManager.Instance && XRSceneManager.Instance.isARVRToggleActive)
            {
                switch (XRSceneManager.Instance.arVRToggle.selectedMode)
                {
                    case XRmode.AR:
                        hands.SetActive(false);
                        break;

                    case XRmode.VR:
                    default:
                        hands.SetActive(
                            !setHandsTransparent); //if setHandsTransparent == false -> hands true, else setHandstransparent => false
                        break;
                }
            }
            
        }

        // Updates the objects regarding the mode
        private static void SetObjects(GameObject[] goArray, bool state)
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
}