using System.IO;
using UnityEngine;

namespace Frameout{
public class SaveManager
{
    string m_filePath;
    
    public SaveData saveData;
    
    public static bool IsCompleteInit { get; private set; } = false;

    public SaveManager()
    {
        m_filePath = Application.persistentDataPath + "/.savedata.json";
        saveData = new SaveData();

        // ファイルが存在しない場合は初期化して生成
        if(!File.Exists(m_filePath))
        {
            saveData.Clear();
            Save();

            IsCompleteInit = true;
        }

        Load();
    }
    
    public void Save()
    {
        string json = JsonUtility.ToJson(saveData);
        StreamWriter streamWriter = new StreamWriter(m_filePath);
        streamWriter.Write(json);
        streamWriter.Flush();
        streamWriter.Close();
    }

    public void Load()
    {
        StreamReader streamReader = new StreamReader(m_filePath);
        string text = streamReader.ReadToEnd();
        streamReader.Close();
        saveData = JsonUtility.FromJson<SaveData>(text);
    }
}
}