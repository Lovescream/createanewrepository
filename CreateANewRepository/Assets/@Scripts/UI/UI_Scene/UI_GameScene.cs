using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameScene : UI_Scene {

    #region Enums

    enum Objects {
        UI_HpInfo,
        UI_StatusInfo,
    }
    #endregion

    #region Fields

    private Player _player;

    #endregion

    #region Initialize / Set

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindObject(typeof(Objects));

        _player = (Main.Scene.CurrentScene as GameScene).Player;

        GetObject((int)Objects.UI_HpInfo).GetComponent<UI_HpInfo>().SetInfo(_player);
        GetObject((int)Objects.UI_StatusInfo).GetComponent<UI_StatusInfo>().SetInfo(_player.Status);

        return true;
    }

    #endregion


}