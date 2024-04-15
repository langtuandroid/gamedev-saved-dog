using UnityEngine;
using Zenject;

public class UICanvas : MonoBehaviour
{
    public bool IsDestroyOnClose;

    protected UIManager _uiManager;

    [Inject]
    private void Construct (UIManager uiManager)
    {
        _uiManager = uiManager;
    }

    public void Setup()
    {
        _uiManager.AddBackUI(this);
        _uiManager.PushBackAction(this, BackKey);
    }

    private void BackKey()
    {}
    
    public void Open()
    {
        gameObject.SetActive(true);
    }
    
    public void Close(float delayTime)
    {
        Invoke(nameof(CloseImmediately), delayTime);
    }

    public void CloseImmediately()
    {
        _uiManager.RemoveBackUI(this);
        gameObject.SetActive(false);
        if (IsDestroyOnClose)
        {
            Destroy(gameObject);
        }
    }
}
