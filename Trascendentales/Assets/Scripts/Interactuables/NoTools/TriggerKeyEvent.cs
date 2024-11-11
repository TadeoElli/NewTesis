using UnityEngine;
using UnityEngine.Events;

public class TriggerKeyEvent : MonoBehaviour
{
    [SerializeField] private float triggerStayTime = 1f; // Tiempo mínimo en el centro para activar el evento
    [SerializeField] private float centerThreshold = 0.2f; // Proporción de distancia al centro, ajusta según sea necesario
    public UnityEvent onKeyCentered; // Evento que se activa cuando la clave cumple las condiciones

    [SerializeField]private Collider triggerCollider;
    private bool isKeyCentered, wasActivated;
    private float timer;

    private void Start()
    {
        timer = 0f;
        isKeyCentered = false;
        wasActivated = false;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("entro");
        // Verifica si el objeto es la "Key"
        if (wasActivated)
            return;
        if (other.CompareTag("Key"))
        {
            // Calcula la distancia entre el objeto y el centro del trigger
            Vector3 centerPosition = triggerCollider.bounds.center;
            float distance = Vector3.Distance(centerPosition, other.transform.position);

            // Verifica si está en el centro (dentro del threshold) y ocupando el espacio necesario
            if (distance <= triggerCollider.bounds.extents.x * centerThreshold)
            {
                timer += Time.deltaTime;

                if (timer >= triggerStayTime)
                {
                    onKeyCentered.Invoke(); // Dispara el evento
                    Debug.Log("disparo");
                    wasActivated = true;
                }
            }
            else
            {
                timer = 0f; // Reinicia el temporizador si se sale del área central
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (wasActivated)
            return;
        if (other.CompareTag("Key"))
        {
            timer = 0f; // Reinicia el temporizador cuando el objeto sale del trigger
        }
    }
}

