using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Beehive : MonoBehaviour
{
    [SerializeField] private Transform generateBeePoint;
    [SerializeField] private int quantityBee;
    [SerializeField] private float spawnInterval;
    [SerializeField] private List<Transform> dogePosList;
    
    private int randomIndex, randomDog, randomLives;
    private Vector2[] pointsOnLine;
    private Vector2 randomVector;
    private List<Bee> beeGroup = new List<Bee>();
    
    private AudioManager _audioManager;
    private ObjectPool _objectPool;
    private LinesDrawer _linesDrawer;

    [Inject]
    private void Construct(AudioManager audioManager, ObjectPool objectPool, LinesDrawer linesDrawer)
    {
        _audioManager = audioManager;
        _objectPool = objectPool;
        _linesDrawer = linesDrawer;
    }

    private void Start()
    {
        _linesDrawer.OnEndDraw += BeginGenerate;
    }
    
    private void OnDestroy()
    {
       _linesDrawer.OnEndDraw -= BeginGenerate;
        DestroyAllBees();
    }

    private IEnumerator SpawnBees()
    {
        for (int i = 0; i < quantityBee; i++)
        {
            GameObject obj = _objectPool.GetFromPool(Constant.BEE);
            obj.SetActive(true);
            Bee bee1 = Cache.GetBeeGameObject(obj);

            randomLives = Random.Range(3, 11);
            bee1.Lives = randomLives;

            bee1.TF.position = generateBeePoint.position;

            randomDog = Random.Range(0, dogePosList.Count);
            bee1.TargetDoge = dogePosList[randomDog];
            
            bee1.RandomPointOnLine = GetRandomPointOnLine(pointsOnLine);
            
            if (i < quantityBee/2)
            {
                if (i < pointsOnLine.Length)
                {
                    bee1.FinalPointOnLine = pointsOnLine[0];
                    bee1.SetAngle(90f);
                }
            } else
            {
                bee1.FinalPointOnLine = pointsOnLine[pointsOnLine.Length - 1];
                bee1.SetAngle(180f);
            }
            beeGroup.Add(bee1);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void BeginGenerate() 
    {
        pointsOnLine = _linesDrawer.GetArrayPointsOfLine();

        _audioManager.Play(Constant.AUDIO_SFX_BEE);

        StartCoroutine(SpawnBees());
    } 
    
    public void DestroyAllBees()
    {
        _audioManager.Pause(Constant.AUDIO_SFX_BEE);
        foreach (Bee bee in beeGroup)
        {
            if (bee.gameObject.activeSelf)
            {
                _objectPool.ReturnToPool(Constant.BEE, bee.gameObject);
            }
        }

        beeGroup.Clear();
    }
    
    private Vector2 GetRandomPointOnLine(Vector2[] lines)
    {
        if (lines == null || lines.Length == 0)
        {
            return Vector2.zero;
        }

        randomIndex = Random.Range(0, lines.Length);
        randomVector = lines[randomIndex];
        return randomVector;
    }
}
