using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
public static class GridSaveControler
{
    public static void Save(MGrid grid)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/GridData.jpg";
        FileStream stream = new FileStream(path, FileMode.Create);

        GridData data = new GridData
        {
            grid = grid.Tiles
        };
        ;
        data.size = grid.Size;
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static GridData Load()
    {
        string path = Application.persistentDataPath + "/GridData.jpg";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GridData data = formatter.Deserialize(stream) as GridData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save not found");
            return null;
        }
    }
}
