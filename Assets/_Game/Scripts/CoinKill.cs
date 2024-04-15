using UnityEngine;
using DG.Tweening;
using Zenject;

public class CoinKill : MonoBehaviour
{
    [SerializeField] private Transform coinImg, text;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private TextMesh textMesh;

    private Vector3 initPosImg, initPosText;
    public int amount;

    private ObjectPool _objectPool;

    [Inject]
    private void Construct (ObjectPool objectPool)
    {
        _objectPool = objectPool;
    }
    
    private void OnEnable()
    {
        textMesh.text = "+" + amount;

        sr.DOFade(1f, 0f);
        meshRenderer.material.DOFade(1f, 0f);

        initPosImg = coinImg.position;
        initPosText = text.position;

        sr.DOFade(0f, 1f).SetEase(Ease.InOutQuad);
        meshRenderer.material.DOFade(0f, 1f).SetEase(Ease.InOutQuad);

        coinImg.DOMove(new Vector3(initPosImg.x, initPosImg.y + 1.5f, initPosImg.x), 1f).SetEase(Ease.Linear);
        text.DOMove(new Vector3(initPosText.x, initPosText.y + 1.5f, initPosText.x), 1f).SetEase(Ease.Linear).OnComplete(HideCoin);
    }

    private void HideCoin()
    {
        coinImg.position = initPosImg;
        text.position = initPosText;
        _objectPool.ReturnToPool(Constant.KILL_COIN_VFX, this.gameObject);
    }
}
