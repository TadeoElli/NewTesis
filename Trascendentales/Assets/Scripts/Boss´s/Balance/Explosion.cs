using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private AudioClip explosionClip;
    public void PlaySound()
    {
        AudioManager.Instance.PlaySoundEffect(explosionClip);
    }
}
