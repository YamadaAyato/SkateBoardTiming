using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///         ノーツ判定結果を画面に表示するクラス
/// </summary>
public class JudgeResultDisplay : MonoBehaviour
{
    [Header("判定表示設定")]
    [SerializeField] private GameObject _judgeTextPrefab;
    [SerializeField] private Canvas _displayCanvas;
    [SerializeField] private float _displayDuration = 1.0f;
    [SerializeField] private float _moveDistance = 50f;
    [SerializeField] private AnimationCurve _fadeCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));
    [SerializeField] private AnimationCurve _moveCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

    [Header("判定別色設定")]
    [SerializeField] private Color _perfectColor = Color.yellow;
    [SerializeField] private Color _greatColor = Color.green;
    [SerializeField] private Color _missColor = Color.red;

    private Camera _uiCamera;

    private void Start()
    {
        _uiCamera = _displayCanvas?.worldCamera ?? Camera.main;
    }

    /// <summary>
    ///         判定結果を指定位置に表示
    /// </summary>
    /// <param name="judgeResult">判定結果（Perfect, Great, Miss）</param>
    /// <param name="worldPosition">/param>
    public void ShowJudgeResult(string judgeResult, Vector3 worldPosition)
    {
        if (_judgeTextPrefab == null || _displayCanvas == null) return;

        GameObject judgeObj = Instantiate(_judgeTextPrefab, _displayCanvas.transform);

        // 世界座標をスクリーン座標に変換してUI位置を設定
        Vector3 screenPos = _uiCamera.WorldToScreenPoint(worldPosition);
        RectTransform rectTransform = judgeObj.GetComponent<RectTransform>();

        if (rectTransform != null)
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _displayCanvas.transform as RectTransform, screenPos, _uiCamera, out localPos);
            rectTransform.anchoredPosition = localPos;
        }

        var tmpComponent = judgeObj.GetComponent<TMPro.TextMeshProUGUI>();
        if (tmpComponent != null)
        {
            tmpComponent.text = judgeResult;
            tmpComponent.color = GetJudgeColor(judgeResult);
        }

        Text textComponent = judgeObj.GetComponent<Text>();
        if (textComponent != null)
        {
            textComponent.text = judgeResult;
            textComponent.color = GetJudgeColor(judgeResult);
        }

        StartCoroutine(AnimateJudgeText(judgeObj, rectTransform));
    }

    /// <summary>
    ///         判定結果に応じた色を取得
    /// </summary>
    private Color GetJudgeColor(string judgeResult)
    {
        switch (judgeResult.ToLower())
        {
            case "perfect":
                return _perfectColor;
            case "great":
                return _greatColor;
            case "miss":
                return _missColor;
            default:
                return Color.white;
        }
    }

    /// <summary>
    ///         判定テキストのアニメーション
    /// </summary>
    private IEnumerator AnimateJudgeText(GameObject judgeObj, RectTransform rectTransform)
    {
        Vector2 startPos = rectTransform.anchoredPosition;
        Vector2 endPos = startPos + Vector2.up * _moveDistance;

        CanvasGroup canvasGroup = judgeObj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = judgeObj.AddComponent<CanvasGroup>();

        float elapsed = 0f;

        while (elapsed < _displayDuration)
        {
            float t = elapsed / _displayDuration;

            if (rectTransform != null)
            {
                float moveT = _moveCurve.Evaluate(t);
                rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, moveT);
            }

            if (canvasGroup != null)
                canvasGroup.alpha = _fadeCurve.Evaluate(t);

            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        if (judgeObj != null)
            Destroy(judgeObj);
    }
}