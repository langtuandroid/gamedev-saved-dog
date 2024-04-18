using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    [SerializeField]
    private Image _loadingImage;
    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        var op = SceneManager.LoadSceneAsync(1);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            _loadingImage.fillAmount = progress;
            yield return new WaitForSeconds(0.1f);
        }
        op.allowSceneActivation = true;
    }
}
