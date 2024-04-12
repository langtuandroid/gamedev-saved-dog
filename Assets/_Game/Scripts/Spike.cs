using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Spike : MonoBehaviour
{
    [SerializeField] private Color deathColor;
    [SerializeField] private float timeToDead;

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
        clockTimerUI = GameObject.FindObjectOfType<TimerUI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Constant.DOGE))
        {
            if (clockTimerUI == null)
            {
                return;
            } else
            {
                if (clockTimerUI.RemainingDuration <= 0)
                    return;
            }
            _levelManager.currentLevel.DestroyAllBees();
            StartCoroutine(DeadBySpike(other));
        }
    }
    private IEnumerator DeadBySpike(Collider2D other)
    {
        other.gameObject.GetComponent<AnimationControllerDoge>().SetAnimForDoge(Constant.DOGE_ANIM_DIE);
        other.gameObject.GetComponent<HealthDogeController>().die = true;
        //Destroy(other.gameObject, timeToDead);
        if (_gameManager.IsState(GameState.GamePlay))
            _gameManager.WhenLose();

        yield return new WaitForSeconds(timeToDead);
    }
}
