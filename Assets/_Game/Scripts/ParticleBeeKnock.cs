using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ParticleBeeKnock : MonoBehaviour
{
    private ParticleSystem knockVFX;
    
    private ObjectPool _objectPool;

    [Inject]
    private void Construct (ObjectPool objectPool)
    {
        _objectPool = objectPool;
    }

    private void Awake()
    {
        knockVFX = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        knockVFX.Play();
        Invoke(nameof(StopParticleSystem), 0.4f);
    }

    private void StopParticleSystem()
    {
        knockVFX.Stop();
        _objectPool.ReturnToPool(Constant.PAR_KNOCK_VFX, gameObject);
    }
}
