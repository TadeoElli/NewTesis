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
    [SerializeField] private GameObject shieldBubble;
    private bool isInvencible = false;

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }
    public void Takedmg(int dmg)
    {
        if(isInvencible)
            return;
        lives--;
        OnTakeDamage?.Invoke();
        if (lives <= 0 && isLive)
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
        isInvencible = true;
        shieldBubble.SetActive(true);
        yield return new WaitForSeconds(5f);
        isInvencible = false;
        shieldBubble.SetActive(false);
        yield return null;
    }
}
