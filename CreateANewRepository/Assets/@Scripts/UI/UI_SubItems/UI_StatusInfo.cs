using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StatusInfo : UI_Base {

    #region Fields

    private Status _status;

    #endregion

    #region Initialize / Set

    public override bool Initialize() {
        if(!base.Initialize()) return false;

        return true;
    }

    public void SetInfo(Status status) {
        _status = status;

        this.gameObject.DestroyChilds();
        for (int i = 0; i < (int)StatType.COUNT; i++) {
            UI_StatInfo statInfo = Main.UI.CreateSubItem<UI_StatInfo>(this.transform);
            statInfo.SetInfo(_status, (StatType)i);
        }
    }

    #endregion

}