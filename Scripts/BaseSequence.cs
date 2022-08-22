using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Frameout{
public abstract class BaseSequence : MonoBehaviour
{
    public bool m_isCompleteInit { get; private set; } = false;
    
    const string m_filePath = "Packages/com.frameout.custom_package/Scripts/UtilityManager.prefab";

    protected SaveManager _saveManager;
    
    protected virtual async UniTaskVoid Awake(){
        var token = this.GetCancellationTokenOnDestroy();
        ScreenAspect.Init();
        
        _saveManager = new SaveManager();

        // 初回のみオブジェクトを生成
        if(!m_isCompleteInit){
            await AssetLoader.Instantiate(m_filePath, token);
            m_isCompleteInit = true;
        }
    }

    /// <summary>
    /// フェードを挟んでシーン移動する
    /// </summary>
    public async UniTaskVoid LoadNextScene(string name, float inTime = 1f, float outTime = 1f, float waitTime = 1f, CancellationToken token = default){
        if(!Fader.Instance.IsFading){
            // フェードイン
            await Fader.Instance.FadeIn(inTime, token);
            // シーンロード
            await SceneManager.LoadSceneAsync(name).WithCancellation(token);
            // 指定時間待機
            await UniTask.Delay((int)waitTime*1000, cancellationToken : token);
            // フェードアウト
            await Fader.Instance.FadeOut(outTime, token);
        }
    }

    void OnDestroy()
    {
        Debug.Log("OnDestroy");
        _saveManager.Save();
    }


    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) 
        {
            Debug.Log("OnApplicationPause");
            _saveManager.Save();
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        _saveManager.Save();
    }
}
}