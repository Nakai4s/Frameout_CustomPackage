using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace Frameout{
/// <summary>CSVファイルを扱うクラス</summary>
public class CsvReader
{
    // csvから読み込んだ文字列リスト
    List<string[]> m_strArr = new List<string[]>();

    // ロード完了フラグ
    public bool IsCompleteLoad { get; private set; } = false;

    /// <summary>リストから指定の文字列を取得</summary>
    /// <returns>文字列[row][column]</returns>
    public string GetString(int row, int column)
    {
        return m_strArr[row][column];
    }
    
    /// <summary>
    /// リスト数を取得
    /// </summary>
    public int GetCount()
    {
        return m_strArr.Count;
    }

    /// <summary>csvファイルをロード</summary>
    /// <param name="filename">拡張子を除いたファイル名</param>
    public async UniTask<bool> Load(string filename, CancellationToken token=default)
    {
        IsCompleteLoad = false;

        try{
            var csvHandle = await Addressables.LoadAssetAsync<TextAsset>(filename + ".csv");//.WithCancellation(token);
            var _csvFile = csvHandle as TextAsset;

            StringReader stringReader = new StringReader(_csvFile.text);
            while(stringReader.Peek() != -1)//最後まで読み込むと-1になる関数
            {
                string line = stringReader.ReadLine();
                m_strArr.Add(line.Split(','));//,区切りでリストに追加していく
            }

            IsCompleteLoad = true;
        }
        catch(OperationCanceledException e){
            Debug.Log(e);
            return false;
        }

        return true;
    }
}
}