using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuyDiamonds : MonoBehaviour
{
    [SerializeField] private Button _openShopButton;

    private UIManager _uiManager;
    private AudioManager _audioManager;

    [Inject]
    private void Construct (UIManager uiManager, AudioManager audioManager)
    {
        _uiManager = uiManager;
        _audioManager = audioManager;
    }

    private void OnEnable()
    {
        _openShopButton.onClick.AddListener(OpenShop);
    }

    private void OnDisable()
    {
        _openShopButton.onClick.RemoveListener(OpenShop);
    }

    private void OpenShop()
    {
        _uiManager.OpenUI<UIDiamondShop>();

        _audioManager.PlayBG(Constant.AUDIO_MUSIC_SHOP);
        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);
    }
}
