using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Doge : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private LevelManager levelManager;
    private AnimationControllerDoge animDoge;
    private HealthDogeController healthDoge;
    
    private GameManager _gameManager;
    private LinesDrawer _linesDrawer;

    [Inject]
    private void Construct(GameManager gameManager, LinesDrawer linesDrawer)
    {
        _gameManager = gameManager;
        _linesDrawer = linesDrawer;
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        animDoge = GetComponent<AnimationControllerDoge>();
        healthDoge = GetComponent<HealthDogeController>();

        levelManager.OnWinLevel += ChangeBodyTypeToKinematic;
        levelManager.OnLoseLevel += ChangeBodyTypeToKinematic;
        _linesDrawer.OnEndDraw += ChangeBodyTypeToDynamic;
    }

    private void OnDestroy()
    {
        if (levelManager != null)
        {
            levelManager.OnWinLevel -= ChangeBodyTypeToKinematic;
            levelManager.OnLoseLevel -= ChangeBodyTypeToKinematic;
        }
        
        _linesDrawer.OnEndDraw -= ChangeBodyTypeToDynamic;
    }

    private void ChangeBodyTypeToDynamic()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    public SpriteRenderer GetSprite()
    {
        return sr;
    }
    public void SetColorForDoge(Color color)
    {
        sr.color = color;
    }
    public void ChangeBodyTypeToKinematic()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constant.SAW))
        {
            if (!_gameManager.IsState(GameState.GamePlay))
                return;
            animDoge.SetAnimForDoge(Constant.DOGE_ANIM_DIE);
            healthDoge.die = true;

            _gameManager.WhenLose();
        }
    }
}
