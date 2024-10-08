using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_Script : MonoBehaviour
{
    public ParticleSystem particleMouse;

    public void onParticlesMouse()
    {
        if (MouseState.Instance.IsLeftClickPress())
        {
            particleMouse.Play();
        }
    }
}
