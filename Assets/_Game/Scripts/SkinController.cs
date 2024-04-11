using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SkinController : Singleton<SkinController>
{
    public int currentSkinIndex;
    public int currentHp;
    public List<CharacterSO> charList;
    
    private DataController _dataController;

    [Inject]
    private void Construct(DataController dataController)
    {
        _dataController = dataController;
    }

    public void LoadDataSkin()
    {
        currentSkinIndex = _dataController.currentGameData.currentChar;
        currentHp = charList[currentSkinIndex].hp;
    }
}
