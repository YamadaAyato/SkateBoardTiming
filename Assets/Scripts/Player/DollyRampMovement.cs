using Unity.Cinemachine;
using UnityEngine;
using System;

/// <summary>
///         スケボーでスロープを移動するためのコントローラー
/// </summary>
public class DollyRampMovement : MonoBehaviour
{
    [Header("ランプ移動設定")]
    [SerializeField] private CinemachineSplineCart _dollyCart;
    [SerializeField] private float _rampDuration = 2f;
    [SerializeField] private AnimationCurve _speedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public event Action OnRampPeak;
    public event Action OnRampFinished;

    private float _timer;
    private bool _isMoving;
    private bool _hasReachedPeak;

    private void Update()
    {
        if (_isMoving)
        {
            RampMovement();
        }
    }

    private void RampMovement()
    {
        _timer += Time.deltaTime;
        // ランプ上の進行度を計算
        float t = Mathf.Clamp01(_timer / _rampDuration);

        float curveT = _speedCurve.Evaluate(t);

        // スプライン上の正規化された位置を設定
        _dollyCart.SplinePosition = curveT;

        //  中間地点到達判定
        if (!_hasReachedPeak && t >= 0.5f)
        {
            _hasReachedPeak = true;
            OnRampPeak?.Invoke();
            Debug.Log("ランプのピークに到達");
        }

        //  ランプ終了判定
        if (t >= 1f)
        {
            _isMoving = false;
            OnRampFinished?.Invoke();
            Debug.Log($"ランプの終点に到達 -最終位置{this.transform.position}");
        }
    }

}
