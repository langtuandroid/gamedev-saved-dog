using UnityEngine;
using Zenject;

public class Doge : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private AnimationControllerDoge animDoge;
    private HealthDogeController healthDoge;
    
    private GameManager _gameManager;
    private LinesDrawer _linesDrawer;
    private LevelManager _levelManager;

    [Inject]
    private void Construct(GameManager gameManager, LinesDrawer linesDrawer, LevelManager levelManager)
    {
        _gameManager = gameManager;
        _linesDrawer = linesDrawer;
        _levelManager = levelManager;
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        animDoge = GetComponent<AnimationControllerDoge>();
        healthDoge = GetComponent<HealthDogeController>();

        _levelManager.OnWinLevel += ChangeToKinematic;
        _levelManager.OnLoseLevel += ChangeToKinematic;
        _linesDrawer.OnEndDraw += ChangeToDynamic;
    }

    private void OnDestroy()
    {
        if (_levelManager != null)
        {
            _levelManager.OnWinLevel -= ChangeToKinematic;
            _levelManager.OnLoseLevel -= ChangeToKinematic;
        }
        
        _linesDrawer.OnEndDraw -= ChangeToDynamic;
    }

    private void ChangeToDynamic()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }

    private void ChangeToKinematic()
    {
        _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.angularVelocity = 0;
        _rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(Constant.SAW))
        {
            return;
        }

        if (!_gameManager.IsState(GameState.GamePlay))
            return;
        animDoge.SetAnimForDoge(Constant.DOGE_ANIM_DIE);
        healthDoge.die = true;

        _gameManager.Lose();
    }
}
