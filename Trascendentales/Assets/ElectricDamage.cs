using UnityEngine;

public class ElectricDamage : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable x))
        {
            x.Takedmg(1);
            Debug.Log("particle damage");
        }
    }
}
