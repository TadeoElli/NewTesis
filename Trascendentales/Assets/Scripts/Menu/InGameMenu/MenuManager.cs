using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu, winMenu, loseMenu;
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
    // Función para alternar entre pausar y continuar el juego
    public void TogglePause()
    {
        if (isGamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
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

}
