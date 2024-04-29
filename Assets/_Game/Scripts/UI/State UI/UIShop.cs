using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Spine;
using Spine.Unity;
using TMPro;
using UnityEngine.Serialization;
using Zenject;

public class UIShop : UICanvas
{
    [FormerlySerializedAs("skeletonAnimation"),SerializeField] private SkeletonGraphic skeletonGraphic;
    [FormerlySerializedAs("buttonCharPrefab"),SerializeField] private GameObject buttonCharacterPrefab;
    [SerializeField] private GameObject shopSpine;
    [SerializeField] private Transform content;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI _diamondText;
    [SerializeField] private Image _coinImage;
    [SerializeField] private TextMeshProUGUI priceCoinText;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI characterHealth;
    [SerializeField] private Sprite boxYellow, boxBlue;
    [SerializeField] private Button buyButton, useButton, usedButton, watchButton;
    [SerializeField] private RectTransform popupRect;
    [SerializeField] private TextMeshProUGUI _popupText;
    [SerializeField] private RectTransform coverShopRect, coinRect;
    [SerializeField] private Sprite _coinSprite, _diamondSprite;
    [FormerlySerializedAs("charList"),SerializeField] private List<CharacterSO> charactersList;
    [FormerlySerializedAs("buttonCharList"),SerializeField] private List<Button> buttonCharactersList;
    [FormerlySerializedAs("buttonCharDisplayList"),SerializeField] private List<ButtonCharDisplay> buttonCharactersDisplayList;

    private int currentCharacterIndex, previousCharacterIndex;
   [SerializeField] private List<int> charUnlock = new List<int>();
    private GameObject buttonTemp;
    private ButtonCharDisplay buttonChar;
    private Button button;
    private SkeletonData skeletonData = new SkeletonData();
    private List<Skin> skins;
    
    private DataPersistence _dataPersistence;
    private DataController _dataController;
    private AudioManager _audioManager;

    [Inject]
    private void Construct(DataPersistence dataPersistence, DataController dataController, AudioManager audioManager)
    {
        _dataPersistence = dataPersistence;
        _dataController = dataController;
        _audioManager = audioManager;
    }

    private void OnEnable()
    {
        currentCharacterIndex = -1;
        previousCharacterIndex = -1;
        CheckData();

        SetStateDisplayCharacter(false);
        UpdateCoinText();
        UpdateDiamondText();
        SetAnimForUIShop();
        LoadCharacter();
    }
    
    private void OnDisable()
    {
        ResetAnim();
        ClearShopOnClose();
        SetDefaultPopup();
    }
    
    private void UpdateCoinText()
    {
        coinText.text = _dataController.currentGameData.coin.ToString();
    }
    
    private void UpdateDiamondText()
    {
        _diamondText.text = _dataController.currentGameData.diamonds.ToString();
    }

    private void CheckData()
    {
        if (_dataController.currentGameData.charUnlock.Count == 0)
        {
            foreach (CharacterSO character in charactersList)
            {
                _dataController.currentGameData.charUnlock.Add(character.isOwned ? 1 : 0);
                _dataController.currentGameData.currentAd.Add(0);
                _dataController.currentGameData.maxAd.Add(character.adMustWatch);
            }
        } else if (_dataController.currentGameData.charUnlock.Count < charactersList.Count)
        {
            for(int i = _dataController.currentGameData.charUnlock.Count; i < charactersList.Count; i++)
            {
                _dataController.currentGameData.charUnlock.Add(charactersList[i].isOwned ? 1 : 0);
                _dataController.currentGameData.currentAd.Add(0);
                _dataController.currentGameData.maxAd.Add(charactersList[i].adMustWatch);
            }
        }
    }
    private void SetStateDisplayCharacter(bool value)
    {
        characterName.gameObject.SetActive(value);
        characterHealth.gameObject.SetActive(value);
       // shopSpine.SetActive(value);
    }
    
