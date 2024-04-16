using UnityEngine;
using Zenject;

public class HealthDogeController : MonoBehaviour
{
    private readonly Vector3 offsetText = new Vector3(0f, 0.7f, 0f);
    private readonly Vector3 offsetBlood = new Vector3(0f, 0.4f, 0f);
    
    [SerializeField] private int maxHealth;
    [SerializeField] private float coolDownStingLength, coolDownDogHurt;
    [SerializeField] private HealthBarDoge healthBar;


    private AnimationControllerDoge animDoge;
    private int currentHealth;
    private float counterTimeSting;
    private float counterTimeHurt;

    [HideInInspector] public bool hit, die;

    private Transform tf;
    private Transform TF
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
    public int CurrentHealth => currentHealth;
    public int MaxHealth { set { maxHealth = value; } }
    
    private GameManager _gameManager;
    private AudioManager _audioManager;
    private ObjectPool _objectPool;
    private PhoneVibrate _phoneVibrate;

    [Inject]
    private void Construct(GameManager gameManager, AudioManager audioManager, ObjectPool objectPool, PhoneVibrate phoneVibrate)
    {
        _gameManager = gameManager;
        _audioManager = audioManager;
        _objectPool = objectPool;
        _phoneVibrate = phoneVibrate;
    }

    private void Start()
    {
        animDoge = GetComponent<AnimationControllerDoge>();

        currentHealth = maxHealth;
        counterTimeSting = 0;
        counterTimeHurt = 0;

        healthBar.SetHealthBar(currentHealth, maxHealth);
        hit = false;
        die = false;
    }
    
    private void Update()
    {
        if (counterTimeSting > 0)
        {
            counterTimeSting -= Time.deltaTime;
        }
        
        if (counterTimeHurt > 0)
        {
            counterTimeHurt -= Time.deltaTime;
        }
    }

    private void LoseHealth()
    {
        if (!(counterTimeSting <= 0))
        {
            return;
        }

        if (currentHealth > 0)
        {
            currentHealth--;
            healthBar.SetHealthBar(currentHealth, maxHealth);
            counterTimeSting = coolDownStingLength;

            ShowDamageText();
            ShowBloodEffect();
        }

        if (currentHealth > 0)
        {
            return;
        }

        die = true;
        DisableHealthBar();
        animDoge.SetAnimForDoge(Constant.DOGE_ANIM_HURT);
        _gameManager.Lose();
    }
    
    private void ShowDamageText()
    {
        GameObject obj = _objectPool.GetFromPool(Constant.DMG_TEXT);
        obj.transform.position = TF.position + offsetText;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
    }
    
    private void ShowBloodEffect()
    {
        GameObject obj = _objectPool.GetFromPool(Constant.PAR_BLOOD_VFX);
        obj.transform.position = transform.position + offsetBlood;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (die || !_gameManager.IsState(GameState.GamePlay))
        {
            return;
        }

        if (!collision.gameObject.CompareTag(Constant.BEE))
        {
            return;
        }

        _audioManager.Play(Constant.AUDIO_SFX_STING);
        DogeHurt();
        LoseHealth();

        if (die)
        {
            return;
        }
            
        hit = true;
        animDoge.SetAnimForDoge(Constant.DOGE_ANIM_GET_HIT);
    }

    public void SetWinAnimation()
    {
        animDoge.SetAnimForDoge(Constant.DOGE_ANIM_WIN);
    }
    
    public void DisableHealthBar()
    {
        healthBar.DisableHealthBar();
    }
    
    private void DogeHurt()
    {
        if (!(counterTimeHurt <= 0))
        {
            return;
        }

        counterTimeHurt = coolDownDogHurt;
        _audioManager.Play(Constant.AUDIO_SFX_DOGHURT);
        _phoneVibrate.VibrateDevice();
    }
}
