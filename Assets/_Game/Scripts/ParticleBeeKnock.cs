using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBeeKnock : MonoBehaviour
{
    private ParticleSystem knockVFX;
    void Awake()
    {
        knockVFX = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        knockVFX.Play();
        Invoke("StopParticleSystem", 0.4f);
    }

    private void StopParticleSystem()
    {
        knockVFX.Stop();
        ObjectPool.Instance.ReturnToPool(Constant.PAR_KNOCK_VFX, this.gameObject);
    }
}
