using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAddParent : MonoBehaviour
{
    private Vector3 originalScale;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Guardamos la escala original del player
            originalScale = collision.gameObject.transform.localScale;

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

            // Restauramos la escala original
            collision.gameObject.transform.localScale = originalScale;
        }

    }
}
