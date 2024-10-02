using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance_Damage : MonoBehaviour
{
    public int actualDmg;

    private void OnTriggerEnter(Collider other)
    {
        var x = other.GetComponent<IDamagable>();
        if (x != null )
        {
            x.Takedmg(actualDmg);
        }
    }
}
