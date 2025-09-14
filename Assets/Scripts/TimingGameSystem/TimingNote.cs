using System;
using UnityEngine;

public class TimingNote : MonoBehaviour
{
    private RectTransform _rect;
    private Vector2 _startAnchored;
    private Vector2 _targetAnchored;
    private float _duration;
    private float _timer;
    private bool _active;

    public float Angle { get; private set; } // degrees (0..360)

    public event Action<TimingNote> OnReached;

    /// <summary>
    ///         ノーツ初期化＆設定
    /// </summary>
    public void Init(RectTransform center, RectTransform container,
                     float radius, float angleDeg, float travelTime)
    {
        _rect = GetComponent<RectTransform>();
        _rect.SetParent(container, false);

        // anchoredPosition を基準に動かす + 正規化
        _startAnchored = center.anchoredPosition;
        Angle = angleDeg % 360f;
        if (Angle < 0) Angle += 360f;

        float rad = Angle * Mathf.Deg2Rad;
        _targetAnchored = _startAnchored
            + new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;

        _rect.anchoredPosition = _startAnchored;
        _duration = Mathf.Max(0.001f,travelTime);
        _timer = 0f;
        _active = true;
    }

    private void Update()
    {
        if (!_active) return;

        _timer += Time.unscaledDeltaTime;
        float t = Mathf.Clamp01(_timer / _duration);
        _rect.anchoredPosition = Vector2.Lerp(_startAnchored, _targetAnchored, t);

        if(t>= 1f)
        {
            _active = false;
            OnReached?.Invoke(this);
        }
    }
}
