using System.IO;
using UnityEngine;
using System;

namespace Frameout{
/// <summary>
/// ローカルファイル保存
/// </summary>
public class SaveManager : SingletonMonoBehaviour<SaveManager>
{
    string m_filePath;

    [HideInInspector]
    public SaveData m_data;
    // 初期化完了フラグ
    public bool IsInit { get; private set; } = false;

    void Start()
    {
        m_filePath = Application.persistentDataPath + "/" + ".savedata.json";
        m_data = new SaveData();

        // ファイルが存在しない場合は初期化して生成
        if(!File.Exists(m_filePath))
        {
            m_data.Initialize();
            Save();
            Debug.Log("Initialize Complete!");
        }

        Load();
    }
    
    public void Save()
    {
        string json = JsonUtility.ToJson(m_data);
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
        m_data = JsonUtility.FromJson<SaveData>(text);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) { Save(); }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("on quit!");
        Save();
    }
}
}