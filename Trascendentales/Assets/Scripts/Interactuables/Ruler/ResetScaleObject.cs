using System.Collections;
using UnityEngine;

public class ResetScaleObject : ScalableObject
{
    [Header("ResetScale")]
    [SerializeField] private Vector3 defaultScale;    // Escala predeterminada a la cual el objeto regresará
    [SerializeField] private float resetSpeed = 1.0f; // Velocidad de retorno a la escala predeterminada


    public override void Start()
    {
        // Al iniciar, guardamos la escala actual como escala predeterminada
        base.Start(); // Aseguramos que el Start del padre también se ejecute
    }

    private void Update()
    {
        // Si el objeto no se está escalando, vuelve gradualmente a la escala predeterminada
        if (!isScaling && transform.localScale != defaultScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, resetSpeed * Time.deltaTime);
            // Detenemos cualquier pequeña oscilación al llegar muy cerca de la escala deseada
            if (Vector3.Distance(transform.localScale, defaultScale) < 0.01f)
            {
                transform.localScale = defaultScale;
            }
        }
        // Reseteamos el estado de escalado para el próximo frame
        isScaling = false;
    }

}

