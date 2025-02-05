using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreventFallObject : MonoBehaviour
{
    private Vector3 originPosition;
    private Quaternion originRotation;
    private Vector3 originScale;
    private void OnEnable()
    {
        originPosition = transform.position;
        originRotation = transform.rotation;
        originScale = transform.localScale;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Void"))
        {
            transform.position = originPosition;
            transform.rotation = originRotation;
            transform.localScale = originScale;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Void"))
        {
            transform.position = originPosition;
            transform.rotation = originRotation;
            transform.localScale = originScale;
        }
    }
}
