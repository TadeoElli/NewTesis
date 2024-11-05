using System.Collections;
using UnityEngine;

public class Balance_Attack_Method : MonoBehaviour
{
    [SerializeField] private Transform player; // Referencia al jugador
    [SerializeField] private float hoverPositionY, floorPositionY; // Referencia al jugador
    [SerializeField] private float hoverDuration = 3f; // Tiempo que sigue al jugador
    [SerializeField] private float dropWaitTime = 2f; // Tiempo antes de caer
    [SerializeField] private float resetWaitTime = 3f; // Tiempo antes de volver a subir
    [SerializeField] private float fallSpeed = 5f; // Velocidad de caída
    [SerializeField] private float riseSpeed = 5f; // Velocidad de subida
    [SerializeField] private float speedMultiplier = 1f; // Velocidad de subida

    private bool isFollowing = false;
    private bool isFalling = false;
    private Vector3 targetPosition;

    void Start()
    {
        StartCoroutine(HoverCycle());
    }

    private IEnumerator HoverCycle()
    {
        while (true)
        {
            // Subir y seguir al jugador
            isFollowing = true;
            yield return new WaitForSeconds(hoverDuration * speedMultiplier);

            // Detener el seguimiento y esperar antes de caer
            isFollowing = false;
            yield return new WaitForSeconds(dropWaitTime * speedMultiplier);

            // Caer al suelo
            isFalling = true;
            while (isFalling)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, floorPositionY, transform.position.z), fallSpeed * Time.deltaTime);
                if (transform.position.y <= (floorPositionY + 0.1f))
                {
                    isFalling = false; // Deja de caer cuando toca el suelo
                }
                yield return null;
            }

            // Esperar antes de volver a subir
            yield return new WaitForSeconds(resetWaitTime * speedMultiplier);

            // Subir de nuevo
            while (transform.position.y < hoverPositionY)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, hoverPositionY, transform.position.z), riseSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }

    void Update()
    {
        if (isFollowing && player != null)
        {
            // Seguir la posición del jugador desde arriba
            targetPosition = new Vector3(player.position.x, hoverPositionY, player.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, fallSpeed * Time.deltaTime);
        }
    }

    // Funciones públicas para ajustar la velocidad
    public void IncreaseSpeed()
    {
        speedMultiplier  -= 0.3f;
    }

}

