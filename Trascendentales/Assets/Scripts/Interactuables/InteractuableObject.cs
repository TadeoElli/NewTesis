using UnityEngine;

public class InteractuableObject : MonoBehaviour, IInteractable
{
    [SerializeField] protected bool isAtacheableForCompass;
    [SerializeField] protected bool isAtacheableForRuler;
    [SerializeField] private bool isAtachedToCompass = false;
    [SerializeField] private bool isAtachedToRuler = false;
    private GameObject compassParent = null;
    private GameObject rulerParent = null;
    public bool IsAtachableForCompass() => isAtacheableForCompass;
    public bool IsAtachableForRuler() => isAtacheableForRuler;
    public bool IsAtachedToCompass() => isAtachedToCompass;
    public bool IsAtachedToRuler() => isAtachedToRuler;

    public void SetIsAtachedToCompass(GameObject gameObject)
    {
        isAtachedToCompass = true;
        compassParent = gameObject;
    }
    public void SetUnatachedToCompass()
    {
        isAtachedToCompass = false;
        compassParent = null;
    }
    public GameObject GetCompassParent()
    {
        return compassParent;
    }

    public void SetIsAtachedToRuler(GameObject gameObject)
    {
        isAtachedToRuler = true;
        rulerParent = gameObject;
    }

    public void SetUnatachedToRuler()
    {
        isAtachedToRuler = false;
        rulerParent = null;
    }

    public GameObject GetRulerParent()
    {
        return rulerParent;
    }
}
