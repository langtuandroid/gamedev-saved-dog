using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BuyDiamonds : MonoBehaviour
{
    [SerializeField] private Button _openShopButton;

    private UIManager _uiManager;
    private AudioManager _audioManager;
    private GameManager _gameManager;

    [Inject]
    private void Construct (UIManager uiManager, AudioManager audioManager, GameManager gameManager)
    {
        _uiManager = uiManager;
        _audioManager = audioManager;
        _gameManager = gameManager;
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
        if (_gameManager.GameState is GameState.Lose or GameState.Win)
        {
            return;
        }

        _uiManager.OpenUI<UIDiamondShop>();

        _audioManager.PlayBG(Constant.AUDIO_MUSIC_SHOP);
        _audioManager.Play(Constant.AUDIO_SFX_BUTTON);
    }
}
