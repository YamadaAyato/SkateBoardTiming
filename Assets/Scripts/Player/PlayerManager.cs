using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerManager : MonoBehaviour
    {
        [Header("次の地点への移動")]
        [SerializeField] private List<Transform> _nextPoints = new List<Transform>();
        [SerializeField] private float _moveSpeed = 5f;

        private int _pointsIndex = 0;
        private bool _moveToNext;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
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
                _pointsIndex++;
            }
        }

        private void LanchForce()
        {

        }
    }
}