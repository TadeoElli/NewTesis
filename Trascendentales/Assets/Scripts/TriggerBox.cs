using UnityEngine;
using UnityEngine.Events;

public class TriggerBox : MonoBehaviour
{
    public UnityEvent OnTriggerBoxEnter;
    public UnityEvent<Vector3> OnTriggerBoxEnterV3;
    [SerializeField] private Vector3 newVector3;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTriggerBoxEnter?.Invoke();
            OnTriggerBoxEnterV3?.Invoke(newVector3);
        }
    }
    /*private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
        }
    }*/
}
