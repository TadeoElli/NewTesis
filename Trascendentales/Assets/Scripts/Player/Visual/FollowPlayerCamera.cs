using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    InputManager inputManager;
    private bool followPlayer = true;
    private Transform targetTransform;
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
            transform.position = Vector3.SmoothDamp(transform.position, targetTransform.position, ref velocity, smoothTime);
        }
        else
        {
            // Seguir al jugador si no estamos en transición
            transform.position = new Vector3(inputManager.transform.position.x, inputManager.transform.position.y, 0);
        }
    }



    public void SetNewTransform(Transform newTransform)
    {
        followPlayer = false;
        targetTransform = newTransform;
    }

    public void FollowPlayer()
    {
        Debug.Log(gameObject);
        followPlayer = true;
        //StartCoroutine(StartFollowPLayer());
    }

    private IEnumerator StartFollowPLayer()
    {
        yield return new WaitForSeconds(1);
    }
}

