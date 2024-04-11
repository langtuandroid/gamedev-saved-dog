using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Axit : MonoBehaviour
{
    [SerializeField] private Color deathColor;
    [SerializeField] private float timeToDead;

    private Timer clockTimer;
    private Color dogeColor;
    private GameManager _gameManager;

    [Inject]
    private void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    private void Start()
    {
        clockTimer = GameObject.FindObjectOfType<Timer>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Constant.DOGE))
        {
            if (clockTimer == null)
            {
                return;
            }
            else
            {
                if (clockTimer.RemainingDuration <= 0)
                    return;
            }
            StartCoroutine(DeadByAxit(other));
        }
    }
    private IEnumerator DeadByAxit(Collider2D other)
    {
        other.gameObject.GetComponent<AnimationControllerDoge>().SetAnimForDoge(Constant.DOGE_ANIM_DIE);
        other.gameObject.GetComponent<HealthDogeController>().die = true;
        //Destroy(other.gameObject, timeToDead);
        if (_gameManager.IsState(GameState.GamePlay))
            _gameManager.WhenLose();
        yield return new WaitForSeconds(timeToDead);
    }
}
