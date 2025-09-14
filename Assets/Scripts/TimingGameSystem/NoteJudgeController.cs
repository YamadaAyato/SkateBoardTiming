using UnityEngine;

/// <summary>
///         プレイヤー側のノーツ判定をつかさどるクラス
/// </summary>
public class NoteJudgeController : MonoBehaviour
{
    [SerializeField] private RectTransform _center;
    [SerializeField] private RectTransform _container;
    [SerializeField] private float _playerJudgeRadius = 90f;

    private RectTransform _rect;
    public float Angle { get; private set; }

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector2 local;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_container, Input.mousePosition, null, out local);

        // center の anchoredPosition を基準とする（
        Vector2 dir = (local - _center.anchoredPosition);
        if (dir.sqrMagnitude < 0.0001f) dir = Vector2.right;
        dir.Normalize();

        Vector2 anchored = _center.anchoredPosition + dir * _playerJudgeRadius;
        _rect.anchoredPosition = anchored;

        float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (ang < 0) ang += 360f;
        Angle = ang;
    }

    /// <summary>
    ///         外部から必要なら現在のUI位置を返す
    /// </summary>
    public Vector2 GetAnchoredPosition() => _rect.anchoredPosition;
}
