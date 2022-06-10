using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

// Axel Bauer
// 2022
public class Shoot_XRI : MonoBehaviour
{
    public float energyFactor;
    public GameObject projectilePrefab;
    public Transform projectileOrigin;
    public InputAction shootingAction;

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

            if (ctx.interaction is SlowTapInteraction)
            {
                energy = (float)(ctx.duration) * energyFactor;
            }

            if (projectile && rb)
            {
                rb.isKinematic = false;
                projectile.transform.parent = null;
                rb.AddForce(projectileOrigin.transform.forward * energy, ForceMode.Impulse);
            }
            energy = 0f;
        };

        shootingAction.canceled += ctx =>
        { //if pressed too fast
            rb.isKinematic = false;
            projectile.transform.parent = null;
            rb.AddForce(projectileOrigin.transform.forward * energy, ForceMode.Impulse);
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