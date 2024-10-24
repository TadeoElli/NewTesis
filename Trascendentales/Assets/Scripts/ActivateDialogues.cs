using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDialogues : MonoBehaviour
{
    [SerializeField] GameObject myTutorialO;
    bool Enter = true;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Enter)
            {
                myTutorialO.SetActive(true);

                myTutorialO.GetComponent<DialogueSysterm>().Start_Dialogues();
                Enter = false;
            }
        }
    }
}

