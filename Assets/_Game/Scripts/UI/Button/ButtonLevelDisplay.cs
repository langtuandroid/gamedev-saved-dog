using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLevelDisplay : MonoBehaviour
{
    [SerializeField] private Sprite starReceivedSprite;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private int starsToComplete;
    [SerializeField] private Image imageLevelBG;
    [SerializeField] private GameObject blurImage;
    [SerializeField] private List<Image> starsImageList;

    public Sprite StarReceivedSprite => starReceivedSprite;
    public int StarsToComplete => starsToComplete;
    public List<Image> Stars => starsImageList;

    public void LoadDataUnlocked(int numLevel, Sprite image, int star)
    {
        titleText.text = numLevel.ToString();
        imageLevelBG.sprite = image;
        starsToComplete = star;
        titleText.gameObject.SetActive(true);
    }
    
    public void DisplayLock(bool value)
    {
        blurImage.SetActive(value);
        titleText.gameObject.SetActive(!value);
    }
}
