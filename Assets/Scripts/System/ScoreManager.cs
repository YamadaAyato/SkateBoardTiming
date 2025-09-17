using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///         スコア & コンボ管理クラス
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [Header("UI参照")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _comboText;

    [Header("判定スコア")]
    [SerializeField] private int _perfectScore = 1000;
    [SerializeField] private int _greatScore = 500;
    [SerializeField] private int _missPenalty = 0;

    [Header("コンボ補正")]
    [SerializeField] private float _comboMultiplier = 0.1f;

    private int _currentScore = 0;
    private int _comboCount = 0;

    private void Start()
    {
        UpdateUi();
    }

    /// <summary>
    ///         判定結果を受け取ってスコア & コンボ加算
    /// </summary>
    public void AddScore(string result)
    {
        int baseScore = 0;

        switch (result)
        {
            case "Perfect":
                baseScore = _perfectScore;
                AddCombo();
                break;
            case "Great":
                baseScore = _greatScore;
                AddCombo();
                break;
            case "Miss":
                baseScore = _missPenalty;
                ResetCombo();
                break;
        }

        float multiplier = 1f + (_comboCount * _comboMultiplier);
        int finalScore = Mathf.RoundToInt(baseScore * multiplier);

        _currentScore += finalScore;

        UpdateUi();
        Debug.Log($"判定:{result}, 加算:{finalScore}, 累計スコア:{_currentScore}, コンボ:{_comboCount}");
    }

    /// <summary>
    ///         コンボ加算
    /// </summary>
    private void AddCombo()
    {
        _comboCount++;
    }

    /// <summary>
    ///         コンボリセット
    /// </summary>
    private void ResetCombo()
    {
        _comboCount = 0;
    }

    /// <summary>
    ///         連打ゲームスコア加算
    /// </summary>
    /// <param name="mashCount"></param>
    public void AddMashScore(int mashCount)
    {
        int baseScore = 0;

        if (mashCount < 10)
        {
            baseScore = 100;
        }
        else if (mashCount >= 60 && mashCount < 80)
        {
            baseScore = 1000;
        }
        else
        {
            baseScore = mashCount * 10;
        }

        _currentScore += baseScore;
        UpdateUi();

        Debug.Log($"Mash結果: {mashCount}回 → {baseScore}点, 累計スコア:{_currentScore}");
    }

    /// <summary>
    ///         UI更新
    /// </summary>
    private void UpdateUi()
    {
        if (_scoreText != null)
            _scoreText.text = $"スコア : {_currentScore}";

        if (_comboText != null)
            _comboText.text = _comboCount > 0 ? $"{_comboCount}コンボ!!" : "";
    }

    /// <summary>
    ///         最終スコア取得
    /// </summary>
    /// <returns></returns>
    public int GetFinalScore() => _currentScore;
}
