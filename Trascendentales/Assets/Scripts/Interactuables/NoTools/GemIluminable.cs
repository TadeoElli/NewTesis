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
    [Header("Partículas")]
    [SerializeField] private ParticleSystem chargeParticle; // Efecto al cargar
    [SerializeField] private ParticleSystem dischargeParticle; // Efecto al descargar

    private float currentCharge = 0f; // Nivel actual de carga
    private bool isBeingIlluminated = false; // Indica si está siendo iluminada
    private bool isActive = false; // Indica si está activa
    private Renderer rend;
    private MaterialPropertyBlock block;


    private void Start()
    {
        rend = GetComponent<Renderer>();
        block = new MaterialPropertyBlock();
        rend.GetPropertyBlock(block, 0); // Obtiene el MaterialPropertyBlock para el segundo material (índice 1)
        // Asegurar que los sistemas de partículas estén detenidos al inicio
        if (chargeParticle != null) chargeParticle.Stop();
        if (dischargeParticle != null) dischargeParticle.Stop();
    }

    private void Update()
    {
        // Actualizar brillo del material según la carga
        block.SetFloat("_Brigtness", currentCharge);
        rend.SetPropertyBlock(block, 0);

        if (isBeingIlluminated)
        {
            HandleCharge();
        }
        else
        {
            HandleDischarge();
        }

        // Resetear el flag de iluminación
        isBeingIlluminated = false;
    }

    public void OnLightOn()
    {
        isBeingIlluminated = true;
    }

    private void HandleCharge()
    {
        currentCharge += chargeRate * Time.deltaTime;
        currentCharge = Mathf.Clamp(currentCharge, 0.01f, maxCharge);

        if (currentCharge >= maxCharge && !isActive)
        {
            isActive = true;
            onActivated?.Invoke();
        }

        // Manejar partículas
        if (chargeParticle != null && !chargeParticle.isPlaying)
        {
            chargeParticle.Play();
        }

        if (dischargeParticle != null && dischargeParticle.isPlaying)
        {
            dischargeParticle.Stop();
        }
    }

    private void HandleDischarge()
    {
        currentCharge -= dischargeRate * Time.deltaTime;
        currentCharge = Mathf.Clamp(currentCharge, 0.01f, maxCharge);

        if (currentCharge <= 0f && isActive)
        {
            isActive = false;
            onDeactivated?.Invoke();
        }

        // Manejar partículas
        if (currentCharge > 0.01f)
        {
            if (dischargeParticle != null && !dischargeParticle.isPlaying)
            {
                dischargeParticle.Play();
            }

            if (chargeParticle != null && chargeParticle.isPlaying)
            {
                chargeParticle.Stop();
            }
        }
        else
        {
            // Detener ambos sistemas de partículas si no hay carga
            if (chargeParticle != null && chargeParticle.isPlaying)
            {
                chargeParticle.Stop();
            }

            if (dischargeParticle != null && dischargeParticle.isPlaying)
            {
                dischargeParticle.Stop();
            }
        }
    }
}
