using UnityEngine;
using Zenject;

public class Blade : MonoBehaviour
{
    private const float MIN_SLICE_VELOCITY = 0.01f;
    
    private Camera mainCamera;
    private bool slicing;
    private TrailRenderer bladeTrail;
    private Collider2D bladeCollider;
    private float counterSoundBlade, velocity;
    private Vector3 newPosition;

    private Vector3 Direction
    {
        get;
        set;
    }

    public float swipeSoundLength;

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
    
    private AudioManager _audioManager;

    [Inject]
    private void Construct(AudioManager audioManager)
    {
        _audioManager = audioManager;
    }

    private void OnEnable()
    {
        StopSlicing();
    }

    private void OnDisable()
    {
        StopSlicing();
    }

    private void Awake()
    {
        bladeCollider = GetComponent<Collider2D>();
        mainCamera = Camera.main;
        bladeTrail = GetComponentInChildren<TrailRenderer>();
        swipeSoundLength = 0.3f;
        counterSoundBlade = swipeSoundLength;
    }

    private void Update()
    {
        if (counterSoundBlade > 0)
        {
            counterSoundBlade -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartSlicing();
        } else if (Input.GetMouseButtonUp(0)) 
        {
            StopSlicing();
        } else if (slicing)
        {
            ContinueSlicing();
        }
    }

    private void StopSlicing()
    {
        slicing = false;
        bladeCollider.enabled = false;
        bladeTrail.enabled = false;
    }
    
    private void StartSlicing()
    {
        newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;

        TF.position = newPosition;

        slicing = true;
        bladeCollider.enabled = true;
        bladeTrail.enabled = true;
        bladeTrail.Clear();
    }
    
    private void ContinueSlicing()
    {
        newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;

        Direction = newPosition - TF.position;

        velocity = Direction.magnitude / Time.deltaTime;

        bladeCollider.enabled = MIN_SLICE_VELOCITY < velocity;

        TF.position = newPosition;

        if (!(velocity >= 20f))
        {
            return;
        }

        if (!(counterSoundBlade <= 0))
        {
            return;
        }

        counterSoundBlade = swipeSoundLength;
        _audioManager.Play(Constant.AUDIO_SFX_BLADE);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(Constant.BEE))
        {
            return;
        }

        Bee bee = Cache.GetBee(other);
        bee.KnockBack();

        _audioManager.Play(Constant.AUDIO_SFX_BEE_STAB);
    }
}
