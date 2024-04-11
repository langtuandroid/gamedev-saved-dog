using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBlood : MonoBehaviour
{
    private ParticleSystem bloodVFX;
    void Awake()
    {
        bloodVFX = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        bloodVFX.Play();
        Invoke("StopParticleSystem", 1f);
    }

    private void StopParticleSystem()
    {
        bloodVFX.Stop();
        ObjectPool.Instance.ReturnToPool(Constant.PAR_BLOOD_VFX, this.gameObject);
    }
}
