using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFailCheck : MonoBehaviour
{
    [SerializeField] private Vector3 restartPosition;
    [SerializeField] string[] triggerDialogue;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.position = restartPosition;
            DialogueSystem.Instance.StartDialogue(triggerDialogue);
        }
    }
}
