using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Takes SaveData object and saves the variables into a binary formatted file.
/// Has a save and load data function for GameManager to execute.
/// </summary>
/// <see cref="SaveData"/> on what variables from the game are saved.
/// <seealso cref="GameManager"/> on how save and load data functions are executed.

public static class SaveSystem
{
    /// <summary>
    /// Method to serialize variables to be written into the formatted binary file.
    /// Creates a new file named "save.asteroid" as the save file.
    /// </summary>
    /// <param name="gameManager">To be used to construct SaveData object.</param>
    public static void SaveProgress (GameManager gameManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "save.asteroid";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(gameManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    /// <summary>
    /// Loads up a previously saved game progress and constructs a new SaveData object to store values into.
    /// </summary>
    /// <returns>A SaveData object for GameManager to retrieve previously saved values.</returns>
    public static SaveData LoadData()
    {
        string path = Application.persistentDataPath + "save.asteroid";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("No save file found to load in " + path);
            return null;
        }
    }
}
