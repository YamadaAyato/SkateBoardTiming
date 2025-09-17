/// <summary>
///         名前とスコアを保存するためのクラス
/// </summary>
[System.Serializable]
public class ScoreEntry
{
    public string playerName;
    public int score;

    public ScoreEntry(string name, int score)
    {
        this.playerName = name;
        this.score = score;
    }
}
