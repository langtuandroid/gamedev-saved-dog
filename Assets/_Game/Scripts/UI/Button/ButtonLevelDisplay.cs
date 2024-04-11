using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLevelDisplay : MonoBehaviour
{
    public Color starTrue, starFalse;
    public Text title;
    public int starDone;
    public Image imageLevel;
    public List<Image> stars;
    public int numLevelToLoad;

    public GameObject blur;

    public void LoadDataUnlocked(int numLevel, Sprite image, int star)
    {
        this.numLevelToLoad = numLevel;
        this.title.text = "Level: " + numLevel;
        this.imageLevel.sprite = image;
        this.starDone = star;
    }
    public void DisplayLock(bool a)
    {
        blur.SetActive(a);
    }
}
