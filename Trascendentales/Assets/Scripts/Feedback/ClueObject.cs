using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueObject : MonoBehaviour
{
    [SerializeField] private float range = 5f;
    [SerializeField] private Animator popupPanel;
    [SerializeField] private float checkInterval = 0.5f; // Intervalo de tiempo para la corutina
    private InputManager playerManager;
    private Transform playerTransform;
    private bool isPlayerInRange = false;
    private bool isClueActive = false;

    void Start()
    {
        playerManager = InputManager.Instance;
        if (playerManager == null)
        {
            Debug.LogError("InputManager instance not found!");
            enabled = false; // Desactiva el script si no se encuentra el InputManager
            return;
        }

        if (popupPanel == null)
        {
            Debug.LogError("Popup Panel not assigned on gameObject " + gameObject.name + "!");
            enabled = false; // Desactiva el script si no se asigna el panel
            return;
        }

        playerTransform = playerManager.transform;
        StartCoroutine(CheckPlayerDistance()); // Inicia la corutina para verificar la distancia
    }

    private IEnumerator CheckPlayerDistance()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, playerTransform.position) < range)
            {
                if (!isPlayerInRange)
                {
                    isPlayerInRange = true;
                    playerManager.OnInteract += ManageClue; // Suscribe al evento OnInteract
                }
            }
            else
            {
                if (isPlayerInRange)
                {
                    isPlayerInRange = false;
                    playerManager.OnInteract -= ManageClue; // Suscribe al evento OnInteract
                    HidePopUp();
                }
            }
            yield return new WaitForSeconds(checkInterval); // Espera el intervalo de tiempo
        }
    }

    private void ManageClue()
    {
        if (!isClueActive)
        {
            popupPanel.SetBool("Show",true);
            isClueActive = true;
        }
        else
        {
            HidePopUp();
        }
    }
    private void HidePopUp()
    {
        if (isClueActive)
        {
            popupPanel.SetBool("Show", false);
            isClueActive = false;
        }
    }

    private void OnDisable()
    {
        // Asegurarse de desuscribirse del evento cuando el objeto se desactiva
        playerManager.OnInteract -= ManageClue;
        StopAllCoroutines(); // Detiene la corutina
    }

    // Para visualizar el rango en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

