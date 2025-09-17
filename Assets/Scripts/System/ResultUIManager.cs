using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIManager : MonoBehaviour
{
    [Header("UI参照")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private Button _submitButton;
    [SerializeField] private Transform _rankingPanel;
    [SerializeField] private TMP_Text _rankingEntryPrefab;

    private int _finalScore = 0;

    private void Start()
    {
        _finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        _scoreText.text = $"スコア : {_finalScore}";

        _submitButton.onClick.AddListener(OnSubmit);

        ShowRanking();
    }

    private void OnSubmit()
    {
        string playerName = _nameInput.text;

        if (string.IsNullOrEmpty(playerName))
        {
            Debug.Log("名前が未入力です");
            return;
        }

        RankingManager.Instance.AddNewScore(playerName, _finalScore);
        ShowRanking();
    }

    private void ShowRanking()
    {
        foreach (Transform child in _rankingPanel)
            Destroy(child.gameObject);

        var ranking = RankingManager.Instance.GetRanking();
        for (int i = 0; i < ranking.Count; i++)
        {
            TMP_Text entry = Instantiate(_rankingEntryPrefab, _rankingPanel);
            entry.text = $"{i + 1}位: {ranking[i].playerName} - {ranking[i].score}";
        }
    }
}
