using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDialogues : MonoBehaviour
{
    bool Enter = true;
    [SerializeField] string[] triggerDialogue;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Enter)
            {
                DialogueSystem.Instance.StartDialogue(triggerDialogue);
                Enter = false;
            }
        }
    }
}

