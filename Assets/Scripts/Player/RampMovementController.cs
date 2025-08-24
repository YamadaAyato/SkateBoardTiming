using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    ///         スケボーでスロープを移動するためのコントローラー
    /// </summary>
    public class RampMovementController : MonoBehaviour
    {
        [Header("ランプ移動")]
        [SerializeField] private float _rampDuration = 1f;
        [SerializeField] private AnimationCurve _speedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // 速度カーブ

        public event Action OnRampPeak;
        public event Action OnRampFinished;

        private Transform _rampStart;
        private Transform _rampPeak;
        private Transform _rampEnd;
        private float _rampTimer;
        private bool _isOnRamp;
        private bool _hasReachedPeak;

        /// <summary>
        ///         ランプ移動初期化メソッド
        /// </summary>
        public void StartRamp(Transform start, Transform peak, Transform end)
        {
            _rampStart = start;
            _rampPeak = peak;
            _rampEnd = end;
            _rampTimer = 0f;
            _isOnRamp = true;
            _hasReachedPeak = false;

            Debug.Log($"ランプ開始: Start={start.position}, Peak={peak.position}, End={end.position}");
        }

        private void Update()
        {
            if (_isOnRamp) RampMovement();
        }

        /// <summary>
        ///         ランプ移動
        /// </summary>
        private void RampMovement()
        {
            _rampTimer += Time.deltaTime;
            float rawT = _rampTimer / _rampDuration;  // ランプ上の進行度を計算
            float t = Mathf.Clamp01(rawT);  // 割合なので0～1で制御

            // 速度カーブを適用（より自然な動き）
            float curveT = _speedCurve.Evaluate(t);

            //　ベジェ公式
            //  B(t)=(1−t)^2P0 ​+ 2(1−t)tP1 ​+ t^2P2​,
            Vector3 targetPos = Mathf.Pow(1 - curveT, 2) * _rampStart.position +
                              2 * (1 - curveT) * curveT * _rampPeak.position +
                              Mathf.Pow(curveT, 2) * _rampEnd.position;

            //  ベジェ曲線の接点
            //  B′(t)=2(1 − t)(P1 − P0​) + 2t(P2 ​− P1​)
            Vector3 tangent = 2 * (1 - curveT) * (_rampPeak.position - _rampStart.position) +
                              2 * curveT * (_rampEnd.position - _rampPeak.position);

            transform.position = targetPos;
            transform.rotation = Quaternion.LookRotation(tangent);

            //  中間地点到達判定
            if (!_hasReachedPeak && Mathf.Abs(curveT - 0.5f) < 0.02f)
            {
                _hasReachedPeak = true;
                OnRampPeak?.Invoke();
                Debug.Log("ランプのピークに到達");
            }

            //  ランプ終了判定
            if (curveT >= 1f)
            {
                _isOnRamp = false;
                transform.position = _rampEnd.position;
                OnRampFinished?.Invoke();
                Debug.Log("ランプの終点に到達");
            }
        }

        private void OnDrawGizmos()
        {
            if (_rampStart != null && _rampPeak != null && _rampEnd != null)
            {
                // ベジェ曲線の描画
                Gizmos.color = Color.yellow;
                Vector3 prevPoint = _rampStart.position;

                for (int i = 1; i <= 20; i++)
                {
                    float t = i / 20f;
                    float oneMinusT = 1 - t;
                    Vector3 point = oneMinusT * oneMinusT * _rampStart.position +
                                   2 * oneMinusT * t * _rampPeak.position +
                                   t * t * _rampEnd.position;

                    Gizmos.DrawLine(prevPoint, point);
                    prevPoint = point;
                }

                // 制御点の描画
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_rampStart.position, 0.2f);
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_rampPeak.position, 0.2f);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(_rampEnd.position, 0.2f);

                // 制御線の描画
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(_rampStart.position, _rampPeak.position);
                Gizmos.DrawLine(_rampPeak.position, _rampEnd.position);
            }
        }
    }
}
