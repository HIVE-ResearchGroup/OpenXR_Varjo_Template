using UnityEngine;

public class Varjo_Explosion : MonoBehaviour
{
    public float blastradius = 0.5f;
    public float explosionForce = 500f;

    bool hasExploded = false;

    void Explode()
    {
        Collider[] collidersFracturing = Physics.OverlapSphere(transform.position, blastradius);

        foreach (Collider objectInRange in collidersFracturing)
        {
            Varjo_Fracture fracture = objectInRange.GetComponent<Varjo_Fracture>();
            if (fracture != null)
            {
                fracture.Destroy();
            }
        }

        Collider[] collidersMove = Physics.OverlapSphere(transform.position, blastradius);

        foreach (Collider objectInRange in collidersMove)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, blastradius);
            }
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Fracture") && !hasExploded)
        {
            hasExploded = true;
            Explode();
        }
    }
}
