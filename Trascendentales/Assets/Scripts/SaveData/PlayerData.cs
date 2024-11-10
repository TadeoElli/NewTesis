using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float positionX;
    public float positionY;
    public float positionZ;

    public PlayerData(Vector3 position)
    {
        positionX = position.x;
        positionY = position.y;
        positionZ = position.z;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(positionX, positionY, positionZ);
    }
}
