using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ItemSlot : UI_Base {

    #region Enums

    enum Images {
        imgItem,
    }

    #endregion

    #region Properties

    public Item Item { get; protected set; }

    #endregion

    #region MonoBehaviours

    protected override void OnDisable() {
        base.OnDisable();
    }
    protected override void OnDestroy() {
        base.OnDestroy();
    }

    #endregion

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindImage(typeof(Images));

        return true;
    }

    public void SetInfo(Item item) {
        Initialize();
        this.Item = item;
        GetImage((int)Images.imgItem).sprite = Main.Resource.LoadSprite($"{item.Key}.sprite");
    }
}