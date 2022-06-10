using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
/**
 * Script for the physicsButton to work. Checks if triggered/pushed to trigger and makes sure that the button stays
 * in the correct position.
 */
public class PhysicsButton : MonoBehaviour
{

    [Header("Percentage of the button press that is needed to activate the button")]
    [SerializeField] private float threshold = 0.1f;
    [Header("Zone that prevents bounce issues with controllers")]
    [SerializeField] private float deadZone = 0.025f;

    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;

    public UnityEvent onPressed, onReleased;

    public bool toggleWithRay;
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
        if (toggleWithRay)
        {
            Pressed();
        }
    }

    private void Pressed()
    {
        _isPressed = true;
        onPressed.Invoke();
    }

    public void Released()
    {
        _isPressed = false;
        onPressed.Invoke();
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
        Vector3 pos = transform.localPosition;

        if (transform.localPosition.x > 0.001 || transform.localPosition.x < -0.001 || transform.localPosition.z < -0.001 || transform.localPosition.z > 0.001) // if pushing the button top/bottom/left/right
        {

            if (transform.localPosition.y <= 0f)//if that happens when pressed or glitching when pressed
            {
                transform.localPosition = new Vector3(0f, 0f, 0f); //keep calm!
            } else
            {
                transform.localPosition = new Vector3(0f, 0.013f, 0f); //jump back to init position
            }
        }

        if (transform.localPosition.y > 0.15) //max zone (forward)
        {
            transform.localPosition = new Vector3(0f, 0.14f, 0f);
        }

        if (transform.localPosition.y < -0.001) //min zone (backward)
        {
            transform.localPosition = new Vector3(0f, 0f, 0f);
        }
    }
}
