using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

namespace Frameout
{
    public class AssetLoader
    {
        public static async UniTask<T> Load<T>(string path, CancellationToken token=default){
            return await Addressables.LoadAssetAsync<T>(path).WithCancellation(token);
        }

        public static async UniTask<IList<T>> LoadAssets<T>(string tag, Action<T> callback, CancellationToken token=default){
            return await Addressables.LoadAssetsAsync<T>(tag, callback).WithCancellation(token);
        }
        
        public static async UniTask Instantiate(string path, CancellationToken token=default){
            await Addressables.InstantiateAsync(path).WithCancellation(token);
        }
    }
}
