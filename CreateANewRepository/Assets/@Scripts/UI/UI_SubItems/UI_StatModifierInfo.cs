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
            StatType.HpMax => "ü��",
            StatType.HpRegen => "ü�����",
            StatType.Damage => "���ض�",
            StatType.Defense => "����",
            StatType.MoveSpeed => "�̵��ӵ�",
            StatType.AttackSpeed => "���ݼӵ�",
            _ => ""
        };

        GetText((int)Texts.txtStatValue).text = $"{(_modifier.Type == StatModifierType.Multiple ? "x" : "")}{_modifier.Value:+0.###;-0.###}";

    }
}