using System.Collections;
using UnityEngine;

public class Steam : MonoBehaviour
{
    [SerializeField] private float baseForce = 10f; // Fuerza base del torbellino
    public Transform pointDirection;
    public ParticleSystem gasParticles,steamParticles;
    [SerializeField] private AudioSource loopSource; // Evento al desactivarse
    private bool canApplyForce = true; // Flag to control force application

    private void Start()
    {
        loopSource.volume = AudioManager.Instance.GetEffectsVolume();
        loopSource.Play();
        StartCoroutine(CheckForOtherPipes());
    }
    private IEnumerator CheckForOtherPipes()
    {
        while (true) // Run indefinitely until stopped
        {
            // Perform raycast every second
            yield return new WaitForSeconds(1f);

            // Calculate the distance for the raycast based on your requirements
            float rayDistance = Vector3.Distance(transform.position, pointDirection.position);

            // Perform the raycast
            RaycastHit hit;
            if (Physics.Raycast(transform.position, pointDirection.position - transform.position, out hit, rayDistance))
            {
                // Check if the hit object is on the "Pipes" layer (layer 8 is typically used for "Pipes")
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Pipes"))
                {
                    canApplyForce = false; // Disable force application

                    /*// Deactivate other particle system if needed
                    if (gasParticles != null)
                    {
                        gasParticles.Stop(); // Stop or deactivate other particles
                    }
                    */
                    //Debug.Log("Raycast hit Pipes: No force applied.");
                }
            }
            else
            {
                canApplyForce = true; // Enable force application

                // Activate other particle system if needed
                /*if (gasParticles != null)
                {
                    gasParticles.Play(); // Start or activate other particles
                }
                */
                //Debug.Log("Raycast did not hit Pipes: Force can be applied.");
            }
        }
    }
    public void ActivateParticles()
    {
        gasParticles.Play(true);
        steamParticles.Play(true);
    }
    public void DesactivateParticles()
    {
        gasParticles.Stop();
        steamParticles.Stop();
    }
    private void OnParticleCollision(GameObject other)
    {
        // Verificamos si el objeto que entra es el jugador
        if (canApplyForce && other.layer == 3 || other.layer == 6)
        {
            Rigidbody objectRb = other.GetComponent<Rigidbody>();


            if (objectRb != null)
            {
                if (other.TryGetComponent<PlayerLocomotion>(out PlayerLocomotion playerLm))
                    objectRb.velocity = new Vector3(objectRb.velocity.x, 0, objectRb.velocity.z);
                // Calculamos la dirección del torbellino (hacia donde apunta)
                Vector3 launchDirection = (pointDirection.position - transform.position).normalized;

                // Calculamos la fuerza según la escala del torbellino
                float scaleFactor = transform.localScale.magnitude; // Tamaño proporcional
                float currentForce = baseForce * scaleFactor;
                // Aplicamos la fuerza al jugador
                objectRb.AddForce(launchDirection * currentForce, ForceMode.Force);

                //Debug.Log($"Jugador lanzado con fuerza {currentForce} en dirección {launchDirection}");
            }
        }
    }

}