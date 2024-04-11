using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ParticleBlood : MonoBehaviour
{
    private ParticleSystem bloodVFX;
    private ObjectPool _objectPool;

    [Inject]
    private void Construct (ObjectPool objectPool)
    {
        _objectPool = objectPool;
    }

    private void Awake()
    {
        bloodVFX = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        bloodVFX.Play();
        Invoke(nameof(StopParticleSystem), 1f);
    }

    private void StopParticleSystem()
    {
        bloodVFX.Stop();
        _objectPool.ReturnToPool(Constant.PAR_BLOOD_VFX, gameObject);
    }
}
