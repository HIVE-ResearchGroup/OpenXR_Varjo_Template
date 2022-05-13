using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class physicsButton : MonoBehaviour
{

    [Header("Percentage of the button press that is needed to activate the button")]
    [SerializeField] private float threshold = 0.1f;
    [Header("Zone that prevents bounce issues with controllers")]
    [SerializeField] private float deadZone = 0.025f;

    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;

    public UnityEvent onPressed, onReleased;

    public bool ToggleWithRay;
    public InputAction externalTrigger;


    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfOutOfBounce();

        if (!_isPressed && GetValue() + threshold > 1)
        {
            Pressed();
        }

        if (_isPressed && GetValue() - threshold <= 0)
        {
            Released();
        }
    }

    private void Awake()
    {
        externalTrigger.started += ctx =>
        {
            Pressed();
        };

        //released not needed since threshold won't change and automatically trigger "Released" inside update()
    }

    private void OnEnable()
    {
        externalTrigger.Enable();
    }

    private void OnDisable()
    {
        externalTrigger.Disable();
    }

    public void SimulatePress()
    {
        if (ToggleWithRay)
        {
            Pressed();
        }
    }

    private void Pressed()
    {
        _isPressed = true;
        onPressed.Invoke();
        Debug.Log("Pressed");
    }

    public void Released()
    {
        _isPressed = false;
        onPressed.Invoke();
        Debug.Log("Released");
    }

    private float GetValue()
    {
        var value = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;

        if (Mathf.Abs(value) < deadZone)
        {
            value = 0;
        }
        return Mathf.Clamp(value, -1f, 1f);
    }

    private void CheckIfOutOfBounce()
    {
        Vector3 pos = this.transform.localPosition;

        if (this.transform.localPosition.x > 0.001 || this.transform.localPosition.x < -0.001 || this.transform.localPosition.z < -0.001 || this.transform.localPosition.z > 0.001) //if pushing the button top/bottom/left/right
        {

            if (this.transform.localPosition.y <= 0f)//if that happens when pressed or glitching when pressed
            {
                this.transform.localPosition = new Vector3(0f, 0f, 0f); //keep calm!
                Debug.Log("Keep calm");
            } else
            {
                this.transform.localPosition = new Vector3(0f, 0.013f, 0f); //jump back to init position
            }
        }

        if (this.transform.localPosition.y > 0.15) //max zone (forward)
        {
            this.transform.localPosition = new Vector3(0f, 0.14f, 0f);
        }

        if (this.transform.localPosition.y < -0.001) //min zone (backward)
        {
            this.transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }
}
