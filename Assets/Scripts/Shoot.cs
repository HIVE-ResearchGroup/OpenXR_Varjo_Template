using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public float energyFactor;
    public GameObject projectilePrefab;
    public Transform projectileOrigin;
    public InputAction shootingAction;


    bool buttonDown;
    float energy;
    Rigidbody rb;
    GameObject projectile;

    private void Awake()
    {
        shootingAction.started += ctx =>
        {
            projectile = Instantiate(projectilePrefab, projectileOrigin.transform.position, projectileOrigin.transform.rotation);
            rb = projectile.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            projectile.transform.parent = projectileOrigin;
        };

        shootingAction.performed += ctx =>
        {
            energy = energy + Time.deltaTime * energyFactor;
        };

        shootingAction.canceled += ctx =>
        {
            if (projectile && rb)
            {
                rb.isKinematic = false;
                projectile.transform.parent = null;
                rb.AddForce(projectileOrigin.transform.forward * energy, ForceMode.Impulse);
            }
            buttonDown = false;
            energy = 0f;
        };
    }

    private void OnEnable()
    {
        shootingAction.Enable();
    }

    private void OnDisable()
    {
        shootingAction.Disable();
    }
}


/*void Start()
    {
        controller = GetComponent<Controller>();
    }

    void Update()
    {
        if (controller.Primary2DAxisClick)
        {
            if (!buttonDown)
            {
                // Button is pressed, projectile is created

            }
            else
            {
                // Button is held down, projectile gets energy
                energy = energy + Time.deltaTime * energyFactor;
            }
        }
        else if (!controller.Primary2DAxisClick && buttonDown)
        {
            // Button is released, projectile is released
            if (projectile && rb)
            {
                rb.isKinematic = false;
                projectile.transform.parent = null;
                rb.AddForce(projectileOrigin.transform.forward * energy, ForceMode.Impulse);
            }
            buttonDown = false;
            energy = 0f;
        }
    }
}*/