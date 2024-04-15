using UnityEngine;
using DG.Tweening;
using Zenject;

public class DamageText : MonoBehaviour
{
    private MeshRenderer _meshRender;
    private Vector3 _initPos;
    private ObjectPool _objectPool;
    private Transform tf;
    private Transform TF
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

    [Inject]
    private void Construct (ObjectPool objectPool)
    {
        _objectPool = objectPool;
    }


    private void Awake()
    {
        _meshRender = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        _meshRender.material.DOFade(1f, 0f);
        _initPos = TF.position;

        TF.DOMove(new Vector3(_initPos.x + Random.Range(-0.7f, 0.7f),_initPos.y + 1f, _initPos.z), 1f, false).SetEase(Ease.OutQuad);
        TF.DOScale(2f, 0.4f).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);
        _meshRender.material.DOFade(0.2f, 1f).SetEase(Ease.OutQuad).OnComplete(HideText);
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
