using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActDisplay : MonoBehaviour
{
    public Text title;
    public Image actImage, star;
    public Text starDone;

    public GameObject blur;
    public Text numStarToUnlockText;

    public void LoadDataWhenUnlocked(string title, Sprite image, int starDone)
    {
        this.title.text = title;
        this.actImage.sprite = image;
        this.starDone.text = starDone + "/30";
    }
    public void LoadDataWhenLock(int num)
    {
        this.numStarToUnlockText.text = num.ToString();
    }
    public void DisplayLock()
    {
        blur.SetActive(true);

        starDone.gameObject.SetActive(false);
        star.gameObject.SetActive(false);
    }
    public void DisplayUnlock()
    {
        blur.SetActive(false);

        starDone.gameObject.SetActive(true);
        star.gameObject.SetActive(true);
    }
}
