using UnityEngine;

public class InteractuableObject : MonoBehaviour, IInteractable, IFeedback
{
    [SerializeField] protected bool isAtacheableForCompass;
    [SerializeField] protected bool isAtacheableForRuler;
    [SerializeField] protected bool isAtacheableForSquad;
    [SerializeField] private bool isAtachedToCompass = false;
    [SerializeField] private bool isAtachedToRuler = false;
    [SerializeField] private bool isAtachedToSquad = false;
    private GameObject compassParent = null;
    public bool IsAtachableForCompass() => isAtacheableForCompass;
    public bool IsAtachableForRuler() => isAtacheableForRuler;
    public bool IsAtachableForSquad() => isAtacheableForSquad;
    public bool IsAtachedToCompass() => isAtachedToCompass;
    public bool IsAtachedToRuler() => isAtachedToRuler;
    public bool IsAtachedToSquad() => isAtachedToSquad;

    [SerializeField]private Material  m_feedbackParent;

    public void SetIsAtachedToCompass(GameObject gameObject)
    {
        isAtachedToCompass = true;
        compassParent = gameObject;
    }
    public void SetIsAtachedToRuler()
    {
        isAtachedToRuler = true;
    }
    public void SetIsAtachedToSquad()
    {
        isAtachedToSquad = true;
    }
    public void SetUnatachedToCompass()
    {
        isAtachedToCompass = false;
        compassParent = null;
        FeedbackManager.Instance.ClearFeedback(gameObject); // Limpiar el feedback al salir del objeto
    }
    public void SetUnatachedToRuler()
    {
        isAtachedToRuler = false;
        FeedbackManager.Instance.ClearFeedback(gameObject); // Limpiar el feedback al salir del objeto
    }
    public void SetUnatachedToSquad()
    {
        isAtachedToSquad = false;
        FeedbackManager.Instance.ClearFeedback(gameObject); // Limpiar el feedback al salir del objeto
    }
    public GameObject GetCompassParent()
    {
        return compassParent;
    }
    private void Update()
    {
        if(isAtachedToCompass || isAtachedToRuler || isAtachedToSquad)
            FeedbackManager.Instance.ApplyFeedback(gameObject, m_feedbackParent);
    }
    public void SetIsAtachableForRuler(bool isAtachable)
    {
        isAtacheableForRuler = isAtachable;
    }
    private void OnMouseEnter()
    {

        var x = GetComponent<Renderer>().materials[1];
        if (x != null)
        {
            //print("entre");

            //x.SetFloat("_on_off", 1);
        }

    }

    private void OnMouseExit()
    {
        var x = GetComponent<Renderer>().materials[1];
        if (x != null)
        {
            //x.SetFloat("_on_off", 0);
        }
    }

    public void ShowFeedback()
    {
        switch (MouseState.Instance.CurrentToolActive())
        {
            case ToolTypes.Brush:
                break;
            case ToolTypes.Ruler:
                if (!MouseState.Instance.IsLeftClickPress() && MouseState.Instance.IsRightClickPress() && isAtacheableForRuler)
                    FeedbackManager.Instance.ApplyFeedback(gameObject, m_feedbackParent);
                break;
            case ToolTypes.Squad:
                if (!MouseState.Instance.IsLeftClickPress() && MouseState.Instance.IsRightClickPress() && isAtacheableForSquad)
                    FeedbackManager.Instance.ApplyFeedback(gameObject, m_feedbackParent);
                break;
            case ToolTypes.Compass:
                if (MouseState.Instance.IsLeftClickPress() && !MouseState.Instance.IsRightClickPress() && isAtacheableForCompass)
                    FeedbackManager.Instance.ApplyFeedback(gameObject, m_feedbackParent);
                break;
            case ToolTypes.Eraser:
                break;
            default:
                break;
        }
    }

    public void HideFeedback()
    {
        //FeedbackManager.Instance.ClearFeedback(gameObject); // Limpiar el feedback al salir del objeto
    }
}
