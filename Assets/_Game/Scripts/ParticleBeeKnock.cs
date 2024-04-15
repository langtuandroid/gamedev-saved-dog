using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ParticleBeeKnock : MonoBehaviour
{
    private ParticleSystem _knockVFX;
    
    private ObjectPool _objectPool;

    [Inject]
    private void Construct (ObjectPool objectPool)
    {
        _objectPool = objectPool;
    }

    private void Awake()
    {
        _knockVFX = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        _knockVFX.Play();
        Invoke(nameof(StopParticleSystem), 0.4f);
    }

    private void StopParticleSystem()
    {
        _knockVFX.Stop();
        _objectPool.ReturnToPool(Constant.PAR_KNOCK_VFX, gameObject);
    }
}
