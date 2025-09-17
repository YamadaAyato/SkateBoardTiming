using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
///         SceneLoadを制御するクラス
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image _fadeImage;
    [SerializeField] private float _fadeTime = 1f;

    private void Awake()
    {
        if(_fadeImage != null)
        {
            Color c = _fadeImage.color;
            c.a = 0f;
            _fadeImage.color = c;
            _fadeImage.gameObject.SetActive(false);
        }
    }

    /// <summary>
    ///         シーンを同期ロード
    /// </summary>
    public void LoadScene(string sceneName)
    {
        if (_fadeImage == null)
        {
            Debug.LogError("FadeImage が未設定です！");
            return;
        }

        _fadeImage.gameObject.SetActive(true);
        _fadeImage.DOFade(1f, _fadeTime).OnComplete(() =>
        {
            // シーン切り替え
            SceneManager.LoadScene(sceneName);
        });
    }

    /// <summary>
    ///         シーンを非同期ロード
    /// </summary>
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    private IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            yield return null;
        }
    }
}