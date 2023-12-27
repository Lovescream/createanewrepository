using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterData : Data {
    public string Name { get; set; }
    public string Description { get; set; }
    public float Hp { get; set; }
    public float Damage { get; set; }
    public float Defense { get; set; }
    public float Critical { get; set; }
}