using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Frameout.Save{
[Serializable]
public class SaveData
{
    public string m_referenceTime;
    public bool isFirstPlay;
    public int testCount;
    public string test;

    Dictionary<string, string> m_dictionary = new Dictionary<string, string>();

    public void Add(string key, string value){
        m_dictionary.Add(key, value);
    }

    public void SetValue(string key, string value){
        m_dictionary[key] = value;
    }

    public string GetValue(string key){
        return m_dictionary[key];
    }

    // すべてのパラメータを初期値に戻す
    public void Initialize()
    {
        Add("Item", "0.1f");
        Debug.Log(m_dictionary["Item"]);
        SetValue("Item", "FEJOWFP");
        Debug.Log(m_dictionary["Item"]);

        m_referenceTime = DateTime.Now.ToString();
        isFirstPlay = true;
        testCount = 0;
        test = "";
    }
}
}
