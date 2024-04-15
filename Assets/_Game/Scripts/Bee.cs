using UnityEngine;
using Zenject;

public class Bee : MonoBehaviour
{
    private const float TIME_LENGTH_BOUNCING = 0.1f;
    private const float BOUNCE_FORCE = 0.4f;
    
    [SerializeField] private Rigidbody2D rb;

    private Transform targetDoge;
    private float moveSpeed,cosAngle, sinAngle, bX, bY, angleInRadians, angleToDog, timeCounterBouncing;
    private Vector2 bounceDirect, directToDoge, randomPointOnLine, finalPointOnLine, directToPoint, directToDog;
    private IState currentState;
    
    private int lives;
    
    public Transform TargetDoge { set { targetDoge = value; } }
    public Vector2 RandomPointOnLine { set { randomPointOnLine = value; } }
    public Vector2 FinalPointOnLine { set { finalPointOnLine = value; } }
    public int Lives { set { lives = value; } }

    public readonly FindState findState = new FindState();
    public readonly ApproachState approachState = new ApproachState();

    private Transform tf;
    public Transform TF {
        get {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }

    private AudioManager _audioManager;
    private ObjectPool _objectPool;
    private CheerNotify _cheerNotify;

    [Inject]
    private void Construct(AudioManager audioManager, ObjectPool objectPool, CheerNotify cheerNotify)
    {
        _audioManager = audioManager;
        _objectPool = objectPool;
        _cheerNotify = cheerNotify;
    }

    private void Start()
    {
        OnInit();
    }

    private void Update()
    {
        if (targetDoge == null)
        {
            return;
        }

        currentState?.OnExecute(this);

        directToDog = targetDoge.position - transform.position;
        angleToDog = Mathf.Atan2(directToDog.y, directToDog.x) * Mathf.Rad2Deg;
        TF.rotation = Quaternion.AngleAxis(angleToDog+180, Vector3.forward);

    }

    private void OnInit()
    {
        timeCounterBouncing = 0;
        moveSpeed = 0.08f;
        ChangeState(new FindState());
    }

    public void ChangeState(IState newState)
    {
        currentState?.OnExit(this);
        currentState = newState;
        currentState?.OnEnter(this);
    }

    public void CountTimeWhenBouncing()
    {
        if (timeCounterBouncing > 0)
        {
            timeCounterBouncing -= Time.deltaTime;
        } else
        {
            moveSpeed = Mathf.Abs(moveSpeed);
        }
    }

    public void FlyToDoge()
    {
        directToDoge = targetDoge.position - TF.position;
        directToDoge = directToDoge.normalized;
        rb.AddForce(directToDoge * (moveSpeed * 3f), ForceMode2D.Impulse);
    }

    public void KnockBack()
    {
        lives--;
        
        if (lives <= 0)
        {
            _cheerNotify.KeepStreak();
            _audioManager.Play(Constant.AUDIO_SFX_BEE_DEAD);

            ChooseEffect();

            _objectPool.ReturnToPool(Constant.BEE, gameObject);
            return;
        }
        
        ShowEffect(Constant.PAR_KNOCK_VFX);
    }

    private void ChooseEffect()
    {
        var random = Random.Range(0, 101);

        if (random <= 12)
        {
            ShowEffect(Constant.HEADSHOT_VFX);
            KillCoinEffect(3);
        } else
        {
            ShowEffect(Constant.KILL_VFX);
            KillCoinEffect(1);
        }
    }

    private void KillCoinEffect(int coin)
    {
        GameObject obj = _objectPool.GetFromPool(Constant.KILL_COIN_VFX);
        obj.GetComponent<CoinKill>().amount = coin;
        obj.transform.position = transform.position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
    }

    private void ShowEffect(string effect)
    {
        GameObject obj = _objectPool.GetFromPool(effect);
        obj.transform.position = TF.position;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
    }

    public void FlyRandomToLine()
    {
        directToPoint = randomPointOnLine - (Vector2)TF.position;
        directToPoint.Normalize();

        TF.Translate(directToPoint * (2f * Time.deltaTime));
    }

    public void FlyToFinalPointOnLine()
    {
        directToPoint = finalPointOnLine - (Vector2)TF.position;
        directToPoint.Normalize();
        rb.AddForce(directToPoint * (moveSpeed * 1f), ForceMode2D.Impulse);

    }

    public void SetAngle(float angle)
    {
        angleInRadians = angle;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constant.BEE))
        {
            return;
        }

        bounceDirect = targetDoge.position - TF.position;

        bounceDirect.Normalize();

        cosAngle = Mathf.Cos(angleInRadians);
        sinAngle = Mathf.Sin(angleInRadians);

        bX = bounceDirect.x * cosAngle - bounceDirect.y * sinAngle;
        bY = bounceDirect.x * sinAngle + bounceDirect.y * cosAngle;

        bounceDirect = new Vector2(bX, bY);

        rb.AddForce(bounceDirect * BOUNCE_FORCE, ForceMode2D.Impulse);

        moveSpeed = -1 * moveSpeed;
        timeCounterBouncing = TIME_LENGTH_BOUNCING;
    }
}
