using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureInventory : Inventory {

    #region Properties

    public Creature Owner { get; protected set; }

    #endregion

    #region Fields

    private readonly Dictionary<ItemType, int> _maxCounts = new();
    private readonly Dictionary<ItemType, List<Item>> _equips = new();

    public event Action<Item> OnEquipChanged;

    #endregion

    public CreatureInventory(Creature owner, Dictionary<ItemType, int> maxEquipCounts, int maxCount) : base(maxCount) {
        this.Owner = owner;
        _maxCounts = maxEquipCounts;

        for (int i = 0; i < (int)ItemType.COUNT; i++) {
            _equips[(ItemType)i] = new();
        }
    }

    public bool IsEquip(Item item) {
        foreach (List<Item> list in _equips.Values) {
            if (list.Contains(item)) return true;
        }
        return false;
    }
    public bool CanEquip(Item item) {
        if (!_maxCounts.TryGetValue(item.Type, out int maxCount)) return false;
        if (!_equips.TryGetValue(item.Type, out List<Item> equips)) return false;
        if (equips.Count >= maxCount) return false;
        if (equips.Contains(item)) return false;
        return true;
    }

    public bool Equip(Item item, bool change = true) {
        if (_equips[item.Type].Count >= _maxCounts[item.Type]) {
            if (!change) return false;
            UnEquip(_equips[item.Type][0]);
        }

        _equips[item.Type].Add(item);

        Owner.Status.AddModifiers(item.Modifiers);

        OnEquipChanged?.Invoke(item);

        return true;
    }
    public bool UnEquip(Item item) {
        if (!_equips.TryGetValue(item.Type, out List<Item> equips)) return false;
        if (!equips.Remove(item)) return false;

        Owner.Status.RemoveModifiers(item.Modifiers);

        OnEquipChanged(item);

        return true;
    }
}