using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class HealthDogeController : MonoBehaviour
{
    [SerializeField] private int maxHealth; public int MaxHealth { set { maxHealth = value; } }

    [SerializeField] private float coolDownStingLength, coolDownDogHurt;

    [SerializeField] private HealthBarDoge healthBar;

    private AnimationControllerDoge animDoge;

    private int currentHealth; public int CurrentHealth { get { return currentHealth; } } 
    private float couterTimeSting, counterTimeHurt;
    Vector3 offsetText = new Vector3(0f, 0.7f, 0f);
    Vector3 offsetBlood = new Vector3(0f, 0.4f, 0f);

    [HideInInspector] public bool hit, die;

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
        couterTimeSting = 0;
        counterTimeHurt = 0;

        healthBar.SetHealthBar(currentHealth, maxHealth);
        hit = false;
        die = false;
    }
    
    private void Update()
    {
        if (couterTimeSting > 0)
        {
            couterTimeSting -= Time.deltaTime;
        }
        if (counterTimeHurt > 0)
        {
            counterTimeHurt -= Time.deltaTime;
        }
    }

    private void LoseHealth()
    {
        if (couterTimeSting <= 0)
        {
            if (currentHealth > 0)
            {
                currentHealth--;
                healthBar.SetHealthBar(currentHealth, maxHealth);
                couterTimeSting = coolDownStingLength;

                ShowDamageText();
                ShowBloodEffect();
            }
            if (currentHealth <= 0)
            {
                die = true;
                OffHealthBar();
                animDoge.SetAnimForDoge(Constant.DOGE_ANIM_HURT);
                _gameManager.WhenLose();
            }
        }
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
            return;
        if (collision.gameObject.CompareTag(Constant.BEE))
        {
            _audioManager.Play(Constant.AUDIO_SFX_STING);
            DogeHurt();
            LoseHealth();
            if (die)
                return;
            hit = true;
            animDoge.SetAnimForDoge(Constant.DOGE_ANIM_GET_HIT);
        }
    }

    public void SetAnimWin()
    {
        animDoge.SetAnimForDoge(Constant.DOGE_ANIM_WIN);
    }
    public void OffHealthBar()
    {
        healthBar.TurnOffHealthBar();
    }
    private void DogeHurt()
    {
        if (counterTimeHurt <= 0)
        {
            counterTimeHurt = coolDownDogHurt;
            _audioManager.Play(Constant.AUDIO_SFX_DOGHURT);
            _phoneVibrate.VibrateDevice();
        }
    }
}
