using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using Zenject;

public class UiListGroup : UICanvas
{
    [SerializeField] private GameObject buttonGroupPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private TextMeshProUGUI starsCountInGameText;
    [SerializeField] private RectTransform popupRect;
    [SerializeField] private List<GroupSO> groupSOList;

    private List<Button> buttonGroupList = new List<Button>();
    private ButtonActDisplay buttonGroup;
    private GameObject buttonGroupTemp;
    private Button button;
    private int actSelected;

    public List<GroupSO> GroupSoList => groupSOList;

    private DataController _dataController;
    private AudioManager _audioManager;

    [Inject]
    private void Construct(DataController dataController, AudioManager audioManager)
    {
        _dataController = dataController;
        _audioManager = audioManager;
    }

    private void OnEnable() 
    {
        CheckStarsInLevels();
        UpdateStarsTextInGame();

        for (int i = 0; i < groupSOList.Count; i++)
        {
            buttonGroupTemp = Instantiate(buttonGroupPrefab, content);
            buttonGroup = buttonGroupTemp.GetComponent<ButtonActDisplay>();
            button = buttonGroupTemp.GetComponent<Button>();

            buttonGroup.LoadDataWhenUnlocked(groupSOList[i].title, groupSOList[i].actImage, GetTotalStarsCountInAct(i));
            buttonGroup.LoadDataWhenLock(groupSOList[i].starUnlock);

            if (groupSOList[i].starUnlock > GetTotalStarsCount())
            {
                buttonGroup.DisplayLock();
            } else
            {
                buttonGroup.DisplayUnlock();
            }

            buttonGroupList.Add(button);
        }
        for (int i = 0; i < buttonGroupList.Count; i++)
        {
            int index = i;
            buttonGroupList[i].onClick.AddListener(() =>
            {
                actSelected = index;

                if (groupSOList[index].starUnlock > GetTotalStarsCount())
                {
                    AnimPopup();

                } else
                {
                    _uiManager.OpenUI<UIListLevel>();
                    ClearListGroup();

                    _audioManager.Play(Constant.AUDIO_SFX_PLAY);

                    CloseImmediately();
                }
            });
        }
    }

    private void AnimPopup()
    {
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

    private void CheckStarsInLevels()
    {
        if (_dataController.currentGameData.starDoneInLevels.Count != 0)
        {
            return;
        }

        for (int i = 0; i < 999; i++)
        {
            _dataController.currentGameData.starDoneInLevels.Add(0);
        }
    }
    private void UpdateStarsTextInGame()
    {
        starsCountInGameText.text = GetTotalStarsCount() + "/999";
    }

    private int GetTotalStarsCountInAct(int act)
    {
        int totalStar = 0;
        for(int i = act * 10; i <= (act *10) + 9; i++)
        {
            totalStar += _dataController.currentGameData.starDoneInLevels[i];
        }
        return totalStar;
    }
    private int GetTotalStarsCount()
    {
        int totalStar = 0;
        
        foreach (int t in _dataController.currentGameData.starDoneInLevels)
        {
            totalStar += t;
        }
        return totalStar;
    }

    public void BackButtonClick()
    {
        _uiManager.OpenUI<UIMainMenu>();
        ClearListGroup();
        SetDefaultPopup();

        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);

        CloseImmediately();
    }
    
    public int GetSelectedGroup()
    {
        return actSelected;
    }

    public void ClearListGroup()
    {
        buttonGroupList.Clear();
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
}
