using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPartLogic : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Vector3 correctScale;
    public float scaleTolerance = 0.05f;  // Tolerancia para considerar la escala correcta

    private bool isInteractable = true;
    ScalableObject scalable;
    InteractuableObject interactuable;

    private void Awake()
    {
        scalable = GetComponent<ScalableObject>();
        interactuable = GetComponent<InteractuableObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteractable)
        {
            // Comparar la escala actual con la escala correcta
            if (IsScaleCorrect())
            {
                // Si la escala es correcta, desactivar la interacción
                isInteractable = false;
                DisableInteraction();
            }
        }
    }
    // Función para comparar la escala actual con la escala correcta
    private bool IsScaleCorrect()
    {
        // Verifica si la diferencia entre la escala actual y la correcta está dentro de la tolerancia
        return Vector3.Distance(transform.localScale, correctScale) <= scaleTolerance;
    }

    // Desactivar la capacidad de interactuar con esta parte
    private void DisableInteraction()
    {
        // Desactivar componentes que hacen que esta parte sea interactuable
        Destroy(scalable);
        interactuable.SetIsAtachableForRuler(false);
        //scalable.enabled = false;
        Debug.Log($"{gameObject.name} alcanzó la escala correcta y ya no es interactuable.");
    }
}
