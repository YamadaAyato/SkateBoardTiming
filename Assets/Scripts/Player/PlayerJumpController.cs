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

        public Action OnJumpFinished;

        private bool _isJumping;
        private Vector3 _startPos;

        /// <summary>
        ///         ジャンプスタート！！
        /// </summary>
        public void StartJump()
        {
            if (_isJumping) return;
            StartCoroutine(JumpMovement());
        }

        /// <summary>
        ///         ジャンプ処理
        /// </summary>
        /// <returns></returns>
        private IEnumerator JumpMovement()
        {
            _isJumping = true;
            _startPos = transform.position;
            Quaternion initRot = transform.rotation;

            Time.timeScale = _slowTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            //  スロー演出追加予定

            yield return new WaitForSecondsRealtime(_slowDuration);

            //  タイミング判定
            string result = DebugGetRandomResult();
            Debug.Log("判定結果" + result);

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

            //  スロー解除
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;

            float timer = 0f;
            Quaternion endRot = initRot * Quaternion.Euler(0, _rotAngle, 0);
            float jumpRotStart = 0.3f;
            float jumpRotEnd = 0.7f;

            while (timer < jumpDuration)
            {
                timer += Time.deltaTime;
                float t = timer / jumpDuration;

                //  放物線移動
                float yOffset = Mathf.Sin(Mathf.PI * t) * jumpHeight;
                transform.position = _startPos + new Vector3(0, yOffset, 0);

                if (t >= jumpRotStart && t <= jumpRotEnd)
                {
                    float currentRot = Mathf.InverseLerp(jumpRotStart, jumpRotEnd, t);
                    transform.rotation = Quaternion.Lerp(initRot, endRot, currentRot);
                }
                yield return null;
            }

            transform.position = _startPos;
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

        /// <summary>
        ///         残らないように～
        /// </summary>
        private void OnDestroy()
        {
            if (Time.timeScale != default)
            {
                Time.timeScale = default;
                Time.fixedDeltaTime = default;
            }
        }
    }
}