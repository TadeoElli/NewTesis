using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    [SerializeField] int cantOfAccess = 1;
    private int currentAccess = 0;
    private bool isOpen = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void TryToAccess()
    {
        if (isOpen)
            return;
        currentAccess++;
        if (currentAccess >= cantOfAccess)
        {
            animator.SetTrigger("Open");
            gameObject.GetComponent<Collider>().enabled = false;
            isOpen = true;
        }
    }
    public void TryToClose()
    {
        if (!isOpen)
            return;
        currentAccess--;
        if (currentAccess <= 0)
        {
            animator.SetTrigger("Close");
            gameObject.GetComponent<Collider>().enabled = true;
            isOpen = false;
        }
    }
}
