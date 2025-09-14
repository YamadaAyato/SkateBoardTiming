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

    private void UpdateUi()
    {
        if (_scoreText != null)
            _scoreText.text = $"Score : {_currentScore}";

        if (_comboText != null)
            _comboText.text = _comboCount > 0 ? $"Combo : {_comboCount}" : "";
    }
}
