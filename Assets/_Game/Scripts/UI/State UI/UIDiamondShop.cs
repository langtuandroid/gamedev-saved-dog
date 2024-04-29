using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIDiamondShop : UICanvas
{
    [SerializeField] private Button _pack1Button;
    [SerializeField] private Button _pack2Button;
    [SerializeField] private Button _pack3Button;
    [SerializeField] private Button _pack4Button;
    [SerializeField] private Button _closeButton;

    private IAPService _iapService;
    private AudioManager _audioManager;

    [Inject]
    private void Construct (IAPService iapService, AudioManager audioManager)
    {
        _iapService = iapService;
        _audioManager = audioManager;
    }
    
    private void OnEnable()
    {
        _pack1Button.onClick.AddListener(_iapService.BuyPack1);
        _pack2Button.onClick.AddListener(_iapService.BuyPack2);
        _pack3Button.onClick.AddListener(_iapService.BuyPack3);
        _pack4Button.onClick.AddListener(_iapService.BuyPack4);
        _closeButton.onClick.AddListener(CloseButton);
    }

    private void OnDisable()
    {
        _pack1Button.onClick.RemoveListener(_iapService.BuyPack1);
        _pack2Button.onClick.RemoveListener(_iapService.BuyPack2);
        _pack3Button.onClick.RemoveListener(_iapService.BuyPack3);
        _pack4Button.onClick.RemoveListener(_iapService.BuyPack4);
        _closeButton.onClick.RemoveListener(CloseButton);
    }
    
    private void CloseButton()
    {
        CloseImmediately();

        _audioManager.PlayBG(Constant.AUDIO_MUSIC_BG);
        _audioManager.Play(Constant.AUDIO_SFX_BUTTON); 
    }
}
