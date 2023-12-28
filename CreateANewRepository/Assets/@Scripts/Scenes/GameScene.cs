using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene {

    public Player Player { get; private set; }

    protected override bool Initialize() {
        if (!base.Initialize()) return false;

        Player = Main.Object.SpawnPlayer("Character_00", Vector2.zero);
        Main.Object.SpawnEnemy("Character_01", new(4, 0));
        Main.Object.SpawnEnemy("Character_02", new(0, 4));

        UI = Main.UI.ShowSceneUI<UI_GameScene>();

        return true;
    }

}