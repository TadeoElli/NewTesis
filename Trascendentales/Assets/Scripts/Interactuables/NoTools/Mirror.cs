using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour, IIlluminable
{
    [SerializeField] Telescope telescope;

    public void OnLightOff()
    {
        telescope.SetCharged(false);
        //Debug.Log("LightOff");
    }

    public void OnLightOn()
    {
        telescope.SetCharged(true);
        //Debug.Log("LightOn");
    }
    
}
