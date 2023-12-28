using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HpInfo : UI_Base {

    #region Enums

    enum Texts {
        txtHp,
    }
    enum Sliders {
        hpBar,
    }

    #endregion

    #region Fields

    private Player _player;
    private float _hpMax;

    #endregion

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindText(typeof(Texts));
        BindSlider(typeof(Sliders));

        return true;
    }

    public void SetInfo(Player player) {
        Initialize();

        this._player = player;
        this._hpMax = _player.Status[StatType.HpMax].Value;
        this._player.Status[StatType.HpMax].OnChanged += SetMax;
        this._player.OnChangedHp += Refresh;
        Refresh(this._player.Hp);
    }

    private void SetMax(Stat stat) => this._hpMax = stat.Value;
    private void Refresh(float currentHp) {
        if (_hpMax == 0) return;
        GetText((int)Texts.txtHp).text = $"{currentHp:F0}/{_hpMax:F0}";
        GetSlider((int)Sliders.hpBar).value = currentHp / _hpMax;
    }
}