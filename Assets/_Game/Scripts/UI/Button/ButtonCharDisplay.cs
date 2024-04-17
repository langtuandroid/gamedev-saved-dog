using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCharDisplay : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    [SerializeField] private GameObject selectedIcon;
    [SerializeField]
    private TextMeshProUGUI _ownedText;

    public Image CharacterImage => characterImage;
    public GameObject SelectedIcon => selectedIcon;
    public TextMeshProUGUI OwnedText => _ownedText;
}