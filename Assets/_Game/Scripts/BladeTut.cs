using UnityEngine;
using Zenject;

public class BladeTut : MonoBehaviour
{
    [SerializeField]
    private GameObject handAttack;
    
    private TimeManager _timeManager;

    [Inject]
    private void Construct(TimeManager timeManager)
    {
        _timeManager = timeManager;
    }
    
    private void OnEnable()
    {
        Invoke(nameof(AnimTut), 1.5f);
    }
    
    private void AnimTut()
    {
        _timeManager.SlowTime();
        handAttack.SetActive(true);
    }
}
