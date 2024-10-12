using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    private bool isGamePaused = false;
    public event Action OnPauseGame;
    public event Action OnResumeGame;


    private void Awake() {
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }

    // Función para pausar el juego
    public void PauseGame()
    {
        if (isGamePaused) return; // Si ya está pausado, no hacemos nada

        Time.timeScale = 0; // Pausar el tiempo
        isGamePaused = true;
        OnPauseGame?.Invoke();

        // Opcional: Aquí puedes añadir lógica para mostrar un menú de pausa
        // o detener otros sistemas específicos del juego
        Debug.Log("Juego pausado");
    }

    // Función para continuar el juego
    public void ResumeGame()
    {
        if (!isGamePaused) return; // Si no está pausado, no hacemos nada

        Time.timeScale = 1; // Restaurar el tiempo
        isGamePaused = false;
        OnResumeGame.Invoke();

        // Opcional: Aquí puedes ocultar el menú de pausa
        Debug.Log("Juego reanudado");
    }
    public void ResetEvents()
    {
        OnPauseGame = null;
        OnResumeGame = null;
        ResumeGame();
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
    public void QuitGame(){
        Application.Quit();
    }
}
