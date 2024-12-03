using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    [SerializeField] private AudioClip fireballSound;
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.TryGetComponent<IDamagable>(out IDamagable x))
        {
            x.Takedmg(1);
            Debug.Log("particle damage");
        }
    }
    private void OnEnable()
    {
        StartCoroutine(ReproduceSound());
    }

    private IEnumerator ReproduceSound()
    {
        while (true)
        {
            AudioManager.Instance.PlaySoundEffect(fireballSound);
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
