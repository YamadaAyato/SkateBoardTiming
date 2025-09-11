using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///         ジャンプのゲージを判定するクラス   
/// </summary>
public class JumpGauge : MonoBehaviour
{
    [Header("UI系")]
    [SerializeField] private Slider _slider;
    [SerializeField] private float _slideSpeed = 1f;

    [Header("判定")]
    [SerializeField] private float _perfectThreshold = 0.9f;
    [SerializeField] private float _greatThreshold = 0.75f;
    [SerializeField] private float _goodthreshold = 0.5f;

    public event Action<string> OnGuageResult;

    private float _value;
    private bool _isPlaying = false;

    /// <summary>
    ///         ゲージ判定を始める
    /// </summary>
    public void StartJumpGauge()
    {
        _isPlaying = true;
        _slider.value = 0f;
        _slider?.gameObject.SetActive(true);
    }

    /// <summary>
    ///         時間切れ等強制停止用メソッド
    /// </summary>
    public void CancelGuage()
    {
        if(!_isPlaying)return;
        _isPlaying = false;
        OnGuageResult?.Invoke("Miss");
        _slider?.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isPlaying) SliderMove();
    }

    /// <summary>
    ///         UIのスライダーを動かす
    /// </summary>
    private void SliderMove()
    {
        _value += _slideSpeed * Time.unscaledDeltaTime;
        if(_value >= 1f)_value = 0f;

        _slider.value = _value;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Judge(_value);
            _isPlaying = false;
            _slider?.gameObject.SetActive(false);
        }
    }

    /// <summary>
    ///         タイミングの判定
    /// </summary>
    /// <param name="value"></param>
    private void Judge(float value)
    {
        string result;

        if (value > _perfectThreshold) result = "Perfect";
        else if (value > _greatThreshold) result = "Great";
        else if (value > _goodthreshold) result = "Good";
        else result = "Miss";

        Debug.Log($"ゲージ判定; {result}({value * 100:F0}%)");

        OnGuageResult?.Invoke(result);
    }
}
