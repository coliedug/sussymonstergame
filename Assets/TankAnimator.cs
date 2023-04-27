using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAnimator : MonoBehaviour
{
    [Header("Attacks")]
    [SpineAnimation]
    public string attack1AnimationName;
    [SpineAnimation]
    public string attack2AnimationName;
    [SpineAnimation]
    public string attack3AnimationName;
    [SpineAnimation]
    public string dashAnimationName;
    [Header("Movement")]
    [SpineAnimation]
    public string jumpAnimationName;
    [SpineAnimation]
    public string runAnimationName;
    [Header("Utility")]
    [SpineAnimation]
    public string idleAnimationName;
    [SpineAnimation]
    public string deathAnimationName;
    [SpineAnimation]
    public string teleportInAnimationName;
    [SpineAnimation]
    public string teleportOutAnimationName;

    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;

    public bool isLookingRight;
    private void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
        spineAnimationState.SetAnimation(0, idleAnimationName, true);
    }

    public void PlayAnimation(string animationToPlay)
    {
        skeletonAnimation.AnimationState.SetAnimation(0, animationToPlay, true);
    }
    public void PlayJumpAnimation()
    {
        TrackEntry jumpTrack = skeletonAnimation.AnimationState.SetAnimation(1, jumpAnimationName, false);
        jumpTrack.AttachmentThreshold = 1f;
        jumpTrack.MixDuration = 0f;
        skeletonAnimation.AnimationState.AddEmptyAnimation(1, 0.1f, 0);
        skeletonAnimation.AnimationState.AddAnimation(0, idleAnimationName, true, 0);
    }

        public void PlayMainAttackAnimation()
    {
        int randomNum = UnityEngine.Random.Range(1, 3);
        switch (randomNum)
        {
            case 1:
                skeletonAnimation.AnimationState.SetAnimation(1, attack1AnimationName, false);
                break;
            case 2:
                skeletonAnimation.AnimationState.SetAnimation(1, attack2AnimationName, false);
                break;
            case 3:
                skeletonAnimation.AnimationState.SetAnimation(1, attack3AnimationName, false);
                break;
        }
        skeletonAnimation.AnimationState.AddAnimation(0, idleAnimationName, true, 0);
    }

    private void Update()
    {
        skeletonAnimation.Skeleton.ScaleX = isLookingRight ? -1f : 1f;
    }
}
