using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene {

    protected override bool Initialize() {
        if (!base.Initialize()) return false;

        UI = Main.UI.ShowSceneUI<UI_TitleScene>();

        return true;
    }

}