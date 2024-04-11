using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : Singleton<DataController>, IDataPersistence
{
    public GameData currentGameData;
    public void LoadData(GameData data)
    {
        currentGameData = data;

        UIManager.Instance.GetUI<UIMainMenu>().UpdateCoinText();
        if (UIManager.Instance.IsOpened<UIGameplay>())
        {
            UIManager.Instance.CloseUI<UIMainMenu>();
        }

    }

    public void SaveData(ref GameData data)
    {
        data = currentGameData;
    }
}
