using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/playerData.json";

    // Guardar datos en JSON
    public static void SavePlayerData(Vector3 position)
    {
        PlayerData data = new PlayerData(position);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log("Posición guardada en: " + path);
    }

    // Cargar datos desde JSON
    public static Vector3 LoadPlayerData()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            return data.GetPosition();
        }
        else
        {
            Debug.LogWarning("Archivo de guardado no encontrado. Usando posición inicial.");
            return Vector3.zero; // Posición inicial o posición predeterminada
        }
    }

    // Borrar datos del jugador
    public static void DeletePlayerData()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Datos de guardado borrados.");
        }
        else
        {
            Debug.LogWarning("No se encontraron datos para borrar.");
        }
    }

    public static bool ExistData()
    {
        return File.Exists(path);
    }
}

