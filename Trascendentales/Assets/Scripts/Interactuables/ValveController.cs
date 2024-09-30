using UnityEngine;

public class ValveController : MonoBehaviour
{
    public Transform platform; // Referencia a la plataforma que quieres escalar
    public float maxRotation = 180f; // El ángulo máximo que puedes rotar la válvula (por ejemplo, 180 grados)
    public Vector3 closedScale = new Vector3(1, 1, 1); // Escala de la plataforma cuando la válvula está cerrada
    public Vector3 openScale = new Vector3(3, 1, 1); // Escala de la plataforma cuando la válvula está completamente abierta

    private float currentRotation = 0f; // El ángulo de rotación actual de la válvula

    void Update()
    {
        // Obtenemos la rotación actual del objeto válvula en el eje Y (o el eje que corresponda)
        currentRotation = transform.localEulerAngles.x;

        // Aseguramos que el ángulo esté dentro de 0 a maxRotation
        currentRotation = Mathf.Clamp(currentRotation, 0f, maxRotation);

        // Normalizamos el ángulo para que sea un valor entre 0 y 1
        float progress = currentRotation / maxRotation;

        // Modificamos la escala de la plataforma basándonos en el progreso
        platform.localScale = Vector3.Lerp(closedScale, openScale, progress);
    }
}

