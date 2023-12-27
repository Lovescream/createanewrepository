using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status {
    private Dictionary<StatType, Stat> _stats;

    public Stat this[StatType type] {
        get => _stats[type];
    }

    public Status() {
        _stats = new();
        for (int i = 0; i < (int)StatType.COUNT; i++) {
            _stats.Add((StatType)i, new Stat((StatType)i));
        }
    }
    public Status(CreatureData data) {
        _stats = new() {
            [StatType.HpMax] = new(StatType.HpMax, data.HpMax),
            [StatType.HpRegen] = new(StatType.HpRegen, data.HpRegen),
            [StatType.Damage] = new(StatType.Damage, data.Damage),
            [StatType.Defense] = new(StatType.Defense, data.Defense),
            [StatType.MoveSpeed] = new(StatType.MoveSpeed, data.MoveSpeed),
            [StatType.AttackSpeed] = new(StatType.AttackSpeed, data.AttackSpeed),
        };
    }

    public void AddModifiers(List<StatModifier> modifiers) {
        for (int i = 0; i < modifiers.Count; i++) {
            this[modifiers[i].Stat].AddModifier(modifiers[i]);
        }
    }
    public void RemoveModifiers(List<StatModifier> modifiers) {
        for (int i = 0; i < modifiers.Count; i++) {
            this[modifiers[i].Stat].RemoveModifier(modifiers[i]);
        }
    }
}

public enum StatType {
    HpMax,
    HpRegen,
    Damage,
    Defense,
    MoveSpeed,
    AttackSpeed,
    COUNT,
}
public enum StatModifierType {
    Add,
    Multiple,
    Override,
}