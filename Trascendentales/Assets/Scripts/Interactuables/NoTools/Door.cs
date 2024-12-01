using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;
    [SerializeField] int cantOfAccess = 1;
    [SerializeField] private bool openOnstart = false;
    [SerializeField] private AudioClip doorOpenSound, doorCloseSound;
    private int currentAccess = 0;
    private bool isOpen = false;
    private AudioSource doorSource;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        doorSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        doorSource.volume = AudioManager.Instance.GetEffectsVolume();
        if (openOnstart)
            TryToAccess();
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
            doorSource.PlayOneShot(doorOpenSound);
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
            doorSource.PlayOneShot(doorCloseSound);
        }
    }
}
