using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BalanceRope : MonoBehaviour
{
    public Transform balancePoint; // El punto donde la cuerda se conecta a la balanza
    public Transform platePoint;   // El punto donde la cuerda se conecta al platillo

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // Solo necesitamos dos puntos para la cuerda
    }

    void Update()
    {
        // Actualiza las posiciones de los puntos de la cuerda
        lineRenderer.SetPosition(0, balancePoint.position); // Primer punto en la balanza
        lineRenderer.SetPosition(1, platePoint.position);   // Segundo punto en el platillo
    }
}

