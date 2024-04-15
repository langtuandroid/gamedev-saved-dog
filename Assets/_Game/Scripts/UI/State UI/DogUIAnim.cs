using System.Collections;
using UnityEngine;
using Spine.Unity;

public class DogUIAnim : MonoBehaviour
{
    private const string START = "start_1";
    private const string SCARE = "scare";
    
    [SerializeField] private SkeletonGraphic skeletonAnimation;

    private Spine.AnimationState spineAnimationState;

    private void OnEnable()
    {
        spineAnimationState = skeletonAnimation.AnimationState;

        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        while(true)
        {
            spineAnimationState.SetAnimation(0, START, true);
            yield return new WaitForSeconds(2f);

            spineAnimationState.SetAnimation(0, SCARE, true);
            yield return new WaitForSeconds(1.5f);

        }
    }
}
