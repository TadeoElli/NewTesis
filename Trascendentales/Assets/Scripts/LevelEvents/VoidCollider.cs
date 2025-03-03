using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidCollider : MonoBehaviour
{
    [SerializeField] Vector3 newPosition;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            //collision.gameObject.SetActive(false);
            collision.transform.position = newPosition;
        }
        if(collision.gameObject.layer == 3)
        {
            collision.gameObject.GetComponent<PlayerDamage>().Takedmg(10);
            collision.transform.position = newPosition;
        }
    }
}
