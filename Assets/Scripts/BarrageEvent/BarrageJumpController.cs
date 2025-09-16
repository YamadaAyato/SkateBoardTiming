using System;
using System.Collections;
using UnityEngine;

/// <summary>
///         連打ゲームのジャンプ
/// </summary>
public class BarrageJumpController : MonoBehaviour
{
    [SerializeField] private float _jumpHeight = 30f;
    [SerializeField] public float _jumpDuration = 10f;

    private Rigidbody _rb;
    private bool _isJumping = false;

    public event Action OnBarrageJumpStarted;
    public event Action OnBarrageJumpFinished;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    ///         連打ゲームスタート！！
    /// </summary>
    /// <param name="targetPos"></param>
    public void StartBarrageJump(Vector3 targetPos)
    {
        if (_isJumping) return;
        StartCoroutine(BarrageJump(targetPos));
    }

    /// <summary>
    ///         連打ゲームのジャーンプ！！
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    private IEnumerator BarrageJump(Vector3 targetPos)
    {
        _isJumping = true;
        OnBarrageJumpStarted?.Invoke();

        Vector3 start = _rb.position;
        float elapsed = 0f;

        while (elapsed < _jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _jumpDuration);

            Vector3 pos = Vector3.Lerp(start, targetPos, t);
            pos.y = _jumpHeight * Mathf.Sin(Mathf.PI * t);
            _rb.MovePosition(pos);

            Vector3 dir = (targetPos - _rb.position).normalized;
            if (dir.sqrMagnitude > 0.01f)
            {
                Quaternion rot = Quaternion.LookRotation(dir);
                _rb.MoveRotation(rot);
            }

            yield return null;
        }
        _isJumping = false;
        OnBarrageJumpFinished?.Invoke();
    }
}
