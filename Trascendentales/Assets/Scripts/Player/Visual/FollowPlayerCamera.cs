using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    InputManager inputManager;
    private bool followPlayer = true;
    private Vector3 targetPosition;
    [SerializeField] private float smoothTime = 0.3f; // Tiempo de transición para la suavidad
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    void Update()
    {
        if (!followPlayer)
        {
            // Realizar una transición suave hacia la posición objetivo
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else
        {
            // Seguir al jugador si no estamos en transición
            transform.position = new Vector3(inputManager.transform.position.x, inputManager.transform.position.y, transform.position.z);
        }
    }

    public void SetNewPosition(Vector3 newPosition)
    {
        followPlayer = false;
        targetPosition = newPosition;
    }

    public void FollowPlayer()
    {
        targetPosition = inputManager.transform.position;
        StartCoroutine(StartFollowPLayer());
    }

    private IEnumerator StartFollowPLayer()
    {
        yield return new WaitForSeconds(1);
        followPlayer = true;
    }
}

