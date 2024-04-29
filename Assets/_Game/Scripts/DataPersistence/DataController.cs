using UnityEngine;
using Zenject;

public class DataController : MonoBehaviour, IDataPersistence
{
    public GameData currentGameData;
    
    /*private UIManager _uiManager;

    [Inject]
    private void Construct (UIManager uiManager)
    {
        _uiManager = uiManager;
    }*/
    
    public void LoadData(GameData data)
    {
        currentGameData = data;

        //_uiManager.GetUI<UIMainMenu>().UpdateCoinText();
        
        /*if (_uiManager.IsOpened<UIGameplay>())
        {
            _uiManager.CloseUI<UIMainMenu>();
        }*/
    }

    public void SaveData(ref GameData data)
    {
        data = currentGameData;
    }
}
