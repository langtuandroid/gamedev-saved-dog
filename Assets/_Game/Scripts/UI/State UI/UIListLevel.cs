using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class UIListLevel : UICanvas
{
    [SerializeField] private Transform content;
    [SerializeField] private Image levelDoneInGroupFill;
    [SerializeField] private Image starsDoneInGroupFill;
    [SerializeField] private TextMeshProUGUI starsGainedInGroup;
    [SerializeField] private TextMeshProUGUI starsGainedInGameCount;
    [SerializeField] private TextMeshProUGUI levelDoneText;

    [SerializeField] private GameObject buttonLevelPrefab;
    [FormerlySerializedAs("actList"),SerializeField] private List<Act> groupList;


    private List<Button> buttonLevelList = new List<Button>();
    private ButtonLevelDisplay buttonLevel;
    private GameObject buttonLevelTemp;
    private Button button;

    private int currentGroup;
    
    private GameManager _gameManager;
    private LevelManager _levelManager;
    private DataController _dataController;
    private AudioManager _audioManager;

    [Inject]
    private void Construct (GameManager gameManager, LevelManager levelManager, DataController dataController, AudioManager audioManager)
    {
        _gameManager = gameManager;
        _levelManager = levelManager;
        _dataController = dataController;
        _audioManager = audioManager;
    }

    private void OnEnable()
    {
        CheckStarsInLevels();
        CheckLevelDone();

        currentGroup = _uiManager.GetUI<UiListGroup>().GetSelectedGroup();

        for (int i = 0; i < groupList[currentGroup].levelSOList.Count; i++)
        {
            buttonLevelTemp = Instantiate(buttonLevelPrefab, content);
            buttonLevel = buttonLevelTemp.GetComponent<ButtonLevelDisplay>();
            button = buttonLevelTemp.GetComponent<Button>();

            buttonLevel.LoadDataUnlocked(groupList[currentGroup].levelSOList[i].levelNumber, groupList[currentGroup].levelSOList[i].levelImage, 
                _dataController.currentGameData.starDoneInLevels[i + currentGroup * 10]);

            if (i > 0 && _dataController.currentGameData.starDoneInLevels[i + currentGroup * 10 - 1] == 0)
            {
                buttonLevel.DisplayLock(true);
            } else
            {
                buttonLevel.DisplayLock(false);
            }

            for (int j = 0; j < buttonLevel.StarsToComplete; j++)
            {
                buttonLevel.Stars[j].color = buttonLevel.StarReceivedColor;
            }

            buttonLevelList.Add(button);
        }
        
        for (int i = 0; i < buttonLevelList.Count; i++)
        {
            int index = i;
            buttonLevelList[i].onClick.AddListener(() =>
            {
                if (index > 0 && _dataController.currentGameData.starDoneInLevels[index + currentGroup * 10 - 1] == 0)
                {} else
                {
                    _uiManager.OpenUI<UIGameplay>();
                    _levelManager.OnLoadLevel(index + currentGroup * 10);
                    _gameManager.ChangeState(GameState.GamePlay);
                    ClearLevelList();

                    _audioManager.Play(Constant.AUDIO_SFX_PLAY);

                    CloseImmediately();
                }

            });
        }

        SetupUIListLevel();
    }

    private void SetupUIListLevel()
    {
        starsGainedInGroup.text = GetTotalStarInGroup(currentGroup) + " / 30";
        levelDoneText.text = GetTotalLevelDoneInGroup(currentGroup) + "/10";
        starsGainedInGameCount.text = GetTotalStarsCount() + "/999";


        starsDoneInGroupFill.fillAmount = GetTotalStarInGroup(currentGroup) / 30f;
        levelDoneInGroupFill.fillAmount = GetTotalLevelDoneInGroup(currentGroup) / 10f;
    }

    private void CheckStarsInLevels()
    {
        if (_dataController.currentGameData.starDoneInLevels.Count == 0)
        {
            for (int i = 0; i < 999; i++)
            {
                _dataController.currentGameData.starDoneInLevels.Add(0);
            }
        }
    }
    private void CheckLevelDone()
    {
        if (_dataController.currentGameData.levelDoneInGame.Count != 0)
        {
            return;
        }

        for (int i = 0; i < 999; i++)
        {
            _dataController.currentGameData.levelDoneInGame.Add(0);
        }
    }
    
    private int GetTotalStarInGroup(int act)
    {
        int totalStar = 0;
        for (int i = act * 10; i <= (act * 10) + 9; i++)
        {
            totalStar += _dataController.currentGameData.starDoneInLevels[i];
        }
        return totalStar;
    }
    
    private int GetTotalLevelDoneInGroup(int act)
    {
        int totalLevel = 0;
        for (int i = act * 10; i <= (act * 10) + 9; i++)
        {
            totalLevel += _dataController.currentGameData.levelDoneInGame[i];
        }
        return totalLevel;
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
        _uiManager.OpenUI<UiListGroup>();
        ClearLevelList();

        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);

        CloseImmediately();
    }

    private void ClearLevelList()
    {
        buttonLevelList.Clear();
        for(int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
}

[Serializable]
public class Act
{
    public List<LevelSO> levelSOList;
}
