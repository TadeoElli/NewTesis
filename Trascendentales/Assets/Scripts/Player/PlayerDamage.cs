using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerDamage : MonoBehaviour, IDamagable
{
    private bool isLive = true;
    [SerializeField] private int lives = 3;
    PlayerManager playerManager;
    public UnityEvent OnTakeDamage;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }
    public void Takedmg(int dmg)
    {
        lives--;
        if(lives <= 0 && isLive)
        {
            CallToDeath();
            StartCoroutine(ShowDeadMenu());
            isLive = false;
            return;
        }
        StartCoroutine(dmgVisual());

    }
    private IEnumerator ShowDeadMenu()
    {
        yield return new WaitForSeconds(1.5f);
        MenuManager.Instance.LoseLevel();
    }

    private void CallToDeath()
    {
        playerManager.Death();
    }

    IEnumerator dmgVisual()
    {
        this.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        this.GetComponent<Renderer>().material.color = Color.yellow;

        yield return null;
    }
}
