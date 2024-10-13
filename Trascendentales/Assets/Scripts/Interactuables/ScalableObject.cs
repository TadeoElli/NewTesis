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
    [SerializeField] private Material m_feedback;
    [SerializeField] private Renderer objRenderer;

    public event Action OnEraserInteract;

    public Vector3 GetMinScale() => minScale;
    public Vector3 GetMaxScale() => maxScale;
    public bool CanAttachOthers() => canAttachOthers;
    public bool CanScale() => canScale;
    public float GetMaxRadius() => maxRadius;

    public void SetScaleConstraints(Vector3 minScale, Vector3 maxScale)
    {
        this.minScale = minScale;
        this.maxScale = maxScale;
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

    public void HideFeedback()
    {
        FeedbackManager.Instance.ClearFeedback(objRenderer); // Limpiar el feedback al salir del objeto
    }
}
