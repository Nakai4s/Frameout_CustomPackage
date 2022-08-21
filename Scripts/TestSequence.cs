using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using Frameout.Sound;

namespace Frameout{
public class TestSequence : BaseSequence
{
    CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

    async UniTaskVoid Start()
    {
        var token = _cancellationTokenSource.Token;
        
        await UniTask.WaitUntil(() => m_isCompleteInit, cancellationToken : token);
        _cancellationTokenSource.Cancel();
        await UniTask.WaitUntil(() => SoundManager.s_isCompleteLoad, cancellationToken : token);

        await SoundManager.Instance.PlayBgm(0, 2f, 2f, token);
        await SoundManager.Instance.PlayBgm(1, 2f, 2f, token);

        LoadNextScene("New Scene", 2f, 0.5f, 1f, token).Forget();
    }
}
}