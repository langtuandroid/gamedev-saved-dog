using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroupDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] levelsInGroupText;
    [SerializeField] private Image groupImage;
    [SerializeField] private Image starImage;
    [SerializeField] private TextMeshProUGUI starsReceivedText;
    [SerializeField] private GameObject blurObject;
    [SerializeField] private TextMeshProUGUI starsToUnlockText;

    public void LoadDataLockedGroup(string title, Sprite image, int starDone)
    {
        foreach (var text in levelsInGroupText)
        {
            text.text = title;
        }
        
        groupImage.sprite = image;
        starsReceivedText.text = starDone + "/30";
    }
    
    public void LoadDataUnlockedGroup(int num)
    {
        starsToUnlockText.text = num.ToString();
    }
    
    public void LockGroup()
    {
        blurObject.SetActive(true);

        starsReceivedText.gameObject.SetActive(false);
        starImage.gameObject.SetActive(false);
    }
    
    public void UnlockGroup()
    {
        blurObject.SetActive(false);

        starsReceivedText.gameObject.SetActive(true);
        starImage.gameObject.SetActive(true);
    }
}
