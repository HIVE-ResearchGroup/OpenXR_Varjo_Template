using UnityEngine;

public class Varjo_Fracture : MonoBehaviour
{
    public GameObject fracturedObject;

    public void Destroy()
    {
        Instantiate(fracturedObject, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
