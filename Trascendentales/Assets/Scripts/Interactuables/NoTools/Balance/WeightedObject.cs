using UnityEngine;

public class WeightedObject : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.mass = transform.localScale.magnitude * 3f;
    }
}
