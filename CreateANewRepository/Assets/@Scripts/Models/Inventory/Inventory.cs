using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory {

    #region Properties

    public float Gold {
        get => _gold;
        set {
            _gold = value;
            OnGoldChanged?.Invoke(_gold);
        }
    }
    public int Count => _items.Count;
    public int MaxCount { get; protected set; }

    #endregion

    #region Fields

    private float _gold;

    // Collections.
    private List<Item> _items = new();

    // Callbacks.
    public event Action<float> OnGoldChanged;
    public event Action OnChanged;

    #endregion

    public Inventory(int maxCount = 99) {
        this.MaxCount = maxCount;
    }
    public Item this[string key] => _items.FirstOrDefault(item => item.Key.Equals(key));
    public Item this[int index] => index < _items.Count ? _items[index] : null;
    
    public void Add(Item item) {
        _items.Add(item);
        OnChanged?.Invoke();
    }
    public void Remove(Item item) {
        _items.Remove(item);
        OnChanged?.Invoke();
    }

}