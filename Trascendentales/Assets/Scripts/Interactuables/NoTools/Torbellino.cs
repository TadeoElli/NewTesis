using UnityEngine;

public class Torbellino : MonoBehaviour
{
    [SerializeField] private float baseForce = 10f; // Fuerza base del torbellino
    [SerializeField] private Transform pointDirection;
    private float currentForce = 0f;


    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();

            if (playerRb != null)
            {
                // Calculamos la dirección del torbellino (hacia donde apunta)
                Vector3 launchDirection = (pointDirection.position - transform.position).normalized;

                // Calculamos la fuerza según la escala del torbellino
                float scaleFactor = transform.localScale.magnitude; // Tamaño proporcional
                currentForce = baseForce * scaleFactor;

                // Aplicamos la fuerza al jugador
                playerRb.AddForce(launchDirection * currentForce, ForceMode.Force);

                Debug.Log($"Jugador lanzado con fuerza {currentForce} en dirección {launchDirection}");
            }
        }
    }
}

