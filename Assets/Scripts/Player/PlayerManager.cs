using Stage;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    ///         プレイヤーすべての統括
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerJumpController _jumpController;
        [SerializeField] private RampManager _rampManager;
        [SerializeField] private RampMovementController _rampController;

        [Header("次の地点への移動")]
        [SerializeField] private List<Transform> _nextPoints = new List<Transform>();
        [SerializeField] private float _moveSpeed = 5f;

        private int _pointsIndex = 0;
        private bool _moveToNext;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _moveToNext = true;
        }

        private void Update()
        {
            if (_moveToNext)
            {
                MoveToNextPoint();
            }
        }

        public void MoveToNext() => _moveToNext = true;

        /// <summary>
        ///         ジャンプ地点までの移動
        /// </summary>
        private void MoveToNextPoint()
        {
            //　　配列が空でないか確認
            if (_nextPoints.Count == 0)
            {
                Debug.LogWarning("移動する地点が設定されていません。");
                _moveToNext = false;
                return;
            }

            Transform currentTarget = _nextPoints[_pointsIndex];

            transform.position = Vector3.MoveTowards(
                this.transform.position, currentTarget.position, _moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, currentTarget.position) < 0.05f)
            {
                _moveToNext = false;
                Debug.Log("次の地点に到着！");

                if(_rampManager != null)
                {
                    RampPoint nearest = _rampManager.GetNearestRampPoint(this.transform.position);

                    if (nearest != null)
                    {
                        _rampController.StartRamp(nearest.start, nearest.peak, nearest.end);
                        return;
                    }
                    else
                    {
                        Debug.Log("ランプポイントが見つからない");
                    }
                }

                _pointsIndex++;
            }
        }

        /// <summary>
        ///         ランプ移動後の処理
        /// </summary>
        private void HandleRampFinished()
        {
            _jumpController.StartJump();
        }

        /// <summary>
        ///         ジャンプ移動後の処理            
        /// </summary>
        private void HandleJumpFinished()
        {
            MoveToNext();
        }

        private void OnEnable()
        {
            if (_rampController != null)
                _rampController.OnRampFinished += HandleRampFinished;

            if (_jumpController != null)
                _jumpController.OnJumpFinished += HandleJumpFinished;
        }

        private void OnDisable()
        {
            if (_rampController != null)
                _rampController.OnRampFinished -= HandleRampFinished;

            if (_jumpController != null)
                _jumpController.OnJumpFinished -= HandleJumpFinished;
        }

        private void OnDestroy()
        {
            if (_rampController != null)
                _rampController.OnRampFinished -= HandleRampFinished;

            if (_jumpController != null)
                _jumpController.OnJumpFinished -= HandleJumpFinished;
        }
    }
}