using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }

    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioClip[] _bgmClips;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Play("レトロギャロップ", 0.1f);
    }

    /// <summary>
    ///         名前で指定してBGMを再生
    /// </summary>
    public void Play(string bgmName, float volume = 0.1f)
    {
        AudioClip clip = System.Array.Find(_bgmClips, c => c.name == bgmName);

        if (_bgmSource.clip == clip && _bgmSource.isPlaying) return;

        _bgmSource.clip = clip;
        _bgmSource.volume = volume;
        _bgmSource.loop = true;
        _bgmSource.Play();
    }

    /// <summary>
    ///         BGMを停止
    /// </summary>
    public void Stop()
    {
        _bgmSource.Stop();
    }
}
