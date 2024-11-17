using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class TutorialFailCheck : MonoBehaviour
{
    [SerializeField] private Vector3 restartPosition;
    [SerializeField] string[] triggerDialogue;
    [SerializeField] string[] triggerDialogueEnglish;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.position = restartPosition;
            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
                DialogueSystem.Instance.StartDialogue(triggerDialogueEnglish);
            else
                DialogueSystem.Instance.StartDialogue(triggerDialogue);
        }
    }
}
