using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class DialogueSysterm : MonoBehaviour
{
    [SerializeField] string[] _dialogue;

    [SerializeField] TextMeshProUGUI _text;

    public int index;

    public float txtSpeed;

    public void CheckDialogue()
    {
        if (_text.text == _dialogue[index])
        {
            Nextline();
        }
        else 
        {
            StopAllCoroutines();
            _text.text = _dialogue[index];
        }
    }    

    public void Start_Dialogues()
    {
        _text.text = string.Empty;
        index = 0;
        StartCoroutine(WriteLine());
    }

    IEnumerator WriteLine()
    {
        foreach (char dialogue in _dialogue[index].ToCharArray())
        {
            _text.text += dialogue;

            yield return new WaitForSeconds(txtSpeed);
        }
    }

    public void Nextline()
    {
        if (index < _dialogue.Length - 1)
        {
            index++;
            _text.text = string.Empty;
            StartCoroutine(WriteLine());
        }
        else 
        {
        gameObject.SetActive(false);
        }
    }
}
