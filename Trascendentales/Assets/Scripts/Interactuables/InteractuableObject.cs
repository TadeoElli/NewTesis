﻿using UnityEngine;

public class InteractuableObject : MonoBehaviour, IInteractable
{
    [SerializeField] protected bool isAtacheableForCompass;
    [SerializeField] protected bool isAtacheableForRuler;
    [SerializeField] protected bool isAtacheableForSquad;
    [SerializeField] private bool isAtachedToCompass = false;
    private GameObject compassParent = null;
    public bool IsAtachableForCompass() => isAtacheableForCompass;
    public bool IsAtachableForRuler() => isAtacheableForRuler;
    public bool IsAtachableForSquad() => isAtacheableForSquad;
    public bool IsAtachedToCompass() => isAtachedToCompass;

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

            x.SetFloat("_on_off", 1);
        }

    }

    private void OnMouseExit()
    {
        var x = GetComponent<Renderer>().materials[1];
        if (x != null)
        {
            x.SetFloat("_on_off", 0);
        }
    }
}
