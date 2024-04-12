using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;

public class UiListAct : UICanvas
{
    public GameObject buttonActPrefab;
    public Transform content;
    public List<ActSO> actSOList;
    public List<Button> btnActList;

    [SerializeField] private Text starInGame;
    [SerializeField] private RectTransform popupRect;

    private ButtonActDisplay buttonAct;
    private GameObject buttonActTemp;
    private Button button;

    private int actSelected;

    private DataController _dataController;
    private AudioManager _audioManager;
    private UIManager _uiManager;

    [Inject]
    private void Construct(DataController dataController, AudioManager audioManager, UIManager uiManager)
    {
        _dataController = dataController;
        _audioManager = audioManager;
        _uiManager = uiManager;
    }
    
    void OnEnable() 
    {
        CheckStarInLevels();
        UpdateTextStarInGame();

        for (int i = 0; i < actSOList.Count; i++)
        {
            buttonActTemp = Instantiate(buttonActPrefab, content);
            buttonAct = buttonActTemp.GetComponent<ButtonActDisplay>();
            button = buttonActTemp.GetComponent<Button>();

            // load data when unlocked
            buttonAct.LoadDataWhenUnlocked(actSOList[i].title, actSOList[i].actImage, GetTotalStarInAct(i));
            // load data when lock
            buttonAct.LoadDataWhenLock(actSOList[i].starUnlock);

            // if lock
            if (actSOList[i].starUnlock > GetTotalStarInGame())
            {
                buttonAct.DisplayLock();
            }
            else  // if unlocked
            {
                buttonAct.DisplayUnlock();
            }

            btnActList.Add(button);
        }
        for (int i = 0; i < btnActList.Count; i++)
        {
            int index = i;
            btnActList[i].onClick.AddListener(() =>
            {
                actSelected = index;
                // if lock
                if (actSOList[index].starUnlock > GetTotalStarInGame())
                {
                    // ... Do something
                    AnimPopup();

                }
                else  // if unlocked
                {
                    _uiManager.OpenUI<UIListLevel>();
                    ClearListAct();

                    _audioManager.Play(Constant.AUDIO_SFX_PLAY);

                    CloseDirectly();
                }
            });
        }
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

    private void CheckStarInLevels()
    {
        if (_dataController.currentGameData.starDoneInLevels.Count == 0)
        {
            for (int i = 0; i < 999; i++)
            {
                _dataController.currentGameData.starDoneInLevels.Add(0);
            }
        }
    }
    private void UpdateTextStarInGame()
    {
        starInGame.text = GetTotalStarInGame() + "/999";
    }

    private int GetTotalStarInAct(int act)
    {
        int totalStar = 0;
        for(int i = act * 10; i <= (act *10) + 9; i++)
        {
            totalStar += _dataController.currentGameData.starDoneInLevels[i];
        }
        return totalStar;
    }
    private int GetTotalStarInGame()
    {
        int totalStar = 0;
        for(int i = 0; i < _dataController.currentGameData.starDoneInLevels.Count; i++)
        {
            totalStar += _dataController.currentGameData.starDoneInLevels[i];
        }
        return totalStar;
    }

    public void BackButton()
    {
        _uiManager.OpenUI<UIMainMenu>();
        ClearListAct();
        SetDefautPopup();

        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);

        CloseDirectly();
    }

    
    public int GetSelectedAct()
    {
        return actSelected;
    }

    public void ClearListAct()
    {
        btnActList.Clear();
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
}
