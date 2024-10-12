using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnPauseGame += ShowMenu;
        GameManager.Instance.OnResumeGame += HideMenu;
        this.gameObject.SetActive(false);
    }

    public void ShowMenu()
    {
        this.gameObject.SetActive(true);
    }
    public void HideMenu()
    {
        this.gameObject.SetActive(false);
    }
    public void QuitLevel()
    {
        GameManager.Instance.OnPauseGame -= ShowMenu;
        GameManager.Instance.OnResumeGame -= HideMenu;
    }
}
