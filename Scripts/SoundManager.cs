using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Frameout.Sound{
public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    // ボリューム
    float m_masterVolume = 1f;
    float m_bgmVolume = 1f;
    float m_seVolume = 1f;

    // オーディオソース
    AudioSource m_bgmAudio, m_seAudio;

    // オーディオクリップ
    IList<AudioClip> m_bgmClips = new List<AudioClip>();
    IList<AudioClip> m_seClips = new List<AudioClip>();

    // ロード完了フラグ
    public static bool s_isCompleteLoad { get; private set; } = false;

    // サウンドファイル名と番号を紐つけ
    Dictionary<string, int> m_bgmIndex = new Dictionary<string, int>();
    Dictionary<string, int> m_seIndex = new Dictionary<string, int>();

    CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    public float MasterVolume {
        set{
            m_masterVolume = Mathf.Clamp01(value);
            m_bgmAudio.volume = m_bgmVolume * m_masterVolume;
            m_seAudio.volume = m_seVolume * m_masterVolume;
        }
        get{
            return m_masterVolume;
        }
    }
    public float BgmVolume {
        set{
            m_bgmVolume = Mathf.Clamp01(value);
            m_bgmAudio.volume = m_bgmVolume * m_masterVolume;
        }
        get{
            return m_bgmVolume;
        }
    }
    public float SeVolume {
        set{
            m_seVolume = Mathf.Clamp01(value);
            m_seAudio.volume = m_seVolume * m_masterVolume;
        }
        get{
            return m_seVolume;
        }
    }

    async UniTask<bool> LoadSound(CancellationToken token){
        // bgm
        int bgm_index = 0;
        var bgmHandle = await Addressables.LoadAssetsAsync<AudioClip>("bgm", obj =>
        {
            m_bgmIndex.Add(obj.name, bgm_index++);
        }).WithCancellation(token);
        m_bgmClips = bgmHandle as IList<AudioClip>;

        // se
        int se_index = 0;
        var seHandle = await Addressables.LoadAssetsAsync<AudioClip>("se", obj =>
        {
            m_seIndex.Add(obj.name, se_index++);
        }).WithCancellation(token);
        m_seClips = seHandle as IList<AudioClip>;

        s_isCompleteLoad = true;
        
        if(token.IsCancellationRequested){
            return false;
        }
        
        return true;
    }

    async UniTaskVoid Start(){
        var token = _cancellationTokenSource.Token;

        m_bgmAudio = gameObject.AddComponent<AudioSource>() as AudioSource;
        m_seAudio = gameObject.AddComponent<AudioSource>() as AudioSource;

        await LoadSound(token);
    }

    int GetBgmIndex(string name){
        if(m_bgmIndex.ContainsKey(name)){
            return m_bgmIndex[name];
        }
        else{
            Debug.LogError("bgm file not found...");
            return 0;
        }
    }

    int GetSeIndex(string name){
        if(m_seIndex.ContainsKey(name)){
            Debug.Log(name);
            return m_seIndex[name];
        }
        else{
            Debug.LogError("se file not found...");
            return 0;
        }
    }

    async UniTask FadeIn(float maxTime, CancellationToken token=default){
        float timer = 0f;

        while(timer < maxTime){
            if(m_bgmAudio){
                BgmVolume = Mathf.Lerp(0f, 1f, timer/maxTime);
            if(BgmVolume > 0.95f) { BgmVolume = 1f; }
            timer += Time.deltaTime;
            await UniTask.Yield(token);
            }
            
        }
    }

    async UniTask FadeOut(float maxTime, CancellationToken token=default){ 
        float timer = 0f;

        while(timer < maxTime){
            if(m_bgmAudio){
                BgmVolume = Mathf.Lerp(1f, 0f, timer/maxTime);
            if(BgmVolume > 0.95f) { BgmVolume = 1f; }
            timer += Time.deltaTime;
            await UniTask.Yield(token);
            }
            
        }
    }
    
    /// <summary>
    /// BGM再生
    /// </summary>
    public async UniTask PlayBgm(int index, float inTime=1f, float outTime=1f, CancellationToken token=default){

        await FadeOut(outTime, token);

        index = Mathf.Clamp(index, 0, m_bgmClips.Count-1);
        m_bgmAudio.clip = m_bgmClips[index];
        m_bgmAudio.loop = true;
        m_bgmAudio.Play();
        
        await FadeIn(inTime, token);
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    public async UniTask PlayBgmByName(string name, float inTime=1f, float outTime=1f, CancellationToken token=default){
        await PlayBgm(GetBgmIndex(name), inTime, outTime, token);
    }

    public void StopBgm(){
        m_bgmAudio.Stop();
        m_bgmAudio.clip = null;
    }

    /// <summary>
    /// SE再生
    /// </summary>
    public void PlaySe(int index){
        index = Mathf.Clamp(index, 0, m_seClips.Count-1);
        m_seAudio.PlayOneShot(m_seClips[index], SeVolume * MasterVolume);
    }

    /// <summary>
    /// SE再生
    /// </summary>
    public void PlaySeByName(string name){
        PlaySe(GetSeIndex(name));
    }

    public void StopSe(){
        m_seAudio.Stop();
        m_seAudio.clip = null;
    }

    void OnApplicationQuit()
    {
        Debug.Log("cancel!");
        _cancellationTokenSource.Cancel();
    }
}
}
