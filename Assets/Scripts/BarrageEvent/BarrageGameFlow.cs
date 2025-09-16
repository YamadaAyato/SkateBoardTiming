using UnityEngine;

public class BarrageGameFlow : MonoBehaviour
{
    [SerializeField] private BarrageGameController _barGameCon;
    [SerializeField] private BarrageJumpController _barJumpCon;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private Transform _jumpTarget;

    private void Start()
    {
        _barJumpCon.OnBarrageJumpStarted += () =>
        {
            Debug.Log("ジャンプ開始 → 連打ゲーム開始！！");
            _barGameCon.StartBarrage(_barJumpCon._jumpDuration);
        };

        _barJumpCon.OnBarrageJumpFinished += () =>
        Debug.Log("ジャンプ終了");

        _barGameCon.OnBarrageGameFinished += (count) =>
        {
            _scoreManager.AddMashScore(count);
            // 後でリザルトかなんか追加
        };
    }

    public void StartFinalJump()
    {
        _barJumpCon.StartBarrageJump(_jumpTarget.position);
    }
}
