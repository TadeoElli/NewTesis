using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GemIluminable : MonoBehaviour, IIlluminable
{
    [Header("Configuración de la carga")]
    [SerializeField] private float maxCharge = 2f; // Tiempo requerido para activarse
    [SerializeField] private float dischargeRate = 1f; // Velocidad de descarga por segundo
    [SerializeField] private float chargeRate = 1f; // Velocidad de carga por segundo

    [Header("Eventos")]
    [SerializeField] private UnityEvent onActivated; // Evento al activarse
    [SerializeField] private UnityEvent onDeactivated; // Evento al desactivarse

    private float currentCharge = 0f; // Nivel actual de carga
    private bool isBeingIlluminated = false; // Indica si está siendo iluminada
    private bool isActive = false; // Indica si está activa

    public void OnLightOn()
    {
        // Lógica de carga mientras se llama a OnLightOn
        isBeingIlluminated = true;
        currentCharge += chargeRate * Time.deltaTime;
        currentCharge = Mathf.Clamp(currentCharge, 0, maxCharge);

        // Cambia a activo si se completa la carga
        if (currentCharge >= maxCharge && !isActive)
        {
            isActive = true;
            onActivated?.Invoke();
        }
    }

    private void Update()
    {
        // Si no está siendo iluminada, disminuye la carga
        if (!isBeingIlluminated)
        {
            currentCharge -= dischargeRate * Time.deltaTime;
            currentCharge = Mathf.Clamp(currentCharge, 0, maxCharge);

            // Cambia a inactivo si la carga llega a cero
            if (currentCharge <= 0 && isActive)
            {
                isActive = false;
                onDeactivated?.Invoke();
            }
        }

        // Resetea el flag para evitar acumulación de carga si no es iluminada en el próximo frame
        isBeingIlluminated = false;
    }
}
