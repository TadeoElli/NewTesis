using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GemIluminable : MonoBehaviour, IIlluminable
{
    [Header("Configuración de la carga")]
    [SerializeField] private float maxCharge = 1f; // Tiempo requerido para activarse
    [SerializeField] private float dischargeRate = 0.1f; // Velocidad de descarga por segundo
    [SerializeField] private float chargeRate = 0.1f; // Velocidad de carga por segundo

    [Header("Eventos")]
    [SerializeField] private UnityEvent onActivated; // Evento al activarse
    [SerializeField] private UnityEvent onDeactivated; // Evento al desactivarse

    private float currentCharge = 0f; // Nivel actual de carga
    private bool isBeingIlluminated = false; // Indica si está siendo iluminada
    private bool isActive = false; // Indica si está activa
    private Renderer rend;
    private MaterialPropertyBlock block;

    public void OnLightOn()
    {
        // Lógica de carga mientras se llama a OnLightOn
        isBeingIlluminated = true;
        currentCharge += chargeRate * Time.deltaTime;
        currentCharge = Mathf.Clamp(currentCharge, 0.05f, maxCharge);

        // Cambia a activo si se completa la carga
        if (currentCharge >= maxCharge && !isActive)
        {
            isActive = true;
            onActivated?.Invoke();
        }
    }
    private void Start()
    {
        rend = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
        rend.GetPropertyBlock(block, 0); // Obtiene el MaterialPropertyBlock para el segundo material (índice 1)

    }

    private void Update()
    {
        // Si no está siendo iluminada, disminuye la carga
        block.SetFloat("_Brigtness", currentCharge); // Cambia el color del segundo material
        rend.SetPropertyBlock(block, 0);         // Aplica los cambios al segundo material
        if (!isBeingIlluminated)
        {
            currentCharge -= dischargeRate * Time.deltaTime;
            currentCharge = Mathf.Clamp(currentCharge, 0.05f, maxCharge);

            // Cambia a inactivo si la carga llega a cero
            if (currentCharge <= 0.1f && isActive)
            {
                isActive = false;
                onDeactivated?.Invoke();
            }
        }

        // Resetea el flag para evitar acumulación de carga si no es iluminada en el próximo frame
        isBeingIlluminated = false;
    }
}
