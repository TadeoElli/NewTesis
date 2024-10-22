using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool hasActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasActivated)
        {
            other.GetComponent<PlayerManager>().SavePlayer();
            hasActivated = true;
            Debug.Log("Checkpoint Reached and Activated");
        }
    }
}
