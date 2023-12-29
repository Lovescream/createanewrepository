using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Popup : UI_Base, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

    protected Canvas _canvas;
    protected Transform _panel;

    private Vector2 _originPosition;
    private Vector2 _dragStartPosition;
    private Vector2 _dragOffset;

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        this.SetCanvas();
        _canvas = this.GetComponent<Canvas>();
        SetOrder();

        _panel = this.transform.Find("Panel");

        return true;
    }

    protected override void SetOrder() => _canvas.sortingOrder = Main.UI.OrderUpPopup();

    public void SetOrder(int order) => _canvas.sortingOrder = order;

    public virtual void ClosePopup() => Main.UI.ClosePopup(this);

    public void OnPointerDown(PointerEventData eventData) {
        if (_panel == null) return;

        this.SetPopupToFront();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        _originPosition = _panel.position;
        _dragStartPosition = eventData.position;
        _dragOffset = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData) {
        if (_panel == null) return;
        _dragOffset = eventData.position - _dragStartPosition;
        _panel.position = _originPosition + _dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData) {
        _originPosition = _panel.position;
        _dragStartPosition = Vector2.zero;
        _dragOffset = Vector2.zero;
    }
}