using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIListLevel : UICanvas
{
    public Transform content;
    public Image levelDoneInAct, starDoneInAct;
    public Image circleStar;
    public Text numStarGainedAct, numStarGainedGame, levelDoneText;

    public GameObject buttonLevelPrefab;
    public List<Act> actList;
    public List<Button> btnLevelList;


    private ButtonLevelDisplay buttonLevel;
    private GameObject buttonLevelTemp;
    private Button button;

    private int currentAct;
    private GameManager _gameManager;

    [Inject]
    private void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void OnEnable()
    {
        CheckStarInLevels();
        CheckLevelDone();

        currentAct = UIManager.Instance.GetUI<UiListAct>().GetSelectedAct();

        for (int i = 0; i < actList[currentAct].levelSOList.Count; i++)
        {
            buttonLevelTemp = Instantiate(buttonLevelPrefab, content);
            buttonLevel = buttonLevelTemp.GetComponent<ButtonLevelDisplay>();
            button = buttonLevelTemp.GetComponent<Button>();

            // load data when unlocked
            buttonLevel.LoadDataUnlocked(actList[currentAct].levelSOList[i].numLevel, actList[currentAct].levelSOList[i].levelImage, 
                DataController.Instance.currentGameData.starDoneInLevels[i + currentAct * 10]);
            
            // if lock
            if (i > 0 && DataController.Instance.currentGameData.starDoneInLevels[i + currentAct * 10 - 1] == 0)
            {
                buttonLevel.DisplayLock(true);
            }
            else  // if unlocked
            {
                buttonLevel.DisplayLock(false);
            }


            for (int j = 0; j < buttonLevel.starDone; j++)
            {
                buttonLevel.stars[j].color = buttonLevel.starTrue;
            }

            btnLevelList.Add(button);
        }
        for (int i = 0; i < btnLevelList.Count; i++)
        {
            int index = i;
            btnLevelList[i].onClick.AddListener(() =>
            {
                if (index > 0 && DataController.Instance.currentGameData.starDoneInLevels[index + currentAct * 10 - 1] == 0)
                {
                    // Do sth
                }
                else
                {
                    UIManager.Instance.OpenUI<UIGameplay>();
                    LevelManager.Instance.OnLoadLevel(index + currentAct * 10);
                    _gameManager.ChangeState(GameState.GamePlay);
                    ClearListLevel();

                    AudioManager.instance.Play(Constant.AUDIO_SFX_PLAY);

                    CloseDirectly();
                }

            });
        }

        SetupUIListLevel();
    }

    private void SetupUIListLevel()
    {
        numStarGainedAct.text = GetTotalStarInAct(currentAct) + " / 30";
        levelDoneText.text = GetTotalLevelDoneInAct(currentAct) + "/10";
        numStarGainedGame.text = GetTotalStarInGame() + "/999";


        starDoneInAct.fillAmount = GetTotalStarInAct(currentAct) / 30f;
        levelDoneInAct.fillAmount = GetTotalLevelDoneInAct(currentAct) / 10f;
    }

    private void CheckStarInLevels()
    {
        if (DataController.Instance.currentGameData.starDoneInLevels.Count == 0)
        {
            for (int i = 0; i < 999; i++)
            {
                DataController.Instance.currentGameData.starDoneInLevels.Add(0);
            }
        }
    }
    private void CheckLevelDone()
    {
        if (DataController.Instance.currentGameData.levelDoneInGame.Count == 0)
        {
            for (int i = 0; i < 999; i++)
            {
                DataController.Instance.currentGameData.levelDoneInGame.Add(0);
            }
        }
    }
    private int GetTotalStarInAct(int act)
    {
        int totalStar = 0;
        for (int i = act * 10; i <= (act * 10) + 9; i++)
        {
            totalStar += DataController.Instance.currentGameData.starDoneInLevels[i];
        }
        return totalStar;
    }
    private int GetTotalLevelDoneInAct(int act)
    {
        int totalLevel = 0;
        for (int i = act * 10; i <= (act * 10) + 9; i++)
        {
            totalLevel += DataController.Instance.currentGameData.levelDoneInGame[i];
        }
        return totalLevel;
    }
    private int GetTotalStarInGame()
    {
        int totalStar = 0;
        for (int i = 0; i < DataController.Instance.currentGameData.starDoneInLevels.Count; i++)
        {
            totalStar += DataController.Instance.currentGameData.starDoneInLevels[i];
        }
        return totalStar;
    }

    public void BackButton()
    {
        UIManager.Instance.OpenUI<UiListAct>();
        ClearListLevel();

        AudioManager.instance.Play(Constant.AUDIO_SFX_BUTTON);

        CloseDirectly();
    }
    public void ClearListLevel()
    {
        btnLevelList.Clear();
        for(int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
}

[System.Serializable]
public class Act
{
    public List<LevelSO> levelSOList;
}
