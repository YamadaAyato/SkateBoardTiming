//using System;
//using UnityEngine;

//namespace Player
//{
//    /// <summary>
//    ///         スケボーでスロープを移動するためのコントローラー
//    /// </summary>
//    public class RampMovementController : MonoBehaviour
//    {
//        [Header("ランプ移動")]
//        [SerializeField] private float _rampDuration = 1f;
//        [SerializeField] private AnimationCurve _speedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // 速度カーブ

//        [Header("地面検出設定")]
//        [SerializeField] private float _groundCheckDistance = 5f;
//        [SerializeField] private float _playerOffsetHeight = 0.5f;
//        [SerializeField] private LayerMask _groundLayer = -1;
//        [SerializeField] private bool _useGroundCorrection = true;
//        [SerializeField] private float _groundCorrectionSpeed = 1f;

//        public event Action OnRampPeak;
//        public event Action OnRampFinished;

//        private Transform _rampStart;
//        private Transform _rampPeak;
//        private Transform _rampEnd;
//        private float _rampTimer;
//        private bool _isOnRamp;
//        private bool _hasReachedPeak;
//        private bool _useGroundCorrectionAtEnd;
//        private bool _reverse;
//        private Vector3 _initPosition;

//        /// <summary>
//        ///         ランプ移動初期化メソッド
//        /// </summary>
//        public void StartRamp(Transform start, Transform peak, Transform end, bool reverse = false)
//        {
//            if (reverse)
//            {
//                _rampStart = end;
//                _rampEnd = start;
//            }
//            else
//            {
//                _rampStart = start;
//                _rampEnd = end;
//            }

//            _rampPeak = peak;
//            _reverse = reverse;
//            _rampTimer = 0f;
//            _isOnRamp = true;
//            _hasReachedPeak = false;
//            _initPosition = this.transform.position;

//            Debug.Log($"ランプ開始: Start={start.position}, Peak={peak.position}, End={end.position}");
//        }

//        private void Update()
//        {
//            if (_isOnRamp) RampMovement();
//        }

//        /// <summary>
//        ///         ランプ移動
//        /// </summary>
//        private void RampMovement()
//        {
//            _rampTimer += Time.deltaTime;
//            float rawT = _rampTimer / _rampDuration;  // ランプ上の進行度を計算
//            float t = Mathf.Clamp01(rawT);  // 割合なので0～1で制御

//            // 速度カーブを適用（より自然な動き）
//            float curveT = _speedCurve.Evaluate(t);

//            //　ベジェ公式
//            //  B(t)=(1−t)^2P0 ​+ 2(1−t)tP1 ​+ t^2P2​,
//            Vector3 bezierPos = Mathf.Pow(1 - curveT, 2) * _rampStart.position +
//                              2 * (1 - curveT) * curveT * _rampPeak.position +
//                              Mathf.Pow(curveT, 2) * _rampEnd.position;

//            //  ベジェ曲線の接線
//            //  B′(t)=2(1 − t)(P1 − P0​) + 2t(P2 ​− P1​)
//            Vector3 tangent = 2 * (1 - curveT) * (_rampPeak.position - _rampStart.position) +
//                              2 * curveT * (_rampEnd.position - _rampPeak.position);

//            Vector3 targetPos = _useGroundCorrection ? GetGroundCorrectedPosition(bezierPos, t) : bezierPos;

//            transform.position = targetPos;
//            transform.rotation = Quaternion.LookRotation(tangent);

//            //  中間地点到達判定
//            if (!_hasReachedPeak && curveT >= 0.5f)
//            {
//                _hasReachedPeak = true;
//                OnRampPeak?.Invoke();
//                Debug.Log("ランプのピークに到達");
//            }

//            //  ランプ終了判定
//            if (t >= 1f)
//            {
//                _isOnRamp = false;
//                Vector3 finalPos = (_useGroundCorrection && _useGroundCorrectionAtEnd) ?
//                    GetGroundCorrectedPosition(_rampEnd.position, 1f) : _rampEnd.position;
//                transform.position = finalPos;
//                OnRampFinished?.Invoke();
//                Debug.Log($"ランプの終点に到達 -最終位置{this.transform.position} -エンドポイント{_rampEnd.position}");
//            }
//        }

//        /// <summary>
//        ///         地面検出して位置補正
//        /// </summary>
//        /// <param name="originalPos"></param>
//        /// <param name="t"></param>
//        /// <returns></returns>
//        private Vector3 GetGroundCorrectedPosition(Vector3 originalPos, float t)
//        {
//            Vector3 rayStart = originalPos + Vector3.up * _groundCheckDistance;
//            Ray groundCheckRay = new Ray(rayStart, Vector3.down);

//            if (Physics.Raycast(groundCheckRay, out RaycastHit hit, _groundCheckDistance * 2f, _groundLayer))
//            {
//                Vector3 groundPos = hit.point + Vector3.up * _playerOffsetHeight;

//                // 地面補正後の位置が元の位置より大幅に低い場合は補正を制限
//                float heightDifference = originalPos.y - groundPos.y;
//                if (heightDifference > 2f) // 2ユニット以上低い場合は補正を無視
//                {
//                    Debug.Log($"地面補正をスキップ: 高度差 {heightDifference:F2}");
//                    return originalPos;
//                }
//                if (_groundCorrectionSpeed > 0 && t < 1f)
//                {
//                    // ランプ開始直後は初期位置を基準にして、徐々に地面補正を適用
//                    Vector3 basePos = t < 0.1f ? _initPosition : transform.position;
//                    return Vector3.Lerp(basePos, groundPos, Time.deltaTime * _groundCorrectionSpeed);
//                }
//                else
//                {
//                    return groundPos;
//                }
//            }

//            // 地面が検出されなかった場合は元の位置を返す
//            return originalPos;
//        }

//        private void OnDrawGizmos()
//        {
//            if (_rampStart != null && _rampPeak != null && _rampEnd != null)
//            {
//                // ベジェ曲線の描画
//                Gizmos.color = Color.yellow;
//                Vector3 prevPoint = _rampStart.position;

//                for (int i = 1; i <= 20; i++)
//                {
//                    float t = i / 20f;
//                    float oneMinusT = 1 - t;
//                    Vector3 point = oneMinusT * oneMinusT * _rampStart.position +
//                                   2 * oneMinusT * t * _rampPeak.position +
//                                   t * t * _rampEnd.position;

//                    Gizmos.DrawLine(prevPoint, point);
//                    prevPoint = point;
//                }

//                // 制御点の描画
//                Gizmos.color = Color.red;
//                Gizmos.DrawSphere(_rampStart.position, 0.2f);
//                Gizmos.color = Color.green;
//                Gizmos.DrawSphere(_rampPeak.position, 0.2f);
//                Gizmos.color = Color.blue;
//                Gizmos.DrawSphere(_rampEnd.position, 0.2f);

//                // 制御線の描画
//                Gizmos.color = Color.gray;
//                Gizmos.DrawLine(_rampStart.position, _rampPeak.position);
//                Gizmos.DrawLine(_rampPeak.position, _rampEnd.position);
//            }
//        }
//    }
//}
