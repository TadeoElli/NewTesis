using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossActivable : MonoBehaviour, IBossActivable
{
    public UnityEvent OnActivate, OnDesactivate;
    [SerializeField] private Animator animator;
    public void Activate()
    {
        OnActivate?.Invoke();
        Debug.Log("entro");
        animator.SetBool("isPressed", true);
    }
    public void Deactivate()
    {
        Debug.Log("entro");
        OnDesactivate?.Invoke();
        animator.SetBool("isPressed", false);
    }
}
public interface IBossActivable
{
    public void Activate();
    public void Deactivate();
}
