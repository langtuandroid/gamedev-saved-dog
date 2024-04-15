using UnityEngine;
using Zenject;

public class ParticleBlood : MonoBehaviour
{
    private ParticleSystem _bloodVFX;
    
    private ObjectPool _objectPool;

    [Inject]
    private void Construct (ObjectPool objectPool)
    {
        _objectPool = objectPool;
    }

    private void Awake()
    {
        _bloodVFX = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        _bloodVFX.Play();
        Invoke(nameof(StopParticleSystem), 1f);
    }

    private void StopParticleSystem()
    {
        _bloodVFX.Stop();
        _objectPool.ReturnToPool(Constant.PAR_BLOOD_VFX, gameObject);
    }
}
