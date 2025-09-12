using System.Collections;
using UnityEngine;

public class JumpTimingGameManager : MonoBehaviour
{
    [Header("UI参照（RectTransform）")]
    [SerializeField] private RectTransform _centerRect;
    [SerializeField] private RectTransform _notesContainer;
    [SerializeField] private RectTransform _outerCircle;

    [SerializeField] private float _minTravel = 0.6f;
    [SerializeField] private float _maxTravel = 1.4f;

    private float _radius;
    private Coroutine _spawnRoutine;

    private void Start()
    {
        //　半径の計算
        if(_outerCircle != null) _radius = _outerCircle.rect.width * 0.5f;
    }

    public void StartTimingGame(float jumpDuration)
    {
        StopTimingGame();
        //_spawnRoutine = StartCoroutine(SpawnNotes(jumpDuration));
    }

    public void StopTimingGame()
    {
        if(_spawnRoutine != null)
        {
            StopCoroutine( _spawnRoutine );
            _spawnRoutine = null;
        }
    }

    //private IEnumerator SpawnNotes(float jumpDuration)
    //{
    //    float elapsed = 0f;
    //    while (elapsed < jumpDuration)
    //    {
    //        float angle = Random.Range(0f, 360f);
    //        float travel = Random.Range(_minTravel, _maxTravel);


    //    }
    //}
}
