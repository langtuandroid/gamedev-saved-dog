using System.Collections;
using UnityEngine;
using Zenject;

public class Acid : MonoBehaviour
{
    [SerializeField] private float _timeToDead;

    private TimerUI _clockTimerUI;
    private Color _dogeColor;
    private GameManager _gameManager;

    [Inject]
    private void Construct(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    
    private void Start()
    {
        _clockTimerUI = FindObjectOfType<TimerUI>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(Constant.DOGE))
        {
            return;
        }

        if (_clockTimerUI == null)
        {
            return;
        }

        if (_clockTimerUI.RemainingDuration <= 0)
        {
            return;
        }
        
        StartCoroutine(DeadByAcid(other));
    }
    private IEnumerator DeadByAcid(Collider2D other)
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
