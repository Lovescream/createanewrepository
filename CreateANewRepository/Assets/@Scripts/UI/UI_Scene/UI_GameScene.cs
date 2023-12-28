using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameScene : UI_Scene {

    #region Enums

    enum Objects {
        UI_HpInfo,
        UI_StatusInfo,
    }
    enum Buttons {
        btnInventory,
    }

    #endregion

    #region Properties

    public UI_Popup_Inventory Popup_Inventory { get; set; }
    public UI_Popup_ConfirmEquip Popup_ConfirmEquip { get; set; }

    #endregion

    #region Fields

    private Player _player;
    #endregion

    #region Initialize / Set

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));

        _player = (Main.Scene.CurrentScene as GameScene).Player;

        GetObject((int)Objects.UI_HpInfo).GetComponent<UI_HpInfo>().SetInfo(_player);
        GetObject((int)Objects.UI_StatusInfo).GetComponent<UI_StatusInfo>().SetInfo(_player.Status);

        GetButton((int)Buttons.btnInventory).onClick.AddListener(OnBtnInventory);

        return true;
    }

    #endregion

    #region OnButtons

    private void OnBtnInventory() {
        if (Popup_Inventory != null) {
            Popup_Inventory.SetPopupToFront();
            Popup_Inventory.SetInfo(_player.Inventory);
            return;
        }
        Popup_Inventory = Main.UI.ShowPopupUI<UI_Popup_Inventory>();
        Popup_Inventory.SetInfo(_player.Inventory);
    }

    #endregion
}