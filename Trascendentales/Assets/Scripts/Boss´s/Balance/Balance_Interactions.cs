using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance_Interactions : MonoBehaviour
{
    public int actualDmg;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<IBossActivable>(out IBossActivable y))
        {
            Debug.Log("Entre");
            y.Activate();
        }
        if (other.gameObject.TryGetComponent<IDamagable>(out IDamagable x))
        {
            x.Takedmg(actualDmg);
            Debug.Log("Entre");
        }
        Debug.Log(other.gameObject);

    }
}
