using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    ///   スケボーでスロープを移動するためのコントローラー
    /// </summary>
    public class RampMovementController : MonoBehaviour
    {
        [Header("ランプ移動")]
        [SerializeField] private float _rampDuration = 1f; 

        public event Action OnRampPeak;
        public event Action OnRampFinished;

        private Transform _rampStart; 
        private Transform _rampPeak; 
        private Transform _rampEnd;
        private float _rampTimer;
        private bool _isOnRamp;
        private bool _hasReachedpeak;

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
            _hasReachedpeak = false;
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
            float t = _rampTimer / _rampDuration;  // ランプ上の進行度を計算
            Mathf.Clamp01(t);  // 割合なので0～1で制御

            //　ベジェ公式
            //  B(t)=(1−t)^2P0 ​+ 2(1−t)tP1 ​+ t^2P2​,
            Vector3 pos = Mathf.Pow(1 - t, 2) * _rampStart.position +
                              2 * (1 - t) * t * _rampPeak.position +
                              Mathf.Pow(t, 2) * _rampEnd.position;

            //  ベジェ曲線の接点
            //  B′(t)=2(1 − t)(P1 − P0​) + 2t(P2 ​− P1​)
            Vector3 tangent = 2 * (1 - t) * (_rampPeak.position - _rampStart.position) +
                              2 * t * (_rampEnd.position - _rampPeak.position);

            transform.position = pos;
            transform.rotation = Quaternion.LookRotation(tangent);

            //  中間地点到達判定
            if (!_hasReachedpeak && Mathf.Abs(t - 0.5f) < 0.02f)
            {
                _hasReachedpeak = true;
                OnRampPeak?.Invoke();
            }

            //  ランプ終了判定
            if (t >= 1f)
            {
                _isOnRamp = false;
                OnRampFinished?.Invoke();
            }
        }
    }
}
