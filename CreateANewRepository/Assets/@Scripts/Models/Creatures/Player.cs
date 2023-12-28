using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Creature {

    #region Input

    protected void OnMove(InputValue value) {
        Velocity = value.Get<Vector2>().normalized * Status[StatType.MoveSpeed].Value;
    }
    protected void OnLook(InputValue value) {
        LookDirection = (Camera.main.ScreenToWorldPoint(value.Get<Vector2>()) - this.transform.position).normalized;
    }
    protected void OnFire() {
        Projectile projectile = Main.Object.SpawnProjectile(this.transform.position).SetInfo(this);
        projectile.Velocity = LookDirection.normalized * 10f; // TODO::
    }

    #endregion

}