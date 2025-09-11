using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///         SceneLoadを制御するクラス
/// </summary>
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    ///         シーンを同期ロード
    /// </summary>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    ///         シーンを非同期ロード
    /// </summary>
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    private System.Collections.IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            yield return null;
        }
    }
}