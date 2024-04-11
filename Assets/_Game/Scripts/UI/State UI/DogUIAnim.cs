using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class DogUIAnim : MonoBehaviour
{
    public SkeletonGraphic skeletonAnimation;

    public Spine.AnimationState spineAnimationState;
    void OnEnable()
    {
        //skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;

        StartCoroutine(DoSomeStuff());
    }

    IEnumerator DoSomeStuff()
    {
        while(true)
        {
            spineAnimationState.SetAnimation(0, "start_1", true);
            yield return new WaitForSeconds(2f);

            spineAnimationState.SetAnimation(0, "scare", true);
            yield return new WaitForSeconds(1.5f);

        }
    }
}
