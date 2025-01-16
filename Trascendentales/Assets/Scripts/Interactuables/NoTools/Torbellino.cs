using UnityEngine;

public class Torbellino : MonoBehaviour
{
    [SerializeField] private float baseForce = 10f; // Fuerza base del torbellino
    [SerializeField] private Transform pointDirection;
    private float currentForce = 0f;
    [SerializeField] AudioClip tornadoClip;

    private void OnTriggerEnter(Collider other)
    {
        // Verificamos si el objeto que entra es el jugador
        if (other.gameObject.layer == 3 || other.gameObject.layer == 6)
        {
            Rigidbody objectRb = other.GetComponent<Rigidbody>();


            if (objectRb != null)
            {
                if(other.TryGetComponent<PlayerLocomotion>(out PlayerLocomotion playerLm))
                    playerLm.DisableMovement(0.25f);
                // Calculamos la dirección del torbellino (hacia donde apunta)
                Vector3 launchDirection = (pointDirection.position - transform.position).normalized;

                // Calculamos la fuerza según la escala del torbellino
                float scaleFactor = transform.localScale.magnitude; // Tamaño proporcional
                currentForce = baseForce * scaleFactor;
                if (playerLm == null)
                    currentForce = currentForce * 5f;
                else
                    objectRb.velocity = new Vector3(objectRb.velocity.x,0,objectRb.velocity.z);
                // Aplicamos la fuerza al jugador
                objectRb.AddForce(launchDirection * currentForce, ForceMode.Force);

                //AudioManager.Instance.PlaySoundEffect(tornadoClip);
                Debug.Log($"Jugador lanzado con fuerza {currentForce} en dirección {launchDirection}");
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // Verificamos si el objeto que entra es el jugador
        if (other.gameObject.layer == 3) 
        {
            Rigidbody objectRb = other.GetComponent<Rigidbody>();

            if (objectRb != null)
            {
                objectRb.velocity = new Vector3(objectRb.velocity.x,0,objectRb.velocity.z);
                Vector3 launchDirection = (pointDirection.position - transform.position).normalized;

                // Calculamos la fuerza según la escala del torbellino
                float scaleFactor = transform.localScale.magnitude; // Tamaño proporcional
                currentForce = baseForce * scaleFactor;

                // Aplicamos la fuerza al jugador
                objectRb.AddForce(launchDirection * currentForce, ForceMode.Force);

                //AudioManager.Instance.PlaySoundEffect(tornadoClip);
                Debug.Log($"Jugador lanzado con fuerza {currentForce} en dirección {launchDirection}");
            }
        }
    }

}

