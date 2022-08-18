using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Frameout{
    /// <summary>シーンを跨ぐフェードパネルを管理</summary>
public class Fader : SingletonMonoBehaviour<Fader>
{
    protected GameObject m_fadePanel;
    protected Image m_image;

    public bool IsFading { get; set; }
    public float Alpha { get; set; }
    
    void Start()
    {
        m_fadePanel = transform.GetChild(0).gameObject;
        m_fadePanel.TryGetComponent(out m_image);
        m_fadePanel.SetActive(false);
    }

    public async UniTaskVoid Play(float inTime, float outTime, float waitTime, CancellationToken token=default){
        token.ThrowIfCancellationRequested();

        if(!m_fadePanel.activeSelf){
            await FadeIn(inTime, token);

            await UniTask.Delay((int)waitTime*1000, cancellationToken: token);
            
            await FadeOut(outTime, token);
        }
    }

    public async UniTask FadeIn(float maxTime, CancellationToken token=default){
        m_fadePanel.SetActive(true);
        IsFading = true;

        float timer = 0f;
        while(timer < maxTime){
            Alpha = Mathf.Lerp(0f, 1f, timer / maxTime);
            if(Alpha > 0.95f) { Alpha = 1f; }

            m_image.color = SetColor(m_image.color, Alpha);
            timer += Time.deltaTime;
            await UniTask.Yield(token);
        }
    }

    public async UniTask FadeOut(float maxTime, CancellationToken token=default){
        float timer = 0f;
        while(timer < maxTime){
            Alpha = Mathf.Lerp(1f, 0f, timer / maxTime);
            if(Alpha > 0.95f) { Alpha = 1f; }

            m_image.color = SetColor(m_image.color, Alpha);
            timer += Time.deltaTime;
            await UniTask.Yield(token);
        }

        IsFading = false;
        m_fadePanel.SetActive(false);
    }

    Color SetColor(Color c, float alpha){
        c.a = alpha;
        return c;
    }
}
}