using System.Collections;
using UnityEngine;

public class Balance_Attack_Method : MonoBehaviour
{
    [SerializeField] private Transform player; // Referencia al jugador
    [SerializeField] private float hoverPositionY, floorPositionY; // Alturas
    [SerializeField] private float hoverDuration = 3f; // Tiempo que sigue al jugador
    [SerializeField] private float dropWaitTime = 2f; // Tiempo antes de caer
    [SerializeField] private float resetWaitTime = 3f; // Tiempo antes de volver a subir
    [SerializeField] private float fallSpeed = 5f; // Velocidad de caída
    [SerializeField] private float riseSpeed = 5f; // Velocidad de subida
    [SerializeField] private float speedMultiplier = 1f; // Velocidad global de las acciones

    [Header("Feedback")]
    [SerializeField] private GameObject attackRangeGizmo; // Indicador estático
    [SerializeField] private LayerMask groundLayer;

    private bool isFollowing = false;
    private bool isFalling = false;
    private Vector3 targetPosition;



    void OnEnable()
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

            // Detener el seguimiento y activar el tiempo de espera antes de caer
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

        UpdateIndicators(); // Mantener actualizado el feedback
    }

    private void UpdateIndicators()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 20, groundLayer))
        {
            Vector3 groundPosition = hitInfo.point;

            // Posicionar el indicador estático
            if (attackRangeGizmo != null)
            {
                attackRangeGizmo.transform.position = groundPosition;
            }

        }
    }


    // Funciones públicas para ajustar la velocidad
    public void IncreaseSpeed()
    {
        speedMultiplier -= 0.3f;
    }
}

