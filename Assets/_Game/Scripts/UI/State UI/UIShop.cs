using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Spine;
using Spine.Unity;

public class UIShop : UICanvas
{
    [SerializeField] private SkeletonGraphic skeletonAnimation;
    [SerializeField] private GameObject buttonCharPrefab, shopSpine;
    [SerializeField] private Transform content;
    [SerializeField] private Text myCoinText, coinText, charName, charHp;
    //[SerializeField] private Image charImage;
    [SerializeField] private Sprite boxYellow, boxBlue;
    [SerializeField] private Button buyButton, useButton, usedButton;
    [SerializeField] private RectTransform popupRect;
    public List<CharacterSO> charList;

    public RectTransform coverShopRect, coinRect;

    private GameObject buttonTemp;
    private ButtonCharDisplay buttonChar;
    private Button button;
    public List<Button> buttonCharList;
    public List<ButtonCharDisplay> buttonCharDisplayList;

    private int currentCharIndex, previousCharIndex;

    // Render Skin
    private SkeletonData skeletonData = new SkeletonData();
    private List<Skin> skins;
    private Skin currentSkin;


    // Data
    private List<int> charUnlock = new List<int>();

    private void OnEnable()
    {
        currentCharIndex = -1;
        previousCharIndex = -1;
        CheckData();

        SetStateDisplayChar(false);
        UpdateMyCoinText();
        SetAnimForUIShop();
        LoadChar();
    }
    private void OnDisable()
    {
        ResetAnim();
        ClearShopWhenClose();
        SetDefautPopup();
    }
    private void UpdateMyCoinText()
    {
        myCoinText.text = DataController.Instance.currentGameData.coin.ToString();
    }

    private void CheckData()
    {
        if (DataController.Instance.currentGameData.charUnlock.Count == 0)
        {
            for(int i = 0; i < charList.Count; i++)
            {
                DataController.Instance.currentGameData.charUnlock.Add(charList[i].owned == true ? 1 : 0);
                DataController.Instance.currentGameData.currentAd.Add(0);
                DataController.Instance.currentGameData.maxAd.Add(charList[i].adMustWatch);
            }
        }
        else if (DataController.Instance.currentGameData.charUnlock.Count < charList.Count)
        {
            for(int i = DataController.Instance.currentGameData.charUnlock.Count; i < charList.Count; i++)
            {
                DataController.Instance.currentGameData.charUnlock.Add(charList[i].owned == true ? 1 : 0);
                DataController.Instance.currentGameData.currentAd.Add(0);
                DataController.Instance.currentGameData.maxAd.Add(charList[i].adMustWatch);
            }
        }
    }
    private void SetStateDisplayChar(bool a)
    {
        charName.gameObject.SetActive(a);
        charHp.gameObject.SetActive(a);
        shopSpine.SetActive(a);
    }
    private void LoadChar()
    {
        charUnlock = DataController.Instance.currentGameData.charUnlock;
        for(int i = 0; i < charList.Count; i++)
        {
            buttonTemp = Instantiate(buttonCharPrefab, content);
            buttonChar = buttonTemp.GetComponent<ButtonCharDisplay>();
            button = buttonTemp.GetComponent<Button>();

            // Load 
            buttonChar.charImage.sprite = charList[i].image;
            buttonChar.currentAd = DataController.Instance.currentGameData.currentAd[i];
            buttonChar.maxAd = DataController.Instance.currentGameData.maxAd[i];
            buttonChar.textAd.text = buttonChar.currentAd + "/" + buttonChar.maxAd;
            buttonChar.selectedIcon.SetActive(false);

            // if isUsing character
            if (DataController.Instance.currentGameData.currentChar == i)
            {
                previousCharIndex = i;

                button.image.sprite = boxYellow;
                buttonChar.selectedIcon.SetActive(true);

                buyButton.gameObject.SetActive(false);
                useButton.gameObject.SetActive(false);
                usedButton.gameObject.SetActive(true);

                SetStateDisplayChar(true);
                DisplayChar(i);
            }

            // if unlocked, no need ad unlock
            if (charUnlock[i] == 1)
            {
                buttonChar.textAd.gameObject.SetActive(false);
                buttonChar.adImage.gameObject.SetActive(false);
            }

            // add
            buttonCharList.Add(button);
            buttonCharDisplayList.Add(buttonChar);
        }
        for(int i = 0; i < buttonCharList.Count; i++)
        {
            int index = i;
            buttonCharList[index].onClick.AddListener(() =>
            {
                currentCharIndex = index;

                // Show selected icon
                if (previousCharIndex != -1)
                {
                    buttonCharDisplayList[previousCharIndex].selectedIcon.SetActive(false);
                }
                buttonCharDisplayList[currentCharIndex].selectedIcon.SetActive(true);

                //Debug.Log(index + " --- " + DataController.Instance.currentGameData.currentChar);
                if (DataController.Instance.currentGameData.charUnlock[index] == 1)
                {
                    if (DataController.Instance.currentGameData.currentChar != index)
                    {
                        buyButton.gameObject.SetActive(false);
                        useButton.gameObject.SetActive(true);
                        usedButton.gameObject.SetActive(false);
                    }
                    else
                    {
                        buyButton.gameObject.SetActive(false);
                        useButton.gameObject.SetActive(false);
                        usedButton.gameObject.SetActive(true);
                    }
                }
                else
                {
                    buyButton.gameObject.SetActive(true);
                    useButton.gameObject.SetActive(false);
                    usedButton.gameObject.SetActive(false);
                    coinText.text = charList[index].price.ToString();
                }
                SetStateDisplayChar(true);
                DisplayChar(index);

                previousCharIndex = index;
            });
        }
    }

