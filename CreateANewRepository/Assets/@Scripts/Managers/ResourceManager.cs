using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResourceManager {

    private Dictionary<string, UnityEngine.Object> _resources = new();
    private Dictionary<string, bool> _loaded = new();

    public event Action<string, int, int> OnLoadAsync;
    public event Action OnLoadAsyncCompleted;

    #region Addressables

    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object {
        if (_resources.TryGetValue(key, out UnityEngine.Object resource)) {
            callback?.Invoke(resource as T);
            return;
        }

        var asyncOperation = Addressables.LoadAssetAsync<T>(key);
        asyncOperation.Completed += op => {

            if (op.Result is Texture2D) {
                var asyncOperatioSprites = Addressables.LoadAssetAsync<IList<Sprite>>(key);
                asyncOperatioSprites.Completed += opSprite => {
                    int loadCount = 0;
                    int totalCount = asyncOperatioSprites.Result.Count;
                    foreach (Sprite sprite in opSprite.Result) {
                        loadCount++;
                        _resources.Add($"{key}[{sprite.name}]", sprite);
                        if (loadCount >= totalCount) {
                            callback?.Invoke(op.Result);
                        }
                    }
                };
            }
            else {
                _resources.Add(key, op.Result);
                callback?.Invoke(op.Result);
            }
        };
    }

    public void LoadAllAsync<T>(string label, Action<string, int, int> cbOnLoadAsync = null, Action cbOnLoadAsyncCompleted = null) where T : UnityEngine.Object {
        OnLoadAsync += cbOnLoadAsync;
        OnLoadAsyncCompleted += cbOnLoadAsyncCompleted;

        var operation = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        operation.Completed += op => {
            int loadCount = 0;
            int totalCount = op.Result.Count;

            foreach (var result in op.Result) {
                LoadAsync<T>(result.PrimaryKey, obj => {
                    loadCount++;
                    OnLoadAsync?.Invoke(result.PrimaryKey, loadCount, totalCount);
                    if (loadCount >= totalCount) {
                        _loaded[label] = true;
                        OnLoadAsyncCompleted?.Invoke();

                        OnLoadAsync = null;
                        OnLoadAsyncCompleted = null;
                    }
                });
            }
        };
    }

    #endregion

    public bool IsLoaded(string label) {
        if (!_loaded.TryGetValue(label, out bool isLoaded)) return false;
        return isLoaded;
    }

    public T Load<T>(string key) where T : UnityEngine.Object {
        if (!_resources.TryGetValue(key, out UnityEngine.Object resource)) {
            Debug.LogError($"[ResourceManager] Load<{typeof(T)}>({key}): Failed to load resource.");
            return null;
        }
        return resource as T;
    }

    public void Unload<T>(string key) where T : UnityEngine.Object {
        if (!_resources.TryGetValue(key, out UnityEngine.Object resource)) {
            Debug.LogError($"[ResourceManager] Unload<{typeof(T)}>({key}): Failed to unload resource.");
            return;
        }
        Addressables.Release(resource);
        _resources.Remove(key);

    }

    public GameObject Instantiate(string key, Transform parent = null, bool pooling = false) {
        GameObject prefab = Load<GameObject>(key);
        if (prefab == null) {
            Debug.LogError($"[ResourceManager] Instantiate({key}): Failed to load prefab.");
            return null;
        }

        if (pooling) return Main.Pool.Pop(prefab);

        GameObject obj = UnityEngine.Object.Instantiate(prefab, parent);
        obj.name = prefab.name;
        return obj;
    }

    public void Destroy(GameObject obj) {
        if (obj == null) return;

        if (Main.Pool.Push(obj)) return;

        UnityEngine.Object.Destroy(obj);
    }
}