using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot_Inventory : UI_ItemSlot, IPointerDownHandler{

    #region Enums

    enum Objects {
        EquipMark
    }

    #endregion

    #region Properties

    public CreatureInventory Inventory { get; protected set; }

    #endregion

    #region MonoBehaviours

    protected override void OnDisable() {
        base.OnDisable();
        if (Inventory != null) Inventory.OnEquipChanged -= Refresh;
    }
    protected override void OnDestroy() {
        base.OnDestroy();
        if (Inventory != null) Inventory.OnEquipChanged -= Refresh;
    }
    public void OnPointerDown(PointerEventData eventData) {
        if (Inventory != null && Item != null)
            OnClickItemSlot();
    }

    #endregion

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindObject(typeof(Objects));

        return true;
    }

    public void SetInfo(CreatureInventory inventory, Item item) {
        Initialize();
        base.SetInfo(item);

        this.Inventory = inventory;

        inventory.OnEquipChanged -= Refresh;
        inventory.OnEquipChanged += Refresh;
        Refresh(Item);
    }

    private void Refresh(Item item) {
        if (Inventory == null || item != Item) return;
        Debug.Log(this.GetComponent<RectTransform>().sizeDelta.x);
        GetObject((int)Objects.EquipMark).SetActive(Inventory.IsEquip(item));
        //GetObject((int)Objects.EquipMark).GetComponent<RectTransform>().sizeDelta = this.GetComponent<RectTransform>().sizeDelta / 4;
    }

    private void OnClickItemSlot() {
        UI_Popup_ConfirmEquip popup = (Main.Scene.CurrentScene.UI as UI_GameScene).Popup_ConfirmEquip;
        if (popup == null) {
            popup = Main.UI.ShowPopupUI<UI_Popup_ConfirmEquip>();
            (Main.Scene.CurrentScene.UI as UI_GameScene).Popup_ConfirmEquip = popup;
        }
        popup.SetInfo(Inventory, Item);
        popup.SetPopupToFront();
    }
}