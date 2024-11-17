using UnityEngine;

public class ResetRotationObject : RotableObject
{
    [Header("ResetRotation")]
    [SerializeField] private Quaternion defaultRotation; // Rotación predeterminada a la cual el objeto regresará
    [SerializeField] private float resetSpeed = 1.0f;    // Velocidad de retorno a la rotación predeterminada

    private void Update()
    {
        // Si el objeto no se está rotando, vuelve gradualmente a la rotación predeterminada
        if (!isRotating && transform.rotation != defaultRotation)
        {
            Debug.Log("Rotating");
            transform.rotation = Quaternion.Lerp(transform.rotation, defaultRotation, resetSpeed * Time.deltaTime);
            // Detenemos cualquier pequeña oscilación al llegar muy cerca de la rotación deseada
            if (Quaternion.Angle(transform.rotation, defaultRotation) < 0.1f)
            {
                transform.rotation = defaultRotation;
            }
        }
    }

}

