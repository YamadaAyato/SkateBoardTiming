using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

/// <summary>
///         ジャンプのゲージを判定するクラス   
/// </summary>
public class JumpGauge : MonoBehaviour
{
    [Header("UI系")]
    [SerializeField] private Slider _slider;
    [SerializeField] private float _slideSpeed = 1f;
    [SerializeField] private TMP_Text _resultText;
    [SerializeField] private float _resultDisplayTime = 1f;

    [Header("判定")]
    [SerializeField] private float _perfectThreshold = 0.9f;
    [SerializeField] private float _greatThreshold = 0.75f;
    [SerializeField] private float _goodthreshold = 0.5f;

    public event Action<string> OnGuageResult;

    private float _value;
    private bool _isPlaying;

    /// <summary>
    ///         ゲージ判定を始める
    /// </summary>
    public void StartJumpGauge()
    {
        _isPlaying = true;
        _value = 0f;
        _slider.value = 0f;
        _slider?.gameObject.SetActive(true);
    }

    /// <summary>
    ///         時間切れ等強制停止用メソッド
    /// </summary>
    public void CancelGuage()
    {
        if (!_isPlaying) return;
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
        if (_value >= 1f) _value = 0f;

        _slider.value = _value;

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            SEManager.Instance?.Play("SF決定音1", 0.8f);
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
        ShowResult(result);
    }

    /// <summary>
    ///         DOTweenで判定結果をフェード表示
    /// </summary>
    private void ShowResult(string result)
    {
        if (_resultText == null) return;

        _resultText.gameObject.SetActive(true);
        _resultText.DOKill();

        _resultText.text = result;
        switch (result)
        {
            case "Perfect": _resultText.color = new Color(1f, 1f, 0f, 0f); break;
            case "Great": _resultText.color = new Color(0f, 1f, 0f, 0f); break;
            case "Good": _resultText.color = new Color(0f, 0.5f, 1f, 0f); break;
            case "Miss": _resultText.color = new Color(1f, 0f, 0f, 0f); break;
        }

        Sequence seq = DOTween.Sequence();
        seq.Append(_resultText.DOFade(1f, 0.2f))
           .AppendInterval(_resultDisplayTime)
           .Append(_resultText.DOFade(0f, 0.5f))
           .OnComplete(() => _resultText.gameObject.SetActive(false));
    }

}