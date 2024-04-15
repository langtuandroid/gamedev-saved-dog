using UnityEngine;
using Zenject;

public class Bee : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;

    [SerializeField] private Transform beeRenderer;
    [SerializeField] private Transform targetDoge; public Transform TargetDoge { set { targetDoge = value; } }
    [SerializeField] private float bounceForce;

    [SerializeField] private float timeLengthBouncing;

    public LayerMask layerMask;

    private float timeCounterBouncing;

    private float cosAngle, sinAngle, bX, bY;
    private Vector2 bounceDirect, directToDoge;
    private Vector2 randomPointOnLine; public Vector2 RandomPointOnLine { set { randomPointOnLine = value; } }
    private Vector2 finalPointOnLine; public Vector2 FinalPointOnLine { set { finalPointOnLine = value; } }
    private Vector2 directToPoint;

    private Vector2 directToDog;
    private float angleToDog;

    private IState currentState;

    public float angleInRadians;

    private int lives; public int Lives { set { lives = value; } }
    private float random;

    public FindState findState = new FindState();
    public ApproachState approachState = new ApproachState();

    private Transform tf;
    public Transform TF {
        get {
            if (tf == null) {
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
    
    void Start()
    {
        OnInit();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetDoge == null)
        {
            return;
        }

        if (currentState != null)
        {
            currentState.OnExecute(this);
        }

        directToDog = targetDoge.position - transform.position;
        angleToDog = Mathf.Atan2(directToDog.y, directToDog.x) * Mathf.Rad2Deg;
        TF.rotation = Quaternion.AngleAxis(angleToDog+180, Vector3.forward);

    }

    private void OnInit()
    {
        timeCounterBouncing = 0;
        ChangeState(new FindState());
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }
        currentState = newState;
        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
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
        this.lives--;
        if (lives <= 0)
        {
            _cheerNotify.KeepStreak();
            _audioManager.Play(Constant.AUDIO_SFX_BEE_DEAD);

            ChooseEffect();

            _objectPool.ReturnToPool(Constant.BEE, this.gameObject);
            return;
        }

        
        ShowEffect(Constant.PAR_KNOCK_VFX);

        //Vector3 directKnockBack = TF.position - targetDoge.position;

        //float distance = directKnockBack.magnitude;
        //directKnockBack = directKnockBack.normalized;

        //rb.AddForce(directKnockBack * 5f / distance, ForceMode2D.Impulse);
    }

    private void ChooseEffect()
    {
        random = Random.Range(0, 101);

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

    public bool CanSeeDoge()
    {
        directToDoge = targetDoge.position - TF.position;
        RaycastHit2D hit = Physics2D.Linecast(TF.position, targetDoge.position, layerMask);
        return hit.collider != null ? true : false;
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

    public void SetNoForce()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constant.BEE))
        {
            return;
        }

        //bounceDirect = collision.contacts[0].normal;
        bounceDirect = targetDoge.position - TF.position;

        bounceDirect.Normalize();

        cosAngle = Mathf.Cos(angleInRadians);
        sinAngle = Mathf.Sin(angleInRadians);

        bX = bounceDirect.x * cosAngle - bounceDirect.y * sinAngle;
        bY = bounceDirect.x * sinAngle + bounceDirect.y * cosAngle;

        bounceDirect = new Vector2(bX, bY);

        rb.AddForce(bounceDirect * bounceForce, ForceMode2D.Impulse);

        moveSpeed = -1 * moveSpeed;
        timeCounterBouncing = timeLengthBouncing;
    }
}
