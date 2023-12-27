using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CreatureData : Data {
    public string Name { get; set; }
    public float HpMax { get; set; }
    public float HpRegen { get; set; }
    public float Damage { get; set; }
    public float Defense { get; set; }
    public float MoveSpeed { get; set; }
    public float AttackSpeed { get; set; }
}