   private void LoadCharacter()
{
    charUnlock = _dataController.currentGameData.charUnlock;
    
    for(int i = 0; i < charactersList.Count; i++)
    {
        buttonTemp = Instantiate(buttonCharacterPrefab, content);
        buttonChar = buttonTemp.GetComponent<ButtonCharDisplay>();
        button = buttonTemp.GetComponent<Button>();

        buttonChar.CharacterImage.sprite = charactersList[i].skinImage;
        buttonChar.SelectedIcon.SetActive(false);

        // Проверяем, разблокирован ли персонаж
        if (_dataController.currentGameData.charUnlock[i] == 1)
        {
            buttonChar.OwnedText.gameObject.SetActive(true);
        }
        
        if (charactersList[i].shopType == ShopType.Ad)
        {
            buttonChar.ADObject.SetActive(true);
            buttonChar.CurrentAd = _dataController.currentGameData.currentAd[i];
            buttonChar.MaxAd = _dataController.currentGameData.maxAd[i];
            buttonChar.ADText.text = buttonChar.CurrentAd + "/" + buttonChar.MaxAd;
        } else
        {
            buttonChar.ADObject.SetActive(false);
        }
        
        if (_dataController.currentGameData.currentChar == i)
        {
            previousCharacterIndex = i;

            button.image.sprite = boxYellow;
            buttonChar.SelectedIcon.SetActive(true);

            buyButton.gameObject.SetActive(false);
            watchButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
            usedButton.gameObject.SetActive(true);

            SetStateDisplayCharacter(true);
            DisplayCharacter(i);
        }
        

        buttonCharactersList.Add(button);
        buttonCharactersDisplayList.Add(buttonChar);
    }
    for(int i = 0; i < buttonCharactersList.Count; i++)
    {
        int index = i;
        buttonCharactersList[index].onClick.AddListener(() =>
        {
            currentCharacterIndex = index;

            if (previousCharacterIndex != -1)
            {
                buttonCharactersDisplayList[previousCharacterIndex].SelectedIcon.SetActive(false);
            }
            buttonCharactersDisplayList[currentCharacterIndex].SelectedIcon.SetActive(true);

            if (_dataController.currentGameData.charUnlock[index] == 1)
            {
                if (_dataController.currentGameData.currentChar != index)
                {
                    buyButton.gameObject.SetActive(false);
                    watchButton.gameObject.SetActive(false);
                    useButton.gameObject.SetActive(true);
                    usedButton.gameObject.SetActive(false);
                } else
                {
                    buyButton.gameObject.SetActive(false);
                    watchButton.gameObject.SetActive(false);
                    useButton.gameObject.SetActive(false);
                    usedButton.gameObject.SetActive(true);
                }
            } else
            {
                if (charactersList[index].shopType == ShopType.Ad)
                {
                    watchButton.gameObject.SetActive(true);
                    buyButton.gameObject.SetActive(false);
                    useButton.gameObject.SetActive(false);
                    usedButton.gameObject.SetActive(false);
                } else
                {
                    buyButton.gameObject.SetActive(true);
                    watchButton.gameObject.SetActive(false);
                    useButton.gameObject.SetActive(false);
                    usedButton.gameObject.SetActive(false);
                    priceCoinText.text = charactersList[index].price.ToString();

                    if (charactersList[index].shopType == ShopType.Coin)
                    {
                        _coinImage.sprite = _coinSprite;
                    } else if (charactersList[index].shopType == ShopType.Diamond)
                    {
                        _coinImage.sprite = _diamondSprite;
                    }
                }
            }
            SetStateDisplayCharacter(true);
            DisplayCharacter(index);

            previousCharacterIndex = index;
        });
    }
}

   private void DisplayCharacter(int index)
    {
        //SetCharacterSkin(charactersList[index].animationIndex);
        characterName.text = charactersList[index].skinName;
        characterHealth.text = "HP : " + charactersList[index].health;
    }

    private void SetCharacterSkin(int skinIndex)
    {
        skeletonData = skeletonGraphic.SkeletonData;
        skins = new List<Skin>(skeletonData.Skins.ToArray());
        Skin skin = skins[skinIndex];

        skeletonGraphic.Skeleton.SetSkin(skin);
        skeletonGraphic.Skeleton.SetSlotsToSetupPose();
    }

    public void CloseButton()
    {
        _uiManager.OpenUI<UIMainMenu>();
        CloseImmediately();

        _audioManager.PlayBG(Constant.AUDIO_MUSIC_BG);
        _audioManager.Play(Constant.AUDIO_SFX_BUTTON); 
    }
    
    private void ClearShopOnClose()
    {
        for(int i = 0; i < buttonCharactersList.Count; i++)
        {
            Destroy(buttonCharactersList[i].gameObject);
            Destroy(buttonCharactersDisplayList[i].gameObject);
        }
        buttonCharactersDisplayList.Clear();
        buttonCharactersList.Clear();
    }
    
