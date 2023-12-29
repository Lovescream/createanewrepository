using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TitleScene : UI_Scene {

    #region Enums

    enum Buttons {
        btnStart,
    }

    #endregion

    #region Initialize / Set

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.btnStart).onClick.AddListener(OnBtnStart);

        return true;
    }

    #endregion

    #region OnButtons

    private void OnBtnStart() {
        Main.Scene.LoadScene("GameScene");
    }

    #endregion
}