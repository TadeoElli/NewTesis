using UnityEngine;

public class ElectricDamage : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        audioSource.volume = AudioManager.Instance.GetEffectsVolume();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable x))
        {
            x.Takedmg(1);
            Debug.Log("particle damage");
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<IDamagable>(out IDamagable x))
        {
            x.Takedmg(1);
            Debug.Log("particle damage");
        }
    }
}