    public void BuyButton()
    {
        if (currentCharacterIndex == -1)
        {
            return;
        }

        ShopType shopType = charactersList[currentCharacterIndex].shopType;
        int price = charactersList[currentCharacterIndex].price;

        switch (shopType)
        {
            case ShopType.Coin:
                if (!CheckBalance(_dataController.currentGameData.coin, price))
                {
                    PopupAnimation(shopType);
                    return;
                }
                _dataController.currentGameData.coin -= price;
                UpdateCoinText();
                _dataController.currentGameData.charUnlock[currentCharacterIndex] = 1;
                break;
            case ShopType.Diamond:
                if (!CheckBalance(_dataController.currentGameData.diamonds, price))
                {
                    PopupAnimation(shopType);
                    return;
                }
                _dataController.currentGameData.diamonds -= price;
                UpdateDiamondText();
                _dataController.currentGameData.charUnlock[currentCharacterIndex] = 1;
                break;
        }

        buttonCharactersDisplayList[currentCharacterIndex].OwnedText.gameObject.SetActive(true);

        buyButton.gameObject.SetActive(false);
        watchButton.gameObject.SetActive(false);
        useButton.gameObject.SetActive(true);
        usedButton.gameObject.SetActive(false);

        _dataPersistence.SaveGame();
    }

    private bool CheckBalance(int currentBalance, int price)
    {
        return currentBalance >= price;
    }

    private void PopupAnimation(ShopType shopType)
    {

        if (shopType == ShopType.Coin)
        {
            _popupText.text = "Not enough money";
        } else if (shopType == ShopType.Diamond)
        {
            _popupText.text = "Not enough diamonds";
        }
        
        popupRect.gameObject.SetActive(true);
        popupRect.DOAnchorPos(Vector2.zero, 1.15f, false).SetEase(Ease.InOutQuart);
        popupRect.DOScale(0f, 0.5f).SetDelay(2.5f).OnComplete(SetDefaultPopup);
    }
    
    private void SetDefaultPopup()
    {
        popupRect.DOAnchorPos(new Vector2(0, -1200f), 0f, false);
        popupRect.DOScale(1f, 0f);
        popupRect.gameObject.SetActive(false);
    }

    public void UseSkinButton()
    {
        if (currentCharacterIndex == -1)
        {
            return;
        }

        buttonCharactersList[_dataController.currentGameData.currentChar].image.sprite = boxBlue;
        buttonCharactersList[currentCharacterIndex].image.sprite = boxYellow;
        _dataController.currentGameData.currentChar = currentCharacterIndex;
        buyButton.gameObject.SetActive(false);
        useButton.gameObject.SetActive(false);
        usedButton.gameObject.SetActive(true);
        
        _dataPersistence.SaveGame();
    }


    private void SetAnimForUIShop()
    {
        //coverShopRect.DOAnchorPos(new Vector2(0f, -314f), 0.25f).SetEase(Ease.InOutSine);
        //coinRect.DOAnchorPos(new Vector2(355f, 844f), 0.25f).SetEase(Ease.InOutSine);
    }

    private void ResetAnim()
    {
        //coverShopRect.DOAnchorPos(new Vector2(0f, -1622f), 0f).SetEase(Ease.Linear);
        //coinRect.DOAnchorPos(new Vector2(670f, 844f), 0f).SetEase(Ease.InOutSine);
    }

    #region Ad
    public void AdButton()
    {
        /*if (currentCharIndex == -1)
            return;
        if (!buttonCharDisplayList[currentCharIndex].textAd.gameObject.activeSelf)
            return;
        _dataPersistence.SaveGame();*/
    }
    void OnWatchVideoSuccess()
    {
        /*_dataController.currentGameData.currentAd[currentCharacterIndex]++;

        buttonCharactersDisplayList[currentCharacterIndex].TextAd.text = _dataController.currentGameData.currentAd[currentCharacterIndex] + "/"
            + charactersList[currentCharacterIndex].adMustWatch;
        if (_dataController.currentGameData.currentAd[currentCharacterIndex] == charactersList[currentCharacterIndex].adMustWatch)
        {
            // owned data
            _dataController.currentGameData.charUnlock[currentCharacterIndex] = 1;

            // UI
            buyButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(true);
            usedButton.gameObject.SetActive(false);

            buttonCharactersDisplayList[currentCharacterIndex].TextAd.gameObject.SetActive(false);
            buttonCharactersDisplayList[currentCharacterIndex].ADImage.gameObject.SetActive(false);
        }*/

    }

    void OnWatchVideoFailed()
    {
        Debug.Log("Show Popup Watchvideo failed");
    }
    #endregion
}
