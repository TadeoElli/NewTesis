using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDamage : MonoBehaviour, IDamagable
{
    private bool isLive = true;
    PlayerManager playerManager;
    [SerializeField] GameObject deadMenu;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }
    public void Takedmg(int dmg)
    {
        //StartCoroutine(dmgVisual());
        if(isLive)
        {
            CallToDeath();
            StartCoroutine(ShowDeadMenu());
            isLive = false;
        }
    }
    private IEnumerator ShowDeadMenu()
    {
        yield return new WaitForSeconds(1.5f);
        deadMenu.SetActive(true);
    }

    private void CallToDeath()
    {
        playerManager.Death();
    }

    /*IEnumerator dmgVisual()
    {
        this.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        this.GetComponent<Renderer>().material.color = Color.yellow;

        yield return null;
    }*/
}
