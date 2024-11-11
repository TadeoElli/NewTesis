using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            collision.gameObject.SetActive(false);
        }
        if(collision.gameObject.layer == 3)
        {
            collision.gameObject.GetComponent<PlayerDamage>().Takedmg(10);
        }
    }
}
