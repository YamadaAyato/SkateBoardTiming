using System;
using UnityEngine;

public class TimingNote : MonoBehaviour
{
    private Vector3 _center;
    private float _radius;
    private float _speed;
    private bool _active;

    public event Action<TimingNote> OnNoteReached;

    /// <summary>
    ///         ƒm[ƒc‰Šú‰»•İ’è
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <param name="speed"></param>
    public void Init(Vector3 center, float radius, float speed)
    {
        _center = center;
        _radius = radius;
        _speed = speed;
        _active = true;

        transform.position = center;
    }

    private void Update()
    {
        if (!_active) return;

        Vector3 direction = (transform.position - _center).normalized;
        if (direction == Vector3.zero) direction = Vector3.right;

        transform.position += direction * _speed * Time.deltaTime;

        // ”¼Œa‚ğ’´‚¦‚½‚çÁ‚·
        if(Vector3.Distance(_center, transform.position) >= _radius)
        {
            _active = false;
            OnNoteReached?.Invoke(this);
        }
    }
}
