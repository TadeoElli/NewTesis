using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool hasActivated = false;
    [SerializeField] private GameObject savingFeedback;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasActivated)
        {
            SaveSystem.SavePlayerData(other.transform.position);
            hasActivated = true;
            savingFeedback.SetActive(true);
            StartCoroutine(HideFeedback());
            Debug.Log("Checkpoint Reached and Activated");
        }
    }

    private IEnumerator HideFeedback()
    {
        yield return new WaitForSeconds(1.5f);
        savingFeedback.SetActive(false);
    }
}

