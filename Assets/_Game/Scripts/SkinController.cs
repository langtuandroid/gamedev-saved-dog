using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SkinController : MonoBehaviour
{
    [SerializeField] private List<CharacterSO> charList;
    
    private int currentSkinIndex;
    private int currentHp;
    private DataController _dataController;

    public int CurrentSkinIndex => currentSkinIndex;
    public int CurrentHp => currentHp;

    [Inject]
    private void Construct(DataController dataController)
    {
        _dataController = dataController;
    }

    public void LoadDataSkin()
    {
        currentSkinIndex = _dataController.currentGameData.currentChar;
        currentHp = charList[currentSkinIndex].health;
    }
}
