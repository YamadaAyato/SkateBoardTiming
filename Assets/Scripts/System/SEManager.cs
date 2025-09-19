using UnityEngine;

public class SEManager : MonoBehaviour
{
    public static SEManager Instance { get; private set; }

    [SerializeField] private AudioSource _seAudioSource;
    [SerializeField] private AudioClip[] _seClips;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    ///         名前で指定してSEを再生
    /// </summary>
    public void Play(string seName, float volume = 1f)
    {
        AudioClip clip = System.Array.Find(_seClips, c => c.name == seName);
        if (clip == null)
        {
            Debug.LogWarning($"SEManager: {seName} が見つかりません (配列に入っていない可能性)");
            return;
        }

        _seAudioSource.PlayOneShot(clip, volume);
    }
}
