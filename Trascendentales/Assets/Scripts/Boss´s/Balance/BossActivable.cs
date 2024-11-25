using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossActivable : MonoBehaviour, IBossActivable
{
    public UnityEvent OnActivate, OnDesactivate;
    public void Activate()
    {
        OnActivate?.Invoke();
        Debug.Log("entro");
    }
    public void Deactivate()
    {
        Debug.Log("entro");
        OnDesactivate?.Invoke();
    }
}
public interface IBossActivable
{
    public void Activate();
    public void Deactivate();
}
