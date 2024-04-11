using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum LoadType
{
    Ads,
    Scene
}
public class LoadingScreen : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public GameObject mLoadingAds;
    public GameObject mLoadingScene;

    public Image progressbar;
    public Image imageFade;
    public void Show(LoadType load)
    {
        switch (load)
        {
            case LoadType.Ads:
                canvasGroup.alpha = 1;
                mLoadingAds.SetActive(true);
                mLoadingScene.SetActive(false);
                break;
            case LoadType.Scene:
                canvasGroup.alpha = 1;
                mLoadingAds.SetActive(false);
                mLoadingScene.SetActive(true);

                StartCoroutine(LoadScene());
                break;
            default:
                break;
        }
    }

    public void Hide()
    {
        canvasGroup.DOFade(0, 0.5f);
    }

    IEnumerator LoadScene()
    {
        var op = SceneManager.LoadSceneAsync(1);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1);

        progressbar.DOFillAmount(1, 1f);
        yield return new WaitForSeconds(1f);
        imageFade.DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.75f);
        op.allowSceneActivation = true;
    }
}
