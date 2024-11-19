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
    [SerializeField] private GameObject attackIndicatorGizmo; // Indicador dinámico
    [SerializeField] private LayerMask groundLayer;

    private bool isFollowing = false;
    private bool isFalling = false;
    private Vector3 targetPosition;

    private float dropTimer = 0f; // Temporizador exclusivo para el tiempo de caída
    private bool isExpanding = false; // Flag para controlar la expansión del indicador dinámico

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
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 10, groundLayer))
        {
            Vector3 groundPosition = hitInfo.point;

            // Posicionar el indicador estático
            if (attackRangeGizmo != null)
            {
                attackRangeGizmo.transform.position = groundPosition;
            }

            // Posicionar y manejar el indicador dinámico
            if (attackIndicatorGizmo != null)
            {
                attackIndicatorGizmo.transform.position = groundPosition;
                if(dropTimer > hoverDuration + dropWaitTime + resetWaitTime)
                    ResetIndicatorScale();
                dropTimer += Time.deltaTime;
                float progress = Mathf.Clamp01(dropTimer / (dropWaitTime * speedMultiplier));
                attackIndicatorGizmo.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(4, 4, 4), progress);

            }
        }
    }

    private void ResetIndicatorScale()
    {
        if (attackIndicatorGizmo != null)
        {
            attackIndicatorGizmo.transform.localScale = Vector3.zero;
            dropTimer = 0;
        }
    }

    // Funciones públicas para ajustar la velocidad
    public void IncreaseSpeed()
    {
        speedMultiplier -= 0.3f;
    }
}

