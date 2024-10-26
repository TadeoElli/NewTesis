using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float velocity = 10f;

    // Booleanos para seleccionar el eje de rotaci�n
    public bool x = false;
    public bool y = true;
    public bool z = false;

    void Update()
    {
        // Define el eje de rotaci�n en funci�n de los booleanos
        Vector3 rotationAxis = new Vector3(
            x ? 1 : 0,  // Si `x` es verdadero, usa el eje X
            y ? 1 : 0,  // Si `y` es verdadero, usa el eje Y
            z ? 1 : 0   // Si `z` es verdadero, usa el eje Z
        );

        // Aplica la rotaci�n solo si alg�n eje est� seleccionado
        if (rotationAxis != Vector3.zero)
        {
            transform.Rotate(rotationAxis * velocity * Time.deltaTime);
        }
    }
}