    private void DisplayChar(int index)
    {
        SetSkin(charList[index].animIndex);
        charName.text = charList[index].name;
        charHp.text = "HP : " + charList[index].hp.ToString();
    }

    private void SetSkin(int skinIndex)
    {
        // Setup Skin
        skeletonData = skeletonAnimation.SkeletonData;
        skins = new List<Skin>(skeletonData.Skins.ToArray());
        Skin skin = skins[skinIndex];

        skeletonAnimation.Skeleton.SetSkin(skin);
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
    }

    public void CloseButton()
    {
        UIManager.Instance.OpenUI<UIMainMenu>();
        CloseDirectly();

        AudioManager.instance.PlayBGM(Constant.AUDIO_MUSIC_BG);
        AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);
         }
    private void ClearShopWhenClose()
    {
        for(int i = 0; i < buttonCharList.Count; i++)
        {
            Destroy(buttonCharList[i].gameObject);
            Destroy(buttonCharDisplayList[i].gameObject);
        }
        buttonCharDisplayList.Clear();
        buttonCharList.Clear();
    }
    public void BuyButton()
    {
        if (currentCharIndex == -1)
            return;
        // Check if enough money
        if (DataController.Instance.currentGameData.coin < charList[currentCharIndex].price)
        {
            AnimPopup();
            return;
        }
        else
        {
            // coin data
            DataController.Instance.currentGameData.coin -= charList[currentCharIndex].price;
            UpdateMyCoinText();

            // owned data
            DataController.Instance.currentGameData.charUnlock[currentCharIndex] = 1;

            // UI
            buyButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(true);
            usedButton.gameObject.SetActive(false);
            buttonCharDisplayList[currentCharIndex].textAd.gameObject.SetActive(false);
            buttonCharDisplayList[currentCharIndex].adImage.gameObject.SetActive(false);
        }

        DataPersistence.Instance.SaveGame();
    }

    private void AnimPopup()
    {
        popupRect.gameObject.SetActive(true);
        popupRect.DOAnchorPos(Vector2.zero, 1.15f, false).SetEase(Ease.InOutQuart);
        popupRect.DOScale(0f, 0.5f).SetDelay(2.5f).OnComplete(SetDefautPopup);
    }
    private void SetDefautPopup()
    {
        popupRect.DOAnchorPos(new Vector2(0, -1200f), 0f, false);
        popupRect.DOScale(1f, 0f);
        popupRect.gameObject.SetActive(false);
    }

    public void UseButton()
    {
        if (currentCharIndex == -1)
            return;

        buttonCharList[DataController.Instance.currentGameData.currentChar].image.sprite = boxBlue;
        buttonCharList[currentCharIndex].image.sprite = boxYellow;
        // Data
        DataController.Instance.currentGameData.currentChar = currentCharIndex;
        // UI
        buyButton.gameObject.SetActive(false);
        useButton.gameObject.SetActive(false);
        usedButton.gameObject.SetActive(true);


        DataPersistence.Instance.SaveGame();
    }

    public void AdButton()
    {
        if (currentCharIndex == -1)
            return;
        if (!buttonCharDisplayList[currentCharIndex].textAd.gameObject.activeSelf)
            return;
        DataPersistence.Instance.SaveGame();
    }

    public void SetAnimForUIShop()
    {
        coverShopRect.DOAnchorPos(new Vector2(0f, -314f), 0.25f).SetEase(Ease.InOutSine);
        coinRect.DOAnchorPos(new Vector2(355f, 844f), 0.25f).SetEase(Ease.InOutSine);
    }
    public void ResetAnim()
    {
        coverShopRect.DOAnchorPos(new Vector2(0f, -1622f), 0f).SetEase(Ease.Linear);
        coinRect.DOAnchorPos(new Vector2(670f, 844f), 0f).SetEase(Ease.InOutSine);
    }

    #region Ad
    void OnWatchVideoSuccess()
    {
        DataController.Instance.currentGameData.currentAd[currentCharIndex]++;

        buttonCharDisplayList[currentCharIndex].textAd.text = DataController.Instance.currentGameData.currentAd[currentCharIndex] + "/"
            + charList[currentCharIndex].adMustWatch;
        if (DataController.Instance.currentGameData.currentAd[currentCharIndex] == charList[currentCharIndex].adMustWatch)
        {
            // owned data
            DataController.Instance.currentGameData.charUnlock[currentCharIndex] = 1;

            // UI
            buyButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(true);
            usedButton.gameObject.SetActive(false);

            buttonCharDisplayList[currentCharIndex].textAd.gameObject.SetActive(false);
            buttonCharDisplayList[currentCharIndex].adImage.gameObject.SetActive(false);
        }

    }

    void OnWatchVideoFailed()
    {
        Debug.Log("Show Popup Watchvideo failed");
    }
    #endregion
}
