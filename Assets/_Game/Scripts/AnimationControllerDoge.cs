using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerDoge : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    private HealthDogeController healthDoge;

    // Render Skin
    private SkeletonData skeletonData = new SkeletonData();
    private List<Skin> skins;
    private Skin currentSkin;

    // Render Anim
    private Spine.AnimationState animationState;

    private void Start()
    {
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
        healthDoge = GetComponent<HealthDogeController>();

        // Setup Anim
        animationState = skeletonAnimation.AnimationState;

        // Setup Skin
        skeletonData = skeletonAnimation.state.Data.SkeletonData;
        skins = new List<Skin>(skeletonData.Skins.ToArray());

        SetAnimForDoge(Constant.DOGE_ANIM_IDLE);

        InvokeRepeating("Scare", 0.5f, 0.5f);
    }

    public void SetSkinForDoge(int skinIndex)
    {
        Constant constant = new Constant();
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();

        // Setup Skin
        skeletonData = skeletonAnimation.state.Data.SkeletonData;
        skins = new List<Skin>(skeletonData.Skins.ToArray());
        Skin skin = skins[constant.skins[skinIndex]];

        skeletonAnimation.Skeleton.SetSkin(skin);
        skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        skeletonAnimation.state.Apply(skeletonAnimation.skeleton);
    }
    public void SetAnimForDoge(string animIndex)
    {
        animationState.SetAnimation(0, animIndex, true);
        
    }
    private void Scare()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);

        foreach(Collider2D collider in colliders)
        {
            if (collider.CompareTag(Constant.BEE))
            {
                if (!healthDoge.hit && !healthDoge.die)
                {
                    SetAnimForDoge(Constant.DOGE_ANIM_SCARE);
                    break;
                } 
            }
        }
    }
}
