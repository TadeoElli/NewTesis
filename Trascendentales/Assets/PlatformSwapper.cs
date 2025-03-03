using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSwapper : MonoBehaviour
{
    [SerializeField] private List<Transform> platforms; // Lista de plataformas
    [SerializeField] private float moveSpeed = 5f; // Velocidad de movimiento
    [SerializeField] private float targetY = 5f; // Altura a la que se mueven las plataformas antes del intercambio

    private Transform platformA;
    private Transform platformB;
    private Vector3 originalPositionA;
    private Vector3 originalPositionB;

    private void Start()
    {
        StartCoroutine(SwapPlatformsRoutine());
    }

    private IEnumerator SwapPlatformsRoutine()
    {
        while (true)
        {
            // Seleccionar dos plataformas al azar con X y Z distintos
            SelectRandomPlatforms();

            // Guardar las posiciones originales
            originalPositionA = platformA.position;
            originalPositionB = platformB.position;

            // Mover ambas plataformas hacia la altura especificada (targetY)
            yield return MoveToY(platformA, targetY);
            yield return MoveToY(platformB, targetY);

            // Intercambiar posiciones en X
            yield return MoveToX(platformA, originalPositionB.x);
            yield return MoveToX(platformB, originalPositionA.x);

            // Intercambiar posiciones en Z
            yield return MoveToZ(platformA, originalPositionB.z);
            yield return MoveToZ(platformB, originalPositionA.z);

            // Volver a la altura original
            yield return MoveToY(platformA, originalPositionA.y);
            yield return MoveToY(platformB, originalPositionB.y);

            // Esperar un momento antes de repetir el proceso
            yield return new WaitForSeconds(1f);
        }
    }

    private void SelectRandomPlatforms()
    {
        do
        {
            platformA = platforms[Random.Range(0, platforms.Count)];
            platformB = platforms[Random.Range(0, platforms.Count)];
        } while (platformA == platformB || platformA.position.x == platformB.position.x || platformA.position.z == platformB.position.z);
    }

    private IEnumerator MoveToY(Transform platform, float targetY)
    {
        Vector3 targetPosition = new Vector3(platform.position.x, targetY, platform.position.z);
        while (Vector3.Distance(platform.position, targetPosition) > 0.01f)
        {
            platform.position = Vector3.MoveTowards(platform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator MoveToX(Transform platform, float targetX)
    {
        Vector3 targetPosition = new Vector3(targetX, platform.position.y, platform.position.z);
        while (Vector3.Distance(platform.position, targetPosition) > 0.01f)
        {
            platform.position = Vector3.MoveTowards(platform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator MoveToZ(Transform platform, float targetZ)
    {
        Vector3 targetPosition = new Vector3(platform.position.x, platform.position.y, targetZ);
        while (Vector3.Distance(platform.position, targetPosition) > 0.01f)
        {
            platform.position = Vector3.MoveTowards(platform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}

