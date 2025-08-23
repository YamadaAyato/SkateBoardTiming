using Stage;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private PlayerJumpController _jumpController;
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

                RampPoint ramp = currentTarget.GetComponent<RampPoint>();
                if (ramp != null)
                {
                    _rampController.StartRamp(ramp.start, ramp.peak, ramp.end);
                }

                _pointsIndex++;
            }
        }

        private void HandleRampFinished()
        {
            _jumpController.StartJump();
        }

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