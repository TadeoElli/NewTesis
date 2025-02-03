using UnityEngine;

[System.Serializable]
public class CoinData
{
    public Vector3 position; // Posición de la moneda
    public bool collected; // Estado de la moneda (recolectada o no)
    public string levelName; // Nombre del nivel donde debe aparecer

    public CoinData(Vector3 position, string levelName)
    {
        this.position = position;
        this.levelName = levelName;
        this.collected = false;
    }
}
