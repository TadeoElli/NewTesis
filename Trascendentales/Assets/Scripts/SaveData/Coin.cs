using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.Instance.CollectCoin(transform.position);
            Destroy(gameObject); // Destruir la moneda despu�s de ser recolectada
        }
    }
}

