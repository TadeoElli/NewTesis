using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class bossBalance : MonoBehaviour
{
    [SerializeField] private List<BossPartLogic> bossParts;
    [SerializeField] private GameObject winMenu;
    private bool isAlive = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(bossParts.All(item => !item.isInteractable) && isAlive)
        {
            isAlive = false;
            ShowWinMenu();
        }
    }

    private void ShowWinMenu()
    {
        winMenu.SetActive(true);
    }
}
