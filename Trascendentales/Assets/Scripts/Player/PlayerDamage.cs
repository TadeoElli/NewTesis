using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour, IDamagable
{
    public void Takedmg(int dmg)
    {
        StartCoroutine(dmgVisual());
    }

    IEnumerator dmgVisual()
    {
        this.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        this.GetComponent<Renderer>().material.color = Color.yellow;

        yield return null;
    }
}
