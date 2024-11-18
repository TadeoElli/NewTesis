using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Animations;
[RequireComponent(typeof(InteractuableObject))]
[RequireComponent(typeof(Rigidbody))]
public class MovableObject : MonoBehaviour, IMovable,IFeedback
{
    [SerializeField] private float maxRadius;
    protected Vector3 originalPosition;
    protected Color m_feedbackCompass, m_feedbackEraser;
    [SerializeField] private bool needGravity = false;
    [SerializeField] private Renderer objRenderer;
    [SerializeField] protected GameObject originParticle;
    protected GameObject particleFeedback;
    protected LineRenderer lineRenderer;
    private bool isMovable = true;

    public event Action OnEraserInteract;

    public float GetMaxRadius() => maxRadius;
    public bool GetNeedGravity() => needGravity;
    public bool GetIsMovable() => isMovable;


    protected virtual void Start()
    {
        m_feedbackCompass = ColorDictionary.GetColor("FeedbackCompass");
        m_feedbackEraser = ColorDictionary.GetColor("FeedbackEraser");
        originalPosition = transform.position;
        OnEraserInteract += ResetPosition;
        OnEraserInteract += HideOriginFeedback;
        particleFeedback = Instantiate(originParticle, transform.position, Quaternion.identity);
        particleFeedback.SetActive(false);
        lineRenderer = particleFeedback.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, particleFeedback.transform.position);
    }
    public Vector3 GetOriginalPosition() => originalPosition;

    private void Update()
    {
        if(!particleFeedback.activeSelf)
            return;
        lineRenderer.SetPosition(1, transform.position);
        // Verifica la distancia desde la posición original
        float distance = Vector3.Distance(transform.position, originalPosition);
        if (distance > maxRadius)
        {
            // Calcula la posición máxima permitida dentro del radio
            Vector3 direction = (transform.position - originalPosition).normalized;
            transform.position = originalPosition + direction * maxRadius;
        }
    }
    public void InteractWithEraser(bool isOn2D)
    {
        OnEraserInteract?.Invoke();
    }
    public virtual void ShowOriginFeedback()
    {
        particleFeedback.SetActive(true);
    }
    public void HideOriginFeedback()
    {
        particleFeedback.SetActive(false);
    }
    public void ShowFeedback()
    {
        if (MouseState.Instance.CurrentToolActive() == ToolTypes.Compass &&
            !MouseState.Instance.IsLeftClickPress())
            FeedbackManager.Instance.ApplyFeedback(objRenderer, m_feedbackCompass);
        else if (MouseState.Instance.CurrentToolActive() == ToolTypes.Eraser &&
            !MouseState.Instance.IsLeftClickPress() &&
            !MouseState.Instance.IsRightClickPress())
            FeedbackManager.Instance.ApplyFeedback(objRenderer, m_feedbackEraser);


    }
    protected void ResetPosition()
    {
        transform.position = originalPosition;
    }
    public void HideFeedback()
    {
        if (MouseState.Instance.CurrentToolActive() == ToolTypes.Compass &&
            MouseState.Instance.IsRightClickPress())
            return;
        FeedbackManager.Instance.ClearFeedback(objRenderer);
    }


}
