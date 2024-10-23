using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(PlayerManager player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.data";

        // Añade una comprobación de seguridad antes de sobrescribir
        if (ExistData())
        {
            Debug.Log("Overwriting save data...");
        }

        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(player);
        formatter.Serialize(stream, data);
        stream.Close();
    }


    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save data file not found in " + path);
            return null;
        }
    }
    // Borrar datos del jugador
    public static void DeleteSaveData()
    {
        string path = Application.persistentDataPath + "/player.data";
        if (ExistData())
        {
            File.Delete(path); // Borra el archivo de guardado
            Debug.Log("Save data deleted.");
        }
        else
        {
            Debug.Log("No save data to delete.");
        }
    }

    public static bool ExistData()
    {
        string path = Application.persistentDataPath + "/player.data";
        if (File.Exists(path))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
