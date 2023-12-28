using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StatModifierInfo : UI_Base {

    #region Enums

    enum Texts {
        txtStatType,
        txtStatValue,
    }

    #endregion

    #region Fields

    private StatModifier _modifier;

    #endregion

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindText(typeof(Texts));

        return true;
    }

    public void SetInfo(StatModifier modifier) {
        Initialize();

        this._modifier = modifier;

        GetText((int)Texts.txtStatType).text = _modifier.Stat switch {
            StatType.HpMax => "체력",
            StatType.HpRegen => "체력재생",
            StatType.Damage => "피해랑",
            StatType.Defense => "방어력",
            StatType.MoveSpeed => "이동속도",
            StatType.AttackSpeed => "공격속도",
            _ => ""
        };

        GetText((int)Texts.txtStatValue).text = $"{(_modifier.Type == StatModifierType.Multiple ? "x" : "")}{_modifier.Value:+0.###;-0.###}";

    }
}