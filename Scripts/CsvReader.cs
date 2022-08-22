using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace Frameout{
/// <summary>CSVファイルを扱うクラス</summary>
public class CsvReader
{
    List<string[]> m_strArr = new List<string[]>();

    public bool IsCompleteLoad { get; private set; } = false;

    public string GetString(int row, int column) { return m_strArr[row][column]; }

    public int GetCount(){ return m_strArr.Count; }
    
    /// <summary>CSVロード</summary>
    public async UniTask<bool> Load(string path, CancellationToken token=default)
    {
        IsCompleteLoad = false;

        try{
            var csvFile = await AssetLoader.Load<TextAsset>(path + ".csv", token);

            StringReader stringReader = new StringReader(csvFile.text);
            while(stringReader.Peek() != -1)//最後まで読み込むと-1になる関数
            {
                string line = stringReader.ReadLine();
                m_strArr.Add(line.Split(','));//,区切りでリストに追加していく
            }

            IsCompleteLoad = true;
        }
        catch(OperationCanceledException e){
            Debug.Log(e);
        }

        return IsCompleteLoad;
    }
}
}