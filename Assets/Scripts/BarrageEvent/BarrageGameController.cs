using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

/// <summary>
///         連打ゲーム
/// </summary>
public class BarrageGameController : MonoBehaviour
{
    [SerializeField] private TMP_Text _barrageText;

    private int _count;
    private bool _isActive;

    public event Action<int> OnBarrageGameFinished;

    /// <summary>
    ///         連打ゲームスタート！！
    /// </summary>
    /// <param name="duration"></param>
    public void StartBarrage(float duration)
    {
        if (_isActive) return;
        _count = 0;
        _isActive = true;

        _barrageText.gameObject.SetActive(true);
        _barrageText.text = "連打しろ！！！！！";


        // duration後に強制終了
        Invoke(nameof(EndMash), duration);
    }

    /// <summary>
    ///         ゲーム終わり！！
    /// </summary>
    private void EndMash()
    {
        if (!_isActive) return;

        _isActive = false;
        _barrageText.gameObject.SetActive(false);

        OnBarrageGameFinished?.Invoke(_count);
    }

    private void Update()
    {
        if (!_isActive) return;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            _count++;
            AnimateText();
            _barrageText.text = _count.ToString();
        }
    }

    /// <summary>
    ///         アニメーション
    /// </summary>
    private void AnimateText()
    {
        _barrageText.transform.DOKill();
        _barrageText.transform.localScale = Vector3.one;
        _barrageText.transform.DOScale(1.1f, 0.1f).SetLoops(2, LoopType.Yoyo);
    }
}
