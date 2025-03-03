using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonOfFire : MonoBehaviour
{
    public List<Transform> targetPositions; // Lista de posiciones objetivo
    public float speed = 5f; // Velocidad del movimiento
    private Transform currentTarget; // Posición objetivo actual

    private void Start()
    {
        // Selecciona una posición aleatoria al inicio
        SelectNewTarget();
    }

    private void Update()
    {
        // Si no hay posición objetivo actual, selecciona una nueva
        if (currentTarget == null)
        {
            SelectNewTarget();
        }

        // Mueve el objeto hacia la posición objetivo
        MoveTowardsTarget();

        // Verifica si ha llegado a la posición objetivo
        if (HasReachedTarget())
        {
            SelectNewTarget();
        }
    }

    private void SelectNewTarget()
    {
        // Selecciona una posición aleatoria de la lista
        currentTarget = targetPositions[Random.Range(0, targetPositions.Count)];
    }

    private void MoveTowardsTarget()
    {
        // Mueve el objeto hacia la posición objetivo
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
    }

    private bool HasReachedTarget()
    {
        // Verifica si el objeto ha llegado a la posición objetivo
        return Vector3.Distance(transform.position, currentTarget.position) < 0.1f;
    }
    

}
