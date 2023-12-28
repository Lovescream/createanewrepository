using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup_ConfirmEquip : UI_Popup {

    #region Enums

    enum Texts {
        txtItemName,
        txtItemDescription,
        txtMessage,
    }
    enum Buttons {
        btnCancel,
        btnConfirm,
    }
    enum Objects {
        UI_ItemSlot,
        Layout,
    }

    #endregion

    #region Fields

    private CreatureInventory _inventory;
    private Item _item;

    private Transform _layout;

    #endregion

    #region Initialize / Set

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindObject(typeof(Objects));

        _layout = GetObject((int)Objects.Layout).transform;

        GetButton((int)Buttons.btnCancel).onClick.AddListener(OnBtnCancel);
        GetButton((int)Buttons.btnConfirm).onClick.AddListener(OnBtnConfirm);

        return true;
    }

    public void SetInfo(CreatureInventory inventory, Item item) {
        Initialize();

        this._item = item;
        this._inventory = inventory;

        GetText((int)Texts.txtItemName).text = _item.Name;
        GetText((int)Texts.txtItemDescription).text = _item.Description;
        GetObject((int)Objects.UI_ItemSlot).GetComponent<UI_ItemSlot>().SetInfo(_item);
        _layout.gameObject.DestroyChilds();
        for (int i = 0; i < _item.Modifiers.Count; i++) {
            UI_StatModifierInfo modifierInfo = Main.UI.CreateSubItem<UI_StatModifierInfo>(_layout);
            modifierInfo.SetInfo(_item.Modifiers[i]);
        }

        GetText((int)Texts.txtMessage).text = _inventory.IsEquip(_item) ? "장착 해제 하시겠습니까?" : "장착 하시겠습니까?";
    }

    #endregion

    #region OnButtons

    private void OnBtnCancel() {
        ClosePopup();
    }
    private void OnBtnConfirm() {
        if (_inventory.IsEquip(_item)) _inventory.UnEquip(_item);
        else _inventory.Equip(_item);
        ClosePopup();
    }

    #endregion
}