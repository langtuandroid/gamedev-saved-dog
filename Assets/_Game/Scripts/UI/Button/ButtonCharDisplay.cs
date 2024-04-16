using UnityEngine;
using UnityEngine.UI;

public class ButtonCharDisplay : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    [SerializeField] private GameObject selectedIcon;

    public Image CharacterImage => characterImage;
    public GameObject SelectedIcon => selectedIcon;
}