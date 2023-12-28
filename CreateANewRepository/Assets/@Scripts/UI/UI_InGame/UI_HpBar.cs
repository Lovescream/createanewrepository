using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_HpBar : UI_Base {

    #region Enums

    enum Images {
        Background,
        Fill,
    }
    enum Sliders {
        HpBar,
    }

    #endregion

    #region Fields

    private float _duration;

    private Creature _owner;
    private float _hpMax;

    private Coroutine _coDestroy;

    #endregion

    public override bool Initialize() {
        if (!base.Initialize()) return false;

        BindImage(typeof(Images));
        BindSlider(typeof(Sliders));

        return true;
    }

    public void SetInfo(Creature owner, float duration = 1) {
        this._owner = owner;
        this.transform.SetParent(_owner.transform);
        this.transform.localPosition = new(0, -0.2f);
        this._hpMax = owner.Status[StatType.HpMax].Value;
        this._owner.Status[StatType.HpMax].OnChanged += SetMax;
        this._owner.OnChangedHp += Refresh;
        Refresh(this._owner.Hp);

        ResetInfo(duration);
    }
    public void ResetInfo(float duration = 1) {
        Color color = new(1, 1, 1, 1);
        GetImage((int)Images.Background).color = color;
        GetImage((int)Images.Fill).color = color;

        _duration = duration;
        if (_coDestroy != null) StopCoroutine(this._coDestroy);
        _coDestroy = StartCoroutine(CoDestroy());
    }

    private void SetMax(Stat stat) => this._hpMax = stat.Value;
    private void Refresh(float currentHp) {
        if (_hpMax == 0) return;
        GetSlider((int)Sliders.HpBar).value = currentHp / _hpMax;
    }

    private IEnumerator CoDestroy() {
        yield return new WaitForSeconds(_duration);

        float elapsed = 0;
        while (elapsed < 0.25f) {
            elapsed += Time.deltaTime;

            Color color = new(1, 1, 1, Mathf.Lerp(1, 0, elapsed / 0.25f));
            GetImage((int)Images.Background).color = color;
            GetImage((int)Images.Fill).color = color;

            yield return null;
        }

        _owner.HpBar = null;
        _owner.Status[StatType.HpMax].OnChanged -= SetMax;
        _owner.OnChangedHp -= Refresh;
        _coDestroy = null;
        Main.Resource.Destroy(this.gameObject);
    }
}