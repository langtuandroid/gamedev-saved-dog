using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    private Camera mainCamera;
    private bool slicing;
    private TrailRenderer bladeTrail;
    private Collider2D bladeCollider;
    private float counterSoundBlade, velocity;
    private Vector3 newPosition;

    public Vector3 direction { get; private set; }
    public float minSliceVelocity = 0.01f;
    public float swipeSoundLength;

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
        }  
        else if (Input.GetMouseButtonUp(0)) 
        {
            StopSlicing();
        }
        else if (slicing)
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

        direction = newPosition - TF.position;

        velocity = direction.magnitude / Time.deltaTime;

        bladeCollider.enabled = minSliceVelocity < velocity;

        TF.position = newPosition;

        if (velocity >= 20f)
        {
            if (counterSoundBlade <= 0)
            {
                counterSoundBlade = swipeSoundLength;
                AudioManager.instance.Play(Constant.AUDIO_SFX_BLADE);
            }
        } 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Constant.BEE))
        {
            Bee bee = Cache.GetBee(other); //other.GetComponent<Bee>();
            bee.KnockBack();

            AudioManager.instance.Play(Constant.AUDIO_SFX_BEE_STAB);
        }
    }
}
