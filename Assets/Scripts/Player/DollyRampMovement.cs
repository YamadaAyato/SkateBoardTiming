using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
///         スケボーでスロープを移動するためのコントローラー
/// </summary>
public class DollyRampMovement : MonoBehaviour
{
    [Header("ランプ移動設定")]
    [SerializeField] private float _rampDuration = 2f;
    [SerializeField] private AnimationCurve _speedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    public event Action OnRampPeak;
    public event Action OnRampFinished;

    private CinemachineSplineCart _currentCart;
    private SplineContainer _currentSpline;
    private float _splineLength;
    private float _timer;
    private bool _isMoving;
    private bool _hasReachedPeak;
    private bool _reverse;

    /// <summary>
    /// 指定ランプを開始する
    /// </summary>
    public void StartRamp(CinemachineSplineCart splineCart, bool reverse = false)
    {
        _currentCart = splineCart;
        _currentSpline = splineCart.Spline;
        _splineLength = _currentSpline.CalculateLength(); // spline 全体の長さ（m）

        _reverse = reverse;
        _timer = 0f;
        _isMoving = true;
        _hasReachedPeak = false;

        // 開始地点をセット
        _currentCart.SplinePosition = _reverse ? 1f : 0f;

        Debug.Log($"Ramp開始: {_currentCart.gameObject.name}, reverse={_reverse}, length={_splineLength}");
    }

    private void Update()
    {
        if (_isMoving && _currentCart != null)
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

        //// spline 上の距離を計算
        //float distance = curveT * _splineLength;
        _currentCart.SplinePosition = _reverse ? (1f - curveT) : curveT;

        // プレイヤーをCartの位置・回転に追従
        transform.position = _currentCart.transform.position;
        transform.rotation = _currentCart.transform.rotation;

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
