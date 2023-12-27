using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager {

    public Dictionary<string, CreatureData> Creatures = new();
    //public Dictionary<string, CharacterData> Characters = new();
    //public Dictionary<string, ItemData> Items = new();

    public void Initialize() {
        Creatures = LoadJson<CreatureData>();
        //Characters = LoadJson<CharacterData>();
        //Items = LoadJson<ItemData>();
    }

    private Dictionary<string, T> LoadJson<T>() where T : Data {
        return JsonConvert.DeserializeObject<List<T>>(Main.Resource.Load<TextAsset>($"{typeof(T).Name}").text).ToDictionary(data => data.Key);
    }
}

public class Data {
    public string Key { get; set; }
}