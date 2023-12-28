using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatModifier {

    public StatType Stat { get; set; }
    public StatModifierType Type { get; set; }
    public float Value { get; set; }

    public StatModifier() { }
    public StatModifier(StatType stat, StatModifierType type, float value) {
        Stat = stat;
        Type = type;
        Value = value;
    }
    public StatModifier(string s) {
        string[] strings = s.Split('_');
        Stat = (StatType)Enum.Parse(typeof(StatType), strings[0]);
        Type = (StatModifierType)Enum.Parse(typeof(StatModifierType), strings[1]);
        Value = float.Parse(strings[2]);
    }
    public StatModifier Copy() {
        return new(Stat, Type, Value);
    }
}