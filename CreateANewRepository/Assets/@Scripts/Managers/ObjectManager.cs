using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager {
    
    public Player Player { get; private set; }
    public List<Enemy> Enemies { get; private set; } = new();
    public List<Projectile> Projectiles { get; private set; } = new();

    public Transform EnemyParent {
        get {
            GameObject root = GameObject.Find("@Enemies");
            if (root == null) root = new("@Enemies");
            return root.transform;
        }
    }

    public void Clear() {
        Enemies.Clear();
        Projectiles.Clear();
    }

    public Player SpawnPlayer(string key, Vector2 position) {
        Player = Spawn<Player>(key, position);
        Player.SetInfo(Main.Data.Creatures[key]);
        return Player;
    }
    public Enemy SpawnEnemy(string key, Vector2 position) {
        Enemy enemy = Spawn<Enemy>(key, position);
        Enemies.Add(enemy);
        enemy.SetInfo(Main.Data.Creatures[key]);
        return enemy;
    }
    public void DespawnEnemy(Enemy obj) {
        Enemies.Remove(obj);
        Despawn(obj);
    }
    public Projectile SpawnProjectile(Vector2 position) {
        Projectile projectile = Spawn<Projectile>("", position);
        Projectiles.Add(projectile);
        return projectile;
    }
    public void DespawnProjectile(Projectile obj) {
        Projectiles.Remove(obj);
        Despawn(obj);
    }

    private T Spawn<T>(string key, Vector2 position) where T : Thing {
        Type type = typeof(T);

        string prefabName = null;
        while (type != null) {
            prefabName = type.Name;
            if (Main.Resource.IsExist($"{prefabName}.prefab")) break;
            type = type.BaseType;
        }
        if (string.IsNullOrEmpty(prefabName)) prefabName = "Thing.prefab";

        GameObject obj = Main.Resource.Instantiate($"{prefabName}.prefab", pooling: true);
        obj.transform.position = position;

        return obj.GetOrAddComponent<T>();
    }
    private void Despawn<T>(T obj) where T : Thing {
        Main.Resource.Destroy(obj.gameObject);
    }

}