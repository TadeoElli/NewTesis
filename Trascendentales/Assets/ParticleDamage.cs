using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.TryGetComponent<IDamagable>(out IDamagable x))
        {
            x.Takedmg(1);
            Debug.Log("particle damage");
        }
    }
}
