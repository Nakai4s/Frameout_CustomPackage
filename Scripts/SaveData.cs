using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Frameout{
[Serializable]
public class SaveData
{
    public string m_referenceTime;
    public bool isFirstPlay;
    public int testCount;
    public string test;

    // すべてのパラメータを初期値に戻す
    public void Initialize()
    {
        m_referenceTime = DateTime.Now.ToString();
        isFirstPlay = true;
        testCount = 0;
        test = "";
    }
}
}
