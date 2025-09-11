using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///         ジャンプのゲージを判定するクラス   
/// </summary>
public class JumpGauge : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _sliderSpeed = 1f;

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
        _value += _sliderSpeed * Time.fixedDeltaTime;
        if(_value >= 1f)_value = 0f;

        _slider.value = _value;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Judge(_value);
            _isPlaying = false;
        }
    }

    /// <summary>
    ///         タイミングの判定
    /// </summary>
    /// <param name="value"></param>
    private void Judge(float value)
    {
        string result;

        if (value > 0.9f) result = "Perfect";
        else if (value > 0.75f) result = "Great";
        else if (value > 0.5f) result = "Good";
        else result = "Miss";

        Debug.Log($"ゲージ判定; {result}({value * 100:F0}%)");

        OnGuageResult?.Invoke(result);
    }
}
