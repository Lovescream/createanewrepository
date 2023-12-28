using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : Thing {

    #region Properties

    public CreatureData Data { get; private set; }

    public State<CreatureState> State { get; private set; }
    public Status Status { get; private set; }
    public CreatureInventory Inventory { get; private set; }

    public float Hp {
        get => _hp;
        set {
            if (_hp == value) return;
            if (value <= 0) {
                _hp = 0;
                State.Current = CreatureState.Dead;

            }
            else if (value >= Status[StatType.HpMax].Value) {
                _hp = Status[StatType.HpMax].Value;
            }
            else _hp = value;
            OnChangedHp?.Invoke(_hp);
        }
    }
    public Vector2 Velocity { get; protected set; }
    public Vector2 LookDirection { get; protected set; }
    public float LookAngle => Mathf.Atan2(LookDirection.y, LookDirection.x) * Mathf.Rad2Deg;

    public UI_HpBar HpBar { get; set; }

    #endregion

    #region Fields

    protected static readonly int AnimatorParameterHash_Speed = Animator.StringToHash("Speed");
    protected static readonly int AnimatorParameterHash_Hit = Animator.StringToHash("Hit");
    protected static readonly int AnimatorParameterHash_Dead = Animator.StringToHash("Dead");

    // State, Status.
    private float _hp;

    // Components.
    protected SpriteRenderer _spriter;
    protected Collider2D _collider;
    protected Rigidbody2D _rigidbody;
    protected Animator _animator;

    // Callbacks.
    public event Action<float> OnChangedHp;

    #endregion

    #region MonoBehaviours

    protected virtual void FixedUpdate() {
        State.OnStay();
        _spriter.flipX = LookDirection.x < 0;
        _rigidbody.velocity = Velocity;
        _animator.SetFloat(AnimatorParameterHash_Speed, Velocity.magnitude);
    }

    #endregion

    #region Initialize / Set

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        _spriter = this.GetComponent<SpriteRenderer>();
        _collider = this.GetComponent<Collider2D>();
        _rigidbody = this.GetComponent<Rigidbody2D>();
        _animator = this.GetComponent<Animator>();

        return true;
    }
    public virtual void SetInfo(CreatureData data) {
        Initialize();

        this.Data = data;

        _animator.runtimeAnimatorController = Main.Resource.Load<RuntimeAnimatorController>($"{Data.Key}.animController");
        _animator.SetBool(AnimatorParameterHash_Dead, false);

        _collider.enabled = true;
        if (_collider is BoxCollider2D boxCollider) {
            Sprite sprite = _spriter.sprite;
            float x = sprite.textureRect.width / sprite.pixelsPerUnit;
            float y = sprite.textureRect.height / sprite.pixelsPerUnit;
            boxCollider.size = new(x, y);
        }
        _rigidbody.simulated = true;

        SetStateEvent();
        SetStatus(isFullHp: true);
        SetInventory();
    }
    protected virtual void SetStatus(bool isFullHp = true) {
        this.Status = new(Data);
        Hp = Status[StatType.HpMax].Value;

        OnChangedHp -= ShowHpBar;
        OnChangedHp += ShowHpBar;
    }
    protected virtual void SetStateEvent() {
        State = new();
        State.AddOnEntered(CreatureState.Hit, () => _animator.SetTrigger(AnimatorParameterHash_Hit));
        State.AddOnEntered(CreatureState.Dead, () => {
            _collider.enabled = false;
            _rigidbody.simulated = false;
            _animator.SetBool(AnimatorParameterHash_Dead, true);
        });
    }
    protected virtual void SetInventory() {
        Inventory = new(this, new() {
            [ItemType.Weapon] = 3,
            [ItemType.Armor] = 3,
        }, 20);
    }

    #endregion

    #region State

    public virtual void OnHit(Creature attacker, float damage = 0, KnockbackInfo knockbackInfo = default) {
        Hp -= damage;

        if (knockbackInfo.time > 0) {
            State.Current = CreatureState.Hit;
            Velocity = knockbackInfo.KnockbackVelocity;
            State.SetStateAfterTime(CreatureState.Idle, knockbackInfo.time);
        }
    }

    #endregion

    private void ShowHpBar(float hp) {
        if (HpBar != null) {
            HpBar.ResetInfo();
            return;
        }
        HpBar = Main.UI.ShowHpBar(this);
    }
}

public enum CreatureState {
    Idle,
    Hit,
    Dead,
}

public struct KnockbackInfo {
    public Vector2 KnockbackVelocity => direction.normalized * speed;

    public float time;
    public float speed;
    public Vector2 direction;
}