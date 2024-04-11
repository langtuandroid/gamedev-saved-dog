using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SceneManagement; 
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
    public Color mMainColor;

    public List<Image> mImages;
    public List<Text> mTexts;

    public GameObject objFirstLoading;
    public GameObject objLangAndTut;
    public GameObject objLang;
    public GameObject objTut;
    public GameObject objMain;

    public Button btnConfirmLanguage;
    public Button[] btnSelectLangs;

    int indexTut = 0;
    public Image tutImage;
    public Sprite[] spriteTuts;
    public Button btnNextOrFinish;

    public Text txtButtonNext;
    public Text txtTutIndex;

    public int startData = 0;

    public List<GameObject> mButtonLanguage;
    public Text mTextLanguageTitle;
    private string[] textLangEn = new string[4] { "Language", "Next", "Get Started", "Tutorial" };
    private string[] textLangFr = new string[4] { "Langue", "Suivante", "Commencer", "Didacticiel" };
    private string[] textLangSp = new string[4] { "Idioma", "Próxima", "Empezar", "Tutorial" };
    private string[] textLangVn = new string[4] { "Ngôn Ngữ", "Tiếp", "Bắt Đầu", "Hướng Dẫn" };
    private void Start()
    {
        startData = PlayerPrefs.GetInt("startdata", 0);
        btnConfirmLanguage.onClick.AddListener(OnConfirmLanguage);
        btnNextOrFinish.onClick.AddListener(FinishTut);
        spriteTuts = Resources.LoadAll<Sprite>("Tutorial/");
        tutImage.sprite = spriteTuts[0];
        txtTutIndex.text = GetTranslatedText(PlayerPrefs.GetInt("language-sdk", 0), 3) + " " + (indexTut + 1) + "/" + spriteTuts.Length;
        txtButtonNext.text = GetTranslatedText(0, 1).ToUpper();

        for (int i = 0; i < mTexts.Count; i++)
        {
            mTexts[i].color = mMainColor;
        }

        for (int i = 0; i < mImages.Count; i++)
        {
            mImages[i].color = mMainColor;
        }

        //StartCoroutine(OnStartGame());
        //StartCoroutine(IOpenGame());
        OpenGame();
    }
    public void SelectLang(int lang)
    {
        for (int i = 0; i < mButtonLanguage.Count; i++)
        {
            mButtonLanguage[i].SetActive(i == lang);
            if (i == lang)
            {
                PlayerPrefs.SetInt("language-sdk", lang);
                mTextLanguageTitle.text = GetTranslatedText(lang, 0);
                txtButtonNext.text = GetTranslatedText(lang, 1).ToUpper();
            }
        }
    }
    IEnumerator OnStartGame()
    {
        float counter = 0;
        yield return new WaitForSeconds(3f);
        while (/*!UbiAdsManager.Instance.isFirstInterDone && */counter < 3)
        {
            counter += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(IOnShowLanguage());
    }
    IEnumerator IOnShowLanguage()
    {
        UbiManager.Instance.ShowLoading(LoadType.Ads);
        yield return new WaitForSeconds(1);
        
    }
    void OnShowLanguage()
    {
        objFirstLoading.SetActive(false);
        switch (startData)
        {
            case 0:
                objLangAndTut.SetActive(true);
                objLang.SetActive(true);
                objTut.SetActive(false);
                break;
            case 1:
                objLangAndTut.SetActive(true);
                objLang.SetActive(false);
                objTut.SetActive(true);
                break;
            case 2:
                objLangAndTut.SetActive(false);
                objMain.SetActive(true);
                UbiManager.Instance.ShowLoading(LoadType.Scene);
                break;
            default:
                break;
        }
    }
    private void OnConfirmLanguage()
    {
        PlayerPrefs.SetInt("startdata", 1);
        StartCoroutine(IOnShowTut());
    }
    IEnumerator IOnShowTut()
    {
        //UbiAdsManager.Instance.LoadNativeAd();
        UbiManager.Instance.ShowLoading(LoadType.Ads);
        yield return new WaitForSeconds(1);
    }
    void OnShowTut()
    {
        objLang.SetActive(false);
        objTut.SetActive(true);
        StartCoroutine(INextTut());
    }
    private void FinishTut()
    {
        if (indexTut < spriteTuts.Length - 1)
        {
            indexTut++;

            tutImage.sprite = spriteTuts[indexTut];
            Debug.Log("indexTut = " + indexTut);
            if (indexTut < spriteTuts.Length - 1)
            {
                txtButtonNext.text = GetTranslatedText(PlayerPrefs.GetInt("language-sdk", 0), 1).ToUpper();
            }
            else
            {
                txtButtonNext.text = GetTranslatedText(PlayerPrefs.GetInt("language-sdk", 0), 2).ToUpper();
            }
           
            StartCoroutine(INextTut());
        }
        else
        {
            PlayerPrefs.SetInt("startdata", 2);
            StartCoroutine(IOpenGame());
        }
        txtTutIndex.text = GetTranslatedText(PlayerPrefs.GetInt("language-sdk", 0), 3)+" "+(indexTut + 1) + "/" + spriteTuts.Length;
    }

    IEnumerator INextTut()
    {
        btnNextOrFinish.interactable = false;
        yield return new WaitForSeconds(1);
        btnNextOrFinish.interactable = true;
    }

    IEnumerator IOpenGame()
    {
        float counter = 0;
        yield return new WaitForSeconds(1);
        while (/*!UbiAdsManager.Instance.isFirstInterDone && */counter < 3)
        {
            counter += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        btnNextOrFinish.interactable = false;
        //UbiManager.Instance.ShowLoading(LoadType.Ads);
        //yield return new WaitForSeconds(1);
        //MediationManager.instance.ShowInterstitialAdOnStop(null, null);
        OpenGame();
    }
    void OpenGame()
    {
        Debug.Log("CALL BACK FROM IOpenGame");
        objLangAndTut.SetActive(false);
        objMain.SetActive(false);
        //UbiManager.Instance.ShowLoading(LoadType.Scene);
        StartCoroutine(LoadScene());
    }
    IEnumerator LoadScene()
    {
        var op = SceneManager.LoadSceneAsync(1);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            yield return new WaitForSeconds(0.1f);
        }
        op.allowSceneActivation = true;
    }
    private string GetTranslatedText(int lang, int content)
    {
        switch (lang)
        {
            case 0:
                return textLangEn[content];
            case 1:
                return textLangFr[content];
            case 2:
                return textLangSp[content];
            case 3:
                return textLangVn[content];
            default:
                return textLangEn[content];
        }
    }
#if UNITY_EDITOR
    [UnityEditor.MenuItem("UbiSDK/Clear Data")]
    static void ClearPref()
    {
        PlayerPrefs.DeleteAll();
    }
    [UnityEditor.MenuItem("UbiSDK/Open Init Scene")]
    static void OpenInitScene()
    {
        EditorSceneManager.OpenScene("Assets/UbiSDK/Scenes/InitScene.unity");
    }
    [UnityEditor.MenuItem("UbiSDK/Open Main Scene")]
    static void OpenMainScene()
    {
        EditorSceneManager.OpenScene("Assets/_Game/Scenes/SampleScene.unity");
    }
#endif
}
