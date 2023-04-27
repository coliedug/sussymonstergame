using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandlerScript : MonoBehaviour
{

    #region Inspector
    // [SpineAnimation] attribute allows an Inspector dropdown of Spine animation names coming form SkeletonAnimation.
    [SpineAnimation]
    public string runAnimationName;

    [SpineAnimation]
    public string idleAnimationName;

   // [SpineAnimation]
    //public string shootAnimationName;

    [SpineAnimation]
    public string jumpAnimationName;

    [Header("Transitions")]
   // [SpineAnimation]
   // public string idleTurnAnimationName;

    [SpineAnimation]
    public string runToIdleAnimationName;

    public float runWalkDuration = 1.5f;

    [Header("Character Transitions")]
    [SpineAnimation]
    public string rogueInAnimationName;
    public string rogueOutAnimationName;
    public string tankInAnimationName;
    public string tankOutAnimationName;
    #endregion

    SkeletonAnimation skeletonAnimation;

    // Spine.AnimationState and Spine.Skeleton are not Unity-serialized objects. You will not see them as fields in the inspector.
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;

    public enum states
    {
        idle,
        jumping,
        running,
        switching
    }
    states previousanimationState;
    public states currentState;
    public bool facingRight;

    void Start()
    {
        // Make sure you get these AnimationState and Skeleton references in Start or Later.
        // Getting and using them in Awake is not guaranteed by default execution order.
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
        spineAnimationState.SetAnimation(0, idleAnimationName, true);

        previousanimationState = currentState;
        currentState = states.idle;
    }
    private void Update()
    {
        if (previousanimationState != currentState)
        {
            PlayNewAnimation();
        }

        if ((skeletonAnimation.skeleton.ScaleX < 0) != facingRight)
        {  // Detect changes in model.facingLeft
            skeletonAnimation.Skeleton.ScaleX = facingRight ? -1f : 1f;
        }
    }

    void PlayNewAnimation()
    {
        Debug.Log("Playing new animation");
        string nextAnimation;
        if (previousanimationState != currentState)
        {
            previousanimationState = currentState;
            PlayNewAnimation();
        }
        if (currentState == states.switching)
        {
            nextAnimation = jumpAnimationName;
            currentState = states.idle;
        }
        else if (currentState == states.jumping)
        {
            nextAnimation = jumpAnimationName;
            currentState = states.idle;
        }
        else if (currentState == states.running)
        {
            nextAnimation = runAnimationName;
        }
        else
        {
            nextAnimation = idleAnimationName;
        }

        skeletonAnimation.AnimationState.SetAnimation(0, nextAnimation, true);
    }

}
