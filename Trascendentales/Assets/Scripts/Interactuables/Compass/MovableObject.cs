using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(InteractuableObject))]
[RequireComponent(typeof(Rigidbody))]
public class MovableObject : MonoBehaviour, IMovable,IFeedback
{
    [SerializeField] private float maxRadius;
    private Vector3 originalPosition;
    private Color m_feedbackCompass, m_feedbackEraser;
    [SerializeField] private bool needGravity = false;
    [SerializeField] private Renderer objRenderer;
    [SerializeField] private GameObject originParticle;
    private GameObject particleFeedback;
    private LineRenderer lineRenderer;
    private bool isMovable = true;

    public event Action OnEraserInteract;

    public float GetMaxRadius() => maxRadius;
    public bool GetNeedGravity() => needGravity;
    public bool GetIsMovable() => isMovable;

    private void Start()
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
        lineRenderer.SetPosition(1, transform.position);
    }
    public void InteractWithEraser(bool isOn2D)
    {
        OnEraserInteract?.Invoke();
    }
    public void ShowOriginFeedback()
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
    private void ResetPosition()
    {
        transform.position = originalPosition;
    }
    public void HideFeedback()
    {
        if (MouseState.Instance.CurrentToolActive() == ToolTypes.Compass &&
            MouseState.Instance.IsRightClickPress())
            return;
        Debug.Log("Sali");
        FeedbackManager.Instance.ClearFeedback(objRenderer);
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
            isMovable = false;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            isMovable = true;
    }*/

}