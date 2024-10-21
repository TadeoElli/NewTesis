using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] lastPosition;

    public PlayerData (PlayerManager player)
    {
        lastPosition = new float[3];
        lastPosition[0] = player.transform.position.x;
        lastPosition[1] = player.transform.position.y;
        lastPosition[2] = player.transform.position.z;

    }
}
