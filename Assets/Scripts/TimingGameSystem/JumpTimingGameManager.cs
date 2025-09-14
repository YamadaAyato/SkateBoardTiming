using System;
using System.Collections;
using UnityEngine;

public class JumpTimingGameManager : MonoBehaviour
{
    [Header("UI参照（RectTransform）")]
    [SerializeField] private RectTransform _centerRect;
    [SerializeField] private RectTransform _notesContainer;
    [SerializeField] private RectTransform _outerCircle;

    [Header("参照")]
    [SerializeField] private TimingNote _notePrefabs;
    [SerializeField] private NoteJudgeController _noteJudgeController;

    [SerializeField] private float _minTravel = 0.6f;
    [SerializeField] private float _maxTravel = 1.4f;
    [SerializeField] private float _minSpawnInterval = 0.3f;
    [SerializeField] private float _maxSpawnInterval = 1.0f;
    [SerializeField] private float _niceDeg = 25f;

    public event Action<string> OnJudgeResult;

    private float _radius;
    private Coroutine _spawnRoutine;

    private void Start()
    {
        //　半径の計算
        if (_outerCircle != null) _radius = _outerCircle.rect.width * 0.5f;
    }

    public void StartTimingGame(float jumpDuration)
    {
        StopTimingGame();
        _spawnRoutine = StartCoroutine(SpawnNotes(jumpDuration));
    }

    public void StopTimingGame()
    {
        if (_spawnRoutine != null)
        {
            StopCoroutine(_spawnRoutine);
            _spawnRoutine = null;
        }
    }

    private IEnumerator SpawnNotes(float jumpDuration)
    {
        float elapsed = 0f;
        while (elapsed < jumpDuration)
        {
            float angle = UnityEngine.Random.Range(0f, 360f);
            float travel = UnityEngine.Random.Range(_minTravel, _maxTravel);

            TimingNote note = Instantiate(_notePrefabs, _notesContainer);
            note.Init(_centerRect, _notesContainer, _radius, angle, travel);
            note.OnReached += OnNoteReached;

            float interval = UnityEngine.Random.Range
                (_minSpawnInterval, _maxSpawnInterval);

            yield return new WaitForSecondsRealtime(interval);
            elapsed += interval;
        }

        _spawnRoutine = null;
    }

    private void OnNoteReached(TimingNote note)
    {
        float noteAngle = note.Angle;
        float playerAngle = _noteJudgeController.Angle;
        float diff = Mathf.Abs(Mathf.DeltaAngle(noteAngle, playerAngle));

        string result = (diff <= _niceDeg) ? "Nice" : "Miss";
        Debug.Log($"判定: {result} 角度差= {diff:F1}度");

        OnJudgeResult?.Invoke(result);

        note.OnReached -= OnNoteReached;
        Destroy(note.gameObject);
    }

#if UNITY_EDITOR
    // デバッグ用に短時間で起動するキー（Editorでのテスト用）
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartTimingGame(5f);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StopTimingGame();
        }
    }
#endif
}
