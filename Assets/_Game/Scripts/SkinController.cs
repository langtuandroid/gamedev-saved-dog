using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinController : Singleton<SkinController>
{
    public int currentSkinIndex;
    public int currentHp;
    public List<CharacterSO> charList;

    public void LoadDataSkin()
    {
        currentSkinIndex = DataController.Instance.currentGameData.currentChar;
        currentHp = charList[currentSkinIndex].hp;
    }
}
