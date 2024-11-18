using UnityEngine;

public class Telescope : MonoBehaviour
{
    [Header("Telescopio Configuración")]
    [SerializeField] private Transform rayTarget; // Punto hacia el cual se dispara el raycast
    [SerializeField] private float maxRayLength = 10f; // Longitud máxima del raycast
    [SerializeField] private LineRenderer lineRenderer; // Línea para el haz de luz

    private bool isActive = false; // Flag para determinar si está activado

    private void Update()
    {
        if (!isActive)
        {
            // Desactiva el LineRenderer si no está activo
            lineRenderer.enabled = false;
            return;
        }

        RaycastHit hit;
        Vector3 direction = (rayTarget.position - transform.position).normalized;
        //float rayDistance = Mathf.Min(maxRayLength, Vector3.Distance(transform.position, rayTarget.position));

        // Realiza el Raycast
        if (Physics.Raycast(transform.position, direction, out hit, maxRayLength))
        {
            // Verifica si el objeto impactado implementa la interfaz IIluminable
            var illuminable = hit.collider.GetComponent<IIlluminable>();
            if (illuminable != null)
            {
                illuminable.OnLightOn(); // Llama la función para activar
            }

            // Actualiza el LineRenderer para llegar hasta el punto de impacto
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, rayTarget.position);
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            // Si no hay impacto, el LineRenderer llega al máximo alcance
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, rayTarget.position);
            lineRenderer.SetPosition(1, rayTarget.position + direction * maxRayLength);
        }
    }
    public void SetCharged(bool state)
    {
        isActive = state;
    }

}

