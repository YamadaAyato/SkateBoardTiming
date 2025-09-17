using System.Collections.Generic;
using UnityEngine;

/// <summary>
///         ƒ‰ƒ“ƒLƒ“ƒOŠÇ—
/// </summary>
public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance { get; private set; }

    private List<(string playerName, int score)> _ranking = new List<(string, int)>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadRanking();
    }

    public void AddNewScore(string name, int score)
    {
        _ranking.Add((name, score));
        _ranking.Sort((a, b) => b.score.CompareTo(a.score));

        // Å‘å5Œ‚Ü‚Å•Û
        if (_ranking.Count > 5)
            _ranking.RemoveAt(_ranking.Count - 1);

        SaveRanking();
    }

    public List<(string playerName, int score)> GetRanking()
    {
        return _ranking;
    }

    private void SaveRanking()
    {
        for (int i = 0; i < _ranking.Count; i++)
        {
            PlayerPrefs.SetString($"Ranking_Name_{i}", _ranking[i].playerName);
            PlayerPrefs.SetInt($"Ranking_Score_{i}", _ranking[i].score);
        }
        PlayerPrefs.SetInt("Ranking_Count", _ranking.Count);
        PlayerPrefs.Save();
    }

    private void LoadRanking()
    {
        _ranking.Clear();
        int count = PlayerPrefs.GetInt("Ranking_Count", 0);

        for (int i = 0; i < count; i++)
        {
            string name = PlayerPrefs.GetString($"Ranking_Name_{i}", "???");
            int score = PlayerPrefs.GetInt($"Ranking_Score_{i}", 0);
            _ranking.Add((name, score));
        }
    }
}
