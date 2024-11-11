using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAddParent : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Hacemos al player hijo del objeto
            collision.gameObject.transform.parent = transform;
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Hacemos al player hijo del objeto
            collision.gameObject.transform.parent = transform;
        }
    }
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Desasociamos al player del objeto
            collision.gameObject.transform.parent = null;
        }
    }
}
