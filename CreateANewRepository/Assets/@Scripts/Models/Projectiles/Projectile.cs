using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Thing {

    #region Properties

    public Creature Owner { get; protected set; }
    
    public float Duration { get; protected set; }
    public float Damage { get; protected set; }

    public Vector2 Velocity { get; set; }

    #endregion

    #region Fields

    protected Rigidbody2D _rigidbody;

    private Coroutine _coDestroy;

    #endregion

    #region MonoBehaviours

    protected virtual void FixedUpdate() {
        _rigidbody.velocity = Velocity;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        Creature creature = collision.GetComponent<Creature>();
        if (!creature.IsValid() || !this.IsValid()) return;
        if (creature == Owner) return;
        if (creature.State.Current == CreatureState.Dead) return;

        creature.OnHit(Owner, Damage, new() { time = 0.1f, speed = 10f, direction = (creature.transform.position - this.transform.position).normalized });

        _rigidbody.velocity = Velocity = Vector2.zero;
        if (this.IsValid()) Main.Object.DespawnProjectile(this);
    }

    protected void OnDisable() {
        StopAllCoroutines();
        _coDestroy = null;
    }

    #endregion

    #region Initialize / Set

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        this._rigidbody = this.GetComponent<Rigidbody2D>();

        return true;
    }

    public virtual Projectile SetInfo(Creature owner) {
        this.Owner = owner;
        Duration = 5; // TODO::
        Damage = 10; // TODO::

        if (_coDestroy != null) StopCoroutine(this._coDestroy);
        _coDestroy = StartCoroutine(CoDestroy());

        return this;
    }

    #endregion

    private IEnumerator CoDestroy() {
        yield return new WaitForSeconds(Duration);
        _coDestroy = null;
        Main.Object.DespawnProjectile(this);
    }
}