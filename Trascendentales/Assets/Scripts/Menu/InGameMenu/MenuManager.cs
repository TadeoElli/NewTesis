using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu, winMenu, loseMenu;
    [SerializeField] private Animator toolWheel, drawObjectWheel, dragObjectWheel, eraserWheel;
    private Animator currentAlternativeWheel;
    private bool isGamePaused = false;
    private bool isCompleted = false;

    public static MenuManager Instance { get; private set; }
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
    }
    #region Menus
    // Función para pausar el juego
    private void PauseGame()
    {
        if (isCompleted) return; // Si ya está pausado, no hacemos nada

        Time.timeScale = 0; // Pausar el tiempo
        isGamePaused = true;
        pauseMenu.SetActive(true);
    }
    // Función para continuar el juego
    private void ResumeGame()
    {
        if (isCompleted) return; // Si no está pausado, no hacemos nada

        Time.timeScale = 1; // Restaurar el tiempo
        isGamePaused = false;
        pauseMenu.SetActive(false);
    }
    public void TogglePause()
    {
        if (!canTogglePause) return;

        if (isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }

        StartCoroutine(DebounceTogglePause()); // Aplicar debounce
    }


    public void WinLevel()
    {
        if (isCompleted) return;
        isCompleted = true;
        winMenu.SetActive(true);
    }
    public void LoseLevel()
    {
        if (isCompleted) return;
        isCompleted = true;
        loseMenu.SetActive(true);
    }
    #endregion
    #region ToolsWheel
    public void ShowToolWheel()
    {
        toolWheel.SetBool("OpenToolWheel", true);
    }
    public void HideToolWheel()
    {
        toolWheel.SetBool("OpenToolWheel", false);
    }
    public void ShowDrawObjectWheel()
    {
        drawObjectWheel.SetBool("OpenToolWheel", true);
        currentAlternativeWheel = drawObjectWheel;
    }
    public void ShowDragObjectWheel()
    {
        dragObjectWheel.SetBool("OpenToolWheel", true);
        currentAlternativeWheel = dragObjectWheel;
    }
    public void ShowEraserWheel()
    {
        eraserWheel.SetBool("OpenToolWheel", true);
        currentAlternativeWheel = eraserWheel;
    }
    public void HideAlternativeWheel()
    {
        if(!currentAlternativeWheel) return;
        currentAlternativeWheel.SetBool("OpenToolWheel", false);
        currentAlternativeWheel = null;
    }
    #endregion
    private bool canTogglePause = true; // Bandera para evitar múltiples activaciones
    [SerializeField] private float toggleDelay = 0.2f; // Retraso mínimo entre toggles

    private IEnumerator DebounceTogglePause()
    {
        canTogglePause = false;
        yield return new WaitForSecondsRealtime(toggleDelay); // Espera en tiempo real (no afectado por Time.timeScale)
        canTogglePause = true;
    }

}
