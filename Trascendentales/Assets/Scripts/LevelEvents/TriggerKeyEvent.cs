using UnityEngine;
using UnityEngine.Events;

public class TriggerKeyEvent : MonoBehaviour
{
    [SerializeField] private float triggerStayTime = 1f; // Tiempo mínimo en el centro para activar el evento
    [SerializeField] private float centerThreshold = 0.2f; // Proporción de distancia al centro, ajusta según sea necesario
    public UnityEvent onKeyCentered; // Evento que se activa cuando la clave cumple las condiciones
    public UnityEvent onKeyExit; // Evento que se activa cuando la clave cumple las condiciones
    private GameObject keyObject;
    [SerializeField]private Collider triggerCollider;
    private bool  wasActivated;
    private float timer;

    private void Start()
    {
        timer = 0f;
        wasActivated = false;
    }

    private void Update()
    {
        if(!wasActivated)
            return;
        if(keyObject == null)
        {
            keyObject = null;
            Desactivate();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("entro");
        // Verifica si el objeto es la "Key"
        if (wasActivated)
            return;
        if (other.CompareTag("Key"))
        {
            keyObject = other.gameObject;
            // Calcula la distancia entre el objeto y el centro del trigger
            Vector3 centerPosition = triggerCollider.bounds.center;
            float distance = Vector3.Distance(centerPosition, other.transform.position);

            // Verifica si está en el centro (dentro del threshold) y ocupando el espacio necesario
            if (distance <= triggerCollider.bounds.extents.x * centerThreshold)
            {
                timer += Time.deltaTime;

                if (timer >= triggerStayTime)
                {
                    Activate();
                }
            }
            else
            {
                timer = 0f; // Reinicia el temporizador si se sale del área central
            }
        }
    }
    private void Activate()
    {
        onKeyCentered.Invoke(); // Dispara el evento
        Debug.Log("disparo");
        wasActivated = true;
    }
    private void Desactivate()
    {
        onKeyExit.Invoke(); // Dispara el evento
        Debug.Log("disparo");
        wasActivated = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            keyObject = null;
            timer = 0f; // Reinicia el temporizador cuando el objeto sale del trigger
            if (wasActivated)
            {
                Desactivate();
            }
        }
           
    }
}

