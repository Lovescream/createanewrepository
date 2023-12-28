using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager {


    private static readonly int InitialPopupOrder = 10;

    #region Properties

    public GameObject Root {
        get {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null) root = new GameObject { name = "@UI_Root" };
            return root;
        }
    }

    #endregion

    #region Fields

    private int _popupOrder = InitialPopupOrder;

    // Collections.
    private List<UI_Popup> _popups = new();

    #endregion

    public void SetCanvas(GameObject obj) {
        Canvas canvas = obj.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        CanvasScaler scaler = obj.GetOrAddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new(1920, 1080);
    }

    public void Clear() {
        CloseAllPopup();
        Time.timeScale = 1;
    }

    #region SceneUI

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject obj = Main.Resource.Instantiate($"{name}.prefab");
        obj.transform.SetParent(Root.transform);

        return obj.GetOrAddComponent<T>();
    }

    #endregion

    #region Popups

    public int OrderUpPopup() {
        _popupOrder++;
        return _popupOrder - 1;
    }

    public void ReorderAllPopups() {
        _popupOrder = InitialPopupOrder;
        for (int i = 0; i < _popups.Count; i++) {
            _popups[i].SetOrder(_popupOrder++);
        }
    }

    public void SetPopupToFront(UI_Popup popup) {
        if (!_popups.Remove(popup)) return;
        _popups.Add(popup);
        ReorderAllPopups();
    }

    public T ShowPopupUI<T>(string name = null) where T : UI_Popup {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        GameObject obj = Main.Resource.Instantiate($"{name}.prefab");
        obj.transform.SetParent(Root.transform);
        T popup = obj.GetOrAddComponent<T>();
        _popups.Add(popup);

        return popup;
    }

    public void ClosePopup(UI_Popup popup) {
        if (_popups.Count == 0) return;

        bool isLatest = _popups[_popups.Count - 1] == popup;

        _popups.Remove(popup);
        Main.Resource.Destroy(popup.gameObject);

        if (isLatest) _popupOrder--;
        else ReorderAllPopups();
    }

    public void CloseAllPopup() {
        if (_popups.Count == 0) return;
        for (int i = _popups.Count - 1; i >= 0; i--) {
            Main.Resource.Destroy(_popups[i].gameObject);
        }
        _popups.Clear();
        _popupOrder = InitialPopupOrder;
    }

    #endregion

    #region SubItem

    public T CreateSubItem<T>(Transform parent = null, string name = null, bool pooling = true) where T : UI_Base {
        if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

        T item = Main.Resource.Instantiate($"{name}.prefab", parent, pooling).GetOrAddComponent<T>();
        item.transform.SetParent(parent);

        return item;
    }

    #endregion

    #region InGame

    public void ShowDamageText(Vector2 position, float damage) {
        DamageText text = Main.Resource.Instantiate("DamageText.prefab", pooling: true).GetOrAddComponent<DamageText>();
        text.SetInfo(position, damage);
    }
    public UI_HpBar ShowHpBar(Creature creature, float duration = 1) {
        UI_HpBar bar = Main.Resource.Instantiate("UI_HpBar.prefab", pooling: true).GetOrAddComponent<UI_HpBar>();
        bar.SetInfo(creature, duration);
        return bar;
    }

    #endregion
}