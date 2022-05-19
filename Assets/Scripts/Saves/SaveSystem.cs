using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Sistema de guardado
/// </summary>
public static class SaveSystem
{
    /// <summary>
    /// Guarda información en un archivo específico
    /// </summary>
    /// <param name="nombreArchivo">Nombre que queremos para el archivo</param>
    /// <param name="info">Información a guardar</param>
    public static void SaveObject(string nombreArchivo, object info)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.dataPath + "/ArchivosGuardados/" + nombreArchivo; //donde se guarda
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, info);

        stream.Close();
    }

    /// <summary>
    /// Carga un objeto previamente guardado
    /// </summary>
    /// <param name="archivo">Nombre del archivo en el que se encuentra la información</param>
    /// <returns>Un objeto con la información cargada</returns>
    public static object LoadObject(string archivo)
    {
        string path = Application.dataPath + "/ArchivosGuardados/" + archivo; //donde se guarda

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            object data = formatter.Deserialize(stream); //un static cast basicamente
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("No se ha encontrado el archivo: " + path);
            return null;
        }
    }
}
