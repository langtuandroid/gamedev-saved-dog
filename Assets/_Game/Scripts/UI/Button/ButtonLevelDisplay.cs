using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLevelDisplay : MonoBehaviour
{
    [SerializeField] private Color starReceivedColor;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private int starsToComplete;
    [SerializeField] private Image imageLevelBG;
    [SerializeField] private GameObject blurImage;
    [SerializeField] private List<Image> starsImageList;

    public Color StarReceivedColor => starReceivedColor;
    public int StarsToComplete => starsToComplete;
    public List<Image> Stars => starsImageList;

    public void LoadDataUnlocked(int numLevel, Sprite image, int star)
    {
        titleText.text = "Level: " + numLevel;
        imageLevelBG.sprite = image;
        starsToComplete = star;
    }
    
    public void DisplayLock(bool value)
    {
        blurImage.SetActive(value);
    }
}
