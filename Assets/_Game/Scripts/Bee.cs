using UnityEngine;
using Zenject;

public class Bee : MonoBehaviour
{
    private const float TIME_LENGTH_BOUNCING = 0.1f;
    private const float BOUNCE_FORCE = 0.4f;
    
    [SerializeField] private Rigidbody2D rb;

    private Transform targetDoge;
    private float flySpeed,cosineAngle, sineAngle, beeX, beeY, angleInRadians, angleToCharacter, timeCounterBouncing;
    private Vector2 bounceDirect, directToDoge, randomPointOnLine, finalPointOnLine, directToPoint, directToCharacter;
    private IState currentState;
    
    private int lives;
    
    public Transform TargetDoge { set { targetDoge = value; } }
    public Vector2 RandomPointOnLine { set { randomPointOnLine = value; } }
    public Vector2 FinalPointOnLine { set { finalPointOnLine = value; } }
    public int Lives { set { lives = value; } }

    public readonly FindState findState = new FindState();
    public readonly ApproachState approachState = new ApproachState();

    private Transform tf;
    public Transform TF
    {
        get
        {
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

        directToCharacter = targetDoge.position - transform.position;
        angleToCharacter = Mathf.Atan2(directToCharacter.y, directToCharacter.x) * Mathf.Rad2Deg;
        TF.rotation = Quaternion.AngleAxis(angleToCharacter+180, Vector3.forward);

    }

    private void OnInit()
    {
        timeCounterBouncing = 0;
        flySpeed = 0.08f;
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
            flySpeed = Mathf.Abs(flySpeed);
        }
    }

    public void FlyToDoge()
    {
        directToDoge = targetDoge.position - TF.position;
        directToDoge = directToDoge.normalized;
        rb.AddForce(directToDoge * (flySpeed * 3f), ForceMode2D.Impulse);
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
        rb.AddForce(directToPoint * (flySpeed * 1f), ForceMode2D.Impulse);

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

        cosineAngle = Mathf.Cos(angleInRadians);
        sineAngle = Mathf.Sin(angleInRadians);

        beeX = bounceDirect.x * cosineAngle - bounceDirect.y * sineAngle;
        beeY = bounceDirect.x * sineAngle + bounceDirect.y * cosineAngle;

        bounceDirect = new Vector2(beeX, beeY);

        rb.AddForce(bounceDirect * BOUNCE_FORCE, ForceMode2D.Impulse);

        flySpeed = -1 * flySpeed;
        timeCounterBouncing = TIME_LENGTH_BOUNCING;
    }
}
