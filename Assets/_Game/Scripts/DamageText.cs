using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;
public class DamageText : MonoBehaviour
{

    private MeshRenderer meshRender;
    private Vector3 initPos;

    private Transform tf;
    
    private ObjectPool _objectPool;

    [Inject]
    private void Construct (ObjectPool objectPool)
    {
        _objectPool = objectPool;
    }
    
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

    private void Awake()
    {
        meshRender = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        meshRender.material.DOFade(1f, 0f);
        initPos = TF.position;

        TF.DOMove(new Vector3(initPos.x + Random.Range(-0.7f, 0.7f),initPos.y + 1f, initPos.z), 1f, false).SetEase(Ease.OutQuad);
        TF.DOScale(2f, 0.4f).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);
        meshRender.material.DOFade(0.2f, 1f).SetEase(Ease.OutQuad).OnComplete(HideText);
    }

    private void HideText()
    {
        _objectPool.ReturnToPool(Constant.DMG_TEXT, gameObject);
    }

    private void OnDisable()
    {
        TF.DOScale(1f, 0f);
    }
}
