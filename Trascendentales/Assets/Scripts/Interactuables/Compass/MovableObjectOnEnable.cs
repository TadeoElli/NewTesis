using UnityEngine;

public class MovableObjectOnEnable : MovableObject
{
    protected override void Start()
    {
        m_feedbackCompass = ColorDictionary.GetColor("FeedbackCompass");
        m_feedbackEraser = ColorDictionary.GetColor("FeedbackEraser");
        OnEraserInteract += ResetPosition;
        OnEraserInteract += HideOriginFeedback;
    }

    public override void ShowOriginFeedback()
    {
        if(particleFeedback.activeSelf)
            return;
        originalPosition = transform.position;
        particleFeedback.SetActive(true);
        particleFeedback.transform.position = originalPosition;
        lineRenderer.SetPosition(0, particleFeedback.transform.position);
    }
    private void OnEnable()
    {
        originalPosition = transform.position;
        particleFeedback = Instantiate(originParticle, transform.position, Quaternion.identity);
        lineRenderer = particleFeedback.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, particleFeedback.transform.position);
        particleFeedback.SetActive(false);
    }
    private void OnDisable()
    {
        Destroy(particleFeedback);
    }
}
