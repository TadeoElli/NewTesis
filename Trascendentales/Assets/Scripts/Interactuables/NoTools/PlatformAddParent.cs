using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAddParent : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Hacemos al player hijo del objeto
            collision.gameObject.transform.parent = transform;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Hacemos al player hijo del objeto
            collision.gameObject.transform.parent = transform;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Desasociamos al player del objeto
            collision.gameObject.transform.parent = null;
        }
    }
}
