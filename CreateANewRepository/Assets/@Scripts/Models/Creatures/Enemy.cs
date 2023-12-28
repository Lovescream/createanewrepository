using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature {

    protected override void SetStateEvent() {
        base.SetStateEvent();
        State.AddOnStay(CreatureState.Idle, () => {
            Velocity = Vector2.zero;
        });
    }

}