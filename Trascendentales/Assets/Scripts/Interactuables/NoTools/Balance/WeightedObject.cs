using UnityEngine;

public class WeightedObject : MonoBehaviour
{
    public float weight = 1f; // Peso del objeto
    [SerializeField] private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.mass = transform.localScale.magnitude * 3f;
        weight = rb.mass;
    }
}
