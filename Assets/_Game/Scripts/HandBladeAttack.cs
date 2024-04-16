using System.Collections;
using UnityEngine;
using Zenject;

public class HandBladeAttack : MonoBehaviour
{
    [SerializeField] GameObject hand, dashline, parent;
    private bool slided;
    
    private TimeManager _timeManager;

    [Inject]
    private void Construct(TimeManager timeManager)
    {
        _timeManager = timeManager;
    }
    
    private void OnEnable()
    {
        slided = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            slided = true;
        }

        if (!slided)
        {
            return;
        }

        hand.SetActive(false);
        dashline.SetActive(false);
        StartCoroutine(BackToGameplay());
        slided = false;
    }

    private IEnumerator BackToGameplay()
    {
        yield return new WaitForSeconds(0.4f);
        _timeManager.NormalTime();
        parent.SetActive(false);
    }
}
