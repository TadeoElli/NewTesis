using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
public class ActivateDialogues : MonoBehaviour
{
    bool Enter = true;
    [SerializeField] string[] triggerDialogue;
    [SerializeField] string[] triggerDialogueEnglish;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Enter)
            {
                if(LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
                    DialogueSystem.Instance.StartDialogue(triggerDialogueEnglish);
                else
                    DialogueSystem.Instance.StartDialogue(triggerDialogue);
                Enter = false;
            }
        }
    }
}

