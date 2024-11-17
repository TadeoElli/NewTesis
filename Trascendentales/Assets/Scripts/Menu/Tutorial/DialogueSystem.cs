using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; private set; } // Singleton
    [SerializeField] string[] firstDialogue;
    [SerializeField] string[] firstDialogueEnglish;

    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] AudioClip writeClip;
    [SerializeField] GameObject tutorialHud;

    private string[] currentDialogue;
    private int index;
    private InputManager input;
    public float txtSpeed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        input = FindObjectOfType<InputManager>();
        input.OnInteract += CheckDialogue;
        tutorialHud.SetActive(false); // Mantener oculto al inicio
    }
    private void Start()
    {
        if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[0])
            StartDialogue(firstDialogueEnglish);
        else
            StartDialogue(firstDialogue);
        StartDialogue(firstDialogue);
    }

    // Comprobar y avanzar en el diálogo
    private void CheckDialogue()
    {
        if (_text.text == currentDialogue[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            _text.text = currentDialogue[index];
        }
    }

    // Iniciar diálogo con el texto específico
    public void StartDialogue(string[] dialogue)
    {
        StopAllCoroutines();
        currentDialogue = dialogue;
        _text.text = string.Empty;
        index = 0;
        tutorialHud.SetActive(true); // Activar HUD de tutorial
        StartCoroutine(WriteLine());
    }

    // Escribir línea actual de diálogo
    private IEnumerator WriteLine()
    {
        int cantOfChars = 0;
        foreach (char dialogueChar in currentDialogue[index].ToCharArray())
        {
            _text.text += dialogueChar;
            cantOfChars++;
            if(cantOfChars >= 3)
            {
                AudioManager.Instance.PlaySoundEffect(writeClip);
                cantOfChars = 0;
            }
            yield return new WaitForSeconds(txtSpeed);
        }
    }

    // Avanzar a la siguiente línea o cerrar el HUD
    private void NextLine()
    {
        if (index < currentDialogue.Length - 1)
        {
            index++;
            _text.text = string.Empty;
            StartCoroutine(WriteLine());
        }
        else
        {
            tutorialHud.SetActive(false); // Ocultar HUD al finalizar el diálogo
        }
    }
}
