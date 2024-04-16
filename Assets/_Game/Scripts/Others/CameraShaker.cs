using UnityEngine;
using DG.Tweening;
using System;

public class CameraShaker : MonoBehaviour
{
    private static event Action OnShake;
    
    [SerializeField] private new Transform camera;
    [SerializeField] private Vector3 positionStrength;
    [SerializeField] private Vector3 rotationStrength;
    

    public static void Invoke()
    {
        OnShake?.Invoke();
    }

    private void OnEnable() => OnShake += CameraShake;
    private void OnDisable() => OnShake -= CameraShake;

    private void CameraShake()
    {
        camera.DOComplete();
        camera.DOShakePosition(0.8f, positionStrength, 16, 90f, false, true).SetEase(Ease.InOutQuad);
        camera.DOShakeRotation(0.8f, rotationStrength, 16, 90f, true).SetEase(Ease.InOutQuad);
    }
}
