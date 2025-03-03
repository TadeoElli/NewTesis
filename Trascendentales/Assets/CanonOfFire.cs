using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonOfFire : MonoBehaviour
{
    public List<Transform> targetPositions; // Lista de posiciones objetivo
    public float speed = 5f; // Velocidad del movimiento
    private Transform currentTarget; // Posici�n objetivo actual

    private void Start()
    {
        // Selecciona una posici�n aleatoria al inicio
        SelectNewTarget();
    }

    private void Update()
    {
        // Si no hay posici�n objetivo actual, selecciona una nueva
        if (currentTarget == null)
        {
            SelectNewTarget();
        }

        // Mueve el objeto hacia la posici�n objetivo
        MoveTowardsTarget();

        // Verifica si ha llegado a la posici�n objetivo
        if (HasReachedTarget())
        {
            SelectNewTarget();
        }
    }

    private void SelectNewTarget()
    {
        // Selecciona una posici�n aleatoria de la lista
        currentTarget = targetPositions[Random.Range(0, targetPositions.Count)];
    }

    private void MoveTowardsTarget()
    {
        // Mueve el objeto hacia la posici�n objetivo
        transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, speed * Time.deltaTime);
    }

    private bool HasReachedTarget()
    {
        // Verifica si el objeto ha llegado a la posici�n objetivo
        return Vector3.Distance(transform.position, currentTarget.position) < 0.1f;
    }
    

}
