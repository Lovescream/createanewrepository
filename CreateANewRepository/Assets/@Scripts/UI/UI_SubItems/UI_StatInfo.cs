using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StatInfo : UI_Base {

    #region Enums

    enum Texts {
        txtStatType,
        txtStatValue,
    }

    #endregion

    #region Fields

    private Stat _stat;

    #endregion

    #region MonoBehaviours

    protected override void OnDisable() {
        base.OnDisable();

        if (_stat != null) _stat.OnChanged -= SetValue;
    }
    protected override void OnDestroy() {
        if (_stat != null) _stat.OnChanged -= SetValue;
    }

    #endregion

    #region Initialize / Set

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindText(typeof(Texts));

        return true;
    }
    
    public void SetInfo(Status status, StatType type) {
        Initialize();

        GetText((int)Texts.txtStatType).text = type switch {
            StatType.HpMax => "체력",
            StatType.HpRegen => "체력재생",
            StatType.Damage => "피해랑",
            StatType.Defense => "방어력",
            StatType.MoveSpeed => "이동속도",
            StatType.AttackSpeed => "공격속도",
            _=>""
        };

        _stat = status[type];
        _stat.OnChanged += SetValue;
        SetValue(_stat);
    }

    private void SetValue(Stat stat) {
        float originValue = stat.OriginValue;
        float deltaValue = stat.Value - originValue;
        GetText((int)Texts.txtStatValue).text = deltaValue == 0 ? $"{originValue}" : $"{originValue} <color=yellow>({deltaValue:+0.###;-0.###})</color>";
    }

    #endregion


}