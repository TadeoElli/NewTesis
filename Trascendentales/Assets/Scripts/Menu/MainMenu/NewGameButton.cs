using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGameButton : MonoBehaviour
{
    [SerializeField] private Button loadButton;
    public void StartNewGame()
    {
        SaveSystem.DeletePlayerData();
        CoinManager.Instance.ResetCoins();
    }
    private void Awake()
    {
        if(SaveSystem.ExistData())
            loadButton.interactable = true;
        else
            loadButton.interactable = false;
    }
}
