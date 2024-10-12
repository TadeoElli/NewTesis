using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Aplicar feedback según el tipo de interacción
    public void ApplyFeedback(GameObject obj, Material feedbackMaterial)
    {
        var renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            var materials = renderer.materials;
            materials[1] = feedbackMaterial; // Aplicar material en el índice correcto
            renderer.materials = materials;
        }
    }

    // Limpiar el feedback (quitar material)
    public void ClearFeedback(GameObject obj)
    {
        var renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            var materials = renderer.materials;
            materials[1] = null; // Eliminar el material de feedback
            renderer.materials = materials;
        }
    }

    // Métodos para activar/desactivar LineRenderer entre dos objetos
    public void ActivateLineRenderer(LineRenderer line, GameObject parent, GameObject child)
    {
        line.SetPosition(0, parent.transform.position);
        line.SetPosition(1, child.transform.position);
        line.enabled = true;
    }

    public void DeactivateLineRenderer(LineRenderer line)
    {
        line.enabled = false;
    }
}
