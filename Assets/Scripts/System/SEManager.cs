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
    ///         –¼‘O‚Åw’è‚µ‚ÄSE‚ğÄ¶
    /// </summary>
    public void Play(string seName, float volume = 1f)
    {
        AudioClip clip = System.Array.Find(_seClips, c => c.name == seName);

        _seAudioSource.PlayOneShot(clip, volume);
    }
}
