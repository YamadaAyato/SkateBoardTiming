using UnityEngine;

public class JumpTimingGameManager : MonoBehaviour
{
    [SerializeField] private float _noteDiameter = 1.5f;

    public void StartTimingGame(float jumpDuration)
    {
        int noteCount = Mathf.CeilToInt(jumpDuration * _noteDiameter);
        Debug.Log($"ジャンプ開始{jumpDuration:F1}秒　→　ノーツ{noteCount}個");


    }
}
