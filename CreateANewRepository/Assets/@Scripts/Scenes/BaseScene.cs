using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseScene : MonoBehaviour {

    protected virtual string Label => "PreLoad";
    public UI_Scene UI { get; protected set; }

    private bool _initialized = false;

    void Start() {
        if (Main.Resource.IsLoaded(Label)) {
            OnLoadAsyncCompleted();
        }
        else {
            Main.Resource.LoadAllAsync<UnityEngine.Object>(Label, OnLoadAsync, OnLoadAsyncCompleted);
        }
    }

    protected virtual void OnLoadAsync(string key, int count, int totalCount) {
        Debug.Log($"[BaseScene] Load asset {key} ({count} / {totalCount})");
    }
    protected virtual void OnLoadAsyncCompleted() {
        Main.Data.Initialize();
        Initialize();
    }

    protected virtual bool Initialize() {
        if (_initialized) return false;

        Main.Scene.CurrentScene = this;

        Object obj = FindObjectOfType<EventSystem>();
        if (obj == null) Main.Resource.Instantiate("EventSystem.prefab").name = "@EventSystem";

        GameObject.Find("Test").GetComponent<SpriteRenderer>().sprite = Main.Resource.Load<Sprite>("UI[Title 2]");

        _initialized = true;
        return true;
    }

}