using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCharDisplay : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    [SerializeField] private GameObject selectedIcon;
    [SerializeField] private TextMeshProUGUI _ownedText;
    [SerializeField] private GameObject adObject;
    [SerializeField] private TextMeshProUGUI adText;

    private int currentAd;
    private int maxAd;

    public Image CharacterImage => characterImage;
    public GameObject SelectedIcon => selectedIcon;
    public TextMeshProUGUI OwnedText => _ownedText;
    public GameObject ADObject => adObject;
    public TextMeshProUGUI ADText => adText;

    public int CurrentAd
    {
        get => currentAd;
        set => currentAd = value;
    }
    public int MaxAd
    {
        get => maxAd;
        set => maxAd = value;
    }
}