using System.Collections;
using UnityEngine;
using Zenject;

public class Spike : MonoBehaviour
{
    [SerializeField]
    private float _timeToDead;

    private TimerUI clockTimerUI;

    private Color dogeColor;
    private GameManager _gameManager;
    private LevelManager _levelManager;

    [Inject]
    private void Construct(GameManager gameManager, LevelManager levelManager)
    {
        _gameManager = gameManager;
        _levelManager = levelManager;
    }
    
    private void Start()
    {
        clockTimerUI = FindObjectOfType<TimerUI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(Constant.DOGE))
        {
            return;
        }

        if (clockTimerUI == null)
        {
            return;
        }

        if (clockTimerUI.RemainingDuration <= 0)
            return;
        _levelManager.currentLevel.DestroyAllBees();
        StartCoroutine(KillOther(other));
    }
    private IEnumerator KillOther(Collider2D other)
    {
        other.gameObject.GetComponent<AnimationControllerDoge>().SetAnimForDoge(Constant.DOGE_ANIM_DIE);
        other.gameObject.GetComponent<HealthDogeController>().die = true;

        if (_gameManager.IsState(GameState.GamePlay))
        {
            _gameManager.WhenLose();
        }
        
        yield return new WaitForSeconds(_timeToDead);
    }
}
