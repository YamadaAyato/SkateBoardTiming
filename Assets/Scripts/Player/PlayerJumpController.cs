using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    /// <summary>
    ///        プレイヤーのジャンプ制御
    /// </summary>
    public class PlayerJumpController : MonoBehaviour
    {
        [Header("ジャンプの高さ")]
        [SerializeField] private float _baseHeight = 3f;
        [SerializeField] private float _goodHeight = 6f;
        [SerializeField] private float _greatHeight = 9f;
        [SerializeField] private float _perfectHeight = 12f;

        [Header("ジャンプの長さ")]
        [SerializeField] private float _baseJumpDuration = 3f;
        [SerializeField] private float _goodJumpDuration = 4f;
        [SerializeField] private float _greatJumpDuration = 5f;
        [SerializeField] private float _perfectJumpDuration = 6f;

        [Header("スローモーション設定")]
        [SerializeField] private float _slowTimeScale = 0.2f;
        [SerializeField] private float _slowDuration = 2f;

        [Header("ジャンプ回転")]
        [SerializeField] private int _rotAngle = 180;

        [Header("参照")]
        [SerializeField] private JumpGauge _jumpGauge;
        [SerializeField] private JumpTimingGameManager _timingGame;

        public event Action OnJumpFinished;
        public event Action<string> OnJumpEvaluated;

        private bool _isJumping;
        private Vector3 _startPos;

        /// <summary>
        ///         ジャンプスタート！！
        /// </summary>
        public void StartJump(Transform targetPoint = null)
        {
            if (_isJumping) return;
            StartCoroutine(JumpRoutine(targetPoint));
        }

        /// <summary>
        ///         判定スタートとタイムスケールの管理
        /// </summary>
        /// <param name="targetPoint"></param>
        /// <returns></returns>
        private IEnumerator JumpRoutine(Transform targetPoint)
        {
            _isJumping = true;
            _startPos = transform.position;

            Time.timeScale = _slowTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            //  スロー演出追加予定

            _jumpGauge.StartJumpGauge();
            yield return new WaitForSecondsRealtime(_slowDuration);

            //  スロー解除
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }

        /// <summary>
        ///         判定結果の出力
        /// </summary>
        /// <param name="result"></param>
        private void OnGaugeResult(string result)
        {
            Debug.Log("判定結果" + result);

            OnJumpEvaluated?.Invoke(result);

            float jumpHeight = _baseHeight;
            float jumpDuration = _baseJumpDuration;

            switch (result)
            {
                case "Good":
                    jumpHeight = _goodHeight;
                    jumpDuration = _goodJumpDuration;
                    break;
                case "Great":
                    jumpHeight = _greatHeight;
                    jumpDuration = _greatJumpDuration;
                    break;
                case "Perfect":
                    jumpHeight = _perfectHeight;
                    jumpDuration = _perfectJumpDuration;
                    break;
                default:
                    jumpHeight = _baseHeight;
                    jumpDuration = _baseJumpDuration;
                    break;
            }

            if (_timingGame != null)
            {
                _timingGame.StartTimingGame(jumpDuration);
            }

            StartCoroutine(JumpMovement(jumpHeight, jumpDuration));
        }

        /// <summary>
        ///         ジャンプ処理
        /// </summary>
        /// <returns></returns>
        private IEnumerator JumpMovement(float jumpHeight, float jumpDuration, Transform targetPoint = null)
        {
            Vector3 endPos = targetPoint != null ? targetPoint.position : _startPos;

            float timer = 0f;
            Quaternion initRot = transform.rotation;
            Quaternion endRot = initRot * Quaternion.Euler(0, _rotAngle, 0);
            float jumpRotStart = 0.3f;
            float jumpRotEnd = 0.7f;

            while (timer < jumpDuration)
            {
                timer += Time.deltaTime;
                float t = timer / jumpDuration;

                // 始点→終点の水平補間
                Vector3 horizontal = Vector3.Lerp(_startPos, endPos, t);

                //  放物線移動
                float yOffset = Mathf.Sin(Mathf.PI * t) * jumpHeight;
                transform.position = _startPos
                    + new Vector3(0, horizontal.y + yOffset, 3);

                if (t >= jumpRotStart && t <= jumpRotEnd)
                {
                    float currentRot = Mathf.InverseLerp(jumpRotStart, jumpRotEnd, t);
                    transform.rotation = Quaternion.Lerp(initRot, endRot, currentRot);
                }
                yield return null;
            }

            transform.position = new Vector3(endPos.x, endPos.y + 0.5f, endPos.z);
            transform.rotation = endRot;

            _isJumping = false;
            Debug.Log("ジャンプ完了");

            OnJumpFinished?.Invoke();
        }

        /// <summary>
        ///         デバッグ用のタイミング判定
        /// </summary>
        /// <returns></returns>
        private string DebugGetRandomResult()
        {
            int r = UnityEngine.Random.Range(0, 4);
            return r switch
            {
                0 => "Miss",
                1 => "Good",
                2 => "Great",
                _ => "Perfect",
            };
        }

        private void OnEnable()
        {
            if (_jumpGauge != null) _jumpGauge.OnGuageResult += OnGaugeResult;
        }

        private void OnDisable()
        {
            if (_jumpGauge != null) _jumpGauge.OnGuageResult -= OnGaugeResult;
        }

        /// <summary>
        ///         残らないように～
        /// </summary>
        private void OnDestroy()
        {
            if (Time.timeScale != 1f)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
            }

            if (_jumpGauge != null) _jumpGauge.OnGuageResult -= OnGaugeResult;
        }
    }
}