using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerOptional : MonoBehaviour
{

    // Definir una clase Sound para almacenar cada sonido y su nombre
    [Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 0.7f;
        [Range(0.1f, 3f)] public float pitch = 1f;

        [HideInInspector]
        public AudioSource source;
    }

    // Singleton estático
    public static AudioManagerOptional instance;

    public Sound[] sounds; // Array de sonidos

    void Awake()
    {
        // Singleton: Asegurarse de que solo haya un AudioManager
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Mantener el AudioManager entre escenas

        // Crear un AudioSource para cada sonido
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.playOnAwake = false;
        }
    }

    // Método público para reproducir un sonido por nombre
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " No Existe");
            return;
        }
        s.source.Play();
    }

    // Método opcional para detener un sonido por nombre
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " No Existe");
            return;
        }
        s.source.Stop();
    }
}

