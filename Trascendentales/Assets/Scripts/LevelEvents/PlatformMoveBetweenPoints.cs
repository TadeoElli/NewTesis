using System.Collections.Generic;
using UnityEngine;

public class PlatformMoveBetweenPoints : MonoBehaviour
{
    [SerializeField] private List<Vector3> points; // Puntos que define el camino de la plataforma
    [SerializeField] private float speed = 2f; // Velocidad de movimiento
    [SerializeField] private bool reverseAtEnd = false; // True: el camino se recorre en reversa; False: vuelve al inicio

    private int currentPointIndex = 0; // Índice del punto actual
    private bool isMoving = false;
    private bool isReversing = false;

    private void Update()
    {
        if (!isMoving || points.Count == 0) return;

        // Mueve la plataforma hacia el punto actual
        transform.position = Vector3.MoveTowards(transform.position, points[currentPointIndex], speed * Time.deltaTime);

        // Verifica si llegó al punto actual
        if (Vector3.Distance(transform.position, points[currentPointIndex]) < 0.1f)
        {
            if (isReversing)
                currentPointIndex--; // Si está en reversa, retrocede en la lista
            else
                currentPointIndex++; // Avanza al siguiente punto

            // Verifica si llegó al final de la lista de puntos
            if (currentPointIndex >= points.Count)
            {
                if (reverseAtEnd)
                {
                    isReversing = true; // Empieza a moverse en reversa
                    currentPointIndex = points.Count - 2; // Comienza en el penúltimo punto
                }
                else
                {
                    currentPointIndex = 0; // Vuelve al primer punto
                }
            }
            else if (currentPointIndex < 0)
            {
                isReversing = false; // Cambia la dirección a avanzar nuevamente
                currentPointIndex = 1; // Empieza en el segundo punto
            }
        }
    }

    // Función pública para comenzar el movimiento
    public void StartMoving()
    {
        isMoving = true;
    }

    // Función pública para detener el movimiento
    public void StopMoving()
    {
        isMoving = false;
    }
}
