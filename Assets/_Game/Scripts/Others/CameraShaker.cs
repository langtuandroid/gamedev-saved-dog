using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private new Transform camera;
    [SerializeField] private Vector3 positionStrength;
    [SerializeField] private Vector3 rotationStrength;


    private static event Action Shake;

    public static void Invoke()
    {
        Shake?.Invoke();
    }

    private void OnEnable() => Shake += CameraShake;
    private void OnDisable() => Shake -= CameraShake;

    private void CameraShake()
    {
        camera.DOComplete();
        camera.DOShakePosition(0.8f, positionStrength, 16, 90f, false, true).SetEase(Ease.InOutQuad);
        camera.DOShakeRotation(0.8f, rotationStrength, 16, 90f, true).SetEase(Ease.InOutQuad);
    }
}
