using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(InteractuableObject))]
public class ScalableObject : MonoBehaviour, IEscalable, IFeedback
{
    [SerializeField] private Vector3 minScale;
    [SerializeField] private Vector3 maxScale;
    [SerializeField] private float maxRadius;
    [SerializeField] private bool canAttachOthers;
    [SerializeField] public bool canScale = true;
    [SerializeField] public bool isScaling = false;
    private Color m_feedback;
    private Vector3 originalScale;
    [SerializeField] private Renderer objRenderer;

    public event Action OnEraserInteract;

    public Vector3 GetMinScale() => minScale;
    public Vector3 GetMaxScale() => maxScale;
    public bool CanAttachOthers() => canAttachOthers;
    public bool CanScale() => canScale;
    public bool IsScaling() => isScaling;
    public float GetMaxRadius() => maxRadius;

    public void SetScaleConstraints(Vector3 minScale, Vector3 maxScale)
    {
        this.minScale = minScale;
        this.maxScale = maxScale;
    }
    public void SetIsScaling(bool state) {
        isScaling = state;
    }

    public virtual void Start()
    {
        m_feedback = ColorDictionary.GetColor("FeedbackScale");
        originalScale = transform.localScale;
        OnEraserInteract += ResetScale;
    }
    public void InteractWithEraser(bool isOn2D)
    {
        OnEraserInteract?.Invoke();
    }


    public void ShowFeedback()
    {
        if (MouseState.Instance.CurrentToolActive() == ToolTypes.Ruler &&
            !MouseState.Instance.IsLeftClickPress() &&
            !MouseState.Instance.IsRightClickPress() &&
            canScale )
        {
            // Aplicar feedback si la herramienta activa es la regla y no hay clicks activos
            FeedbackManager.Instance.ApplyFeedback(objRenderer, m_feedback);
        }

    }
    private void ResetScale()
    {
        transform.localScale = originalScale;
    }
    public void HideFeedback()
    {
        FeedbackManager.Instance.ClearFeedback(objRenderer); // Limpiar el feedback al salir del objeto
    }
}
