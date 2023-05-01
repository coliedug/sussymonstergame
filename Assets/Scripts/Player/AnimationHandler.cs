using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class AnimationHandler : MonoBehaviour
{
    public enum Characters
    {
        rogue,
        tank
    }
    public Characters activeCharacter;
    public enum states
    {
        idle,
        running,
        dashing,
        jumping,
        attacking,
        slamming,
        swapping
    }
    public states currentState;
    states previousState;
    TankAnimator tankAnimator;
    RogueAnimator rogueAnimator;
    public bool isLookingRight;
    [SerializeField] GameObject audioObject;
    public AudioSource soundEffects;
    public AudioClip attackSound, jumpSound, slamSound;

    void Start()
    {
        soundEffects = audioObject.GetComponent<AudioSource>();
        tankAnimator = GetComponentInChildren<TankAnimator>();
        rogueAnimator = GetComponentInChildren<RogueAnimator>();

        activeCharacter = Characters.rogue;
        rogueAnimator.enabled = true;
        rogueAnimator.gameObject.GetComponent<MeshRenderer>().enabled = true;
        tankAnimator.enabled = false;
        tankAnimator.gameObject.GetComponent<MeshRenderer>().enabled = false;

        rogueAnimator.PlayAnimation(rogueAnimator.idleAnimationName);
        currentState = states.idle;
        previousState = currentState;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != previousState)
        {
            PlayNewAnimation();
        }
        previousState = currentState;
        switch (activeCharacter)
        {
            case Characters.rogue:
                rogueAnimator.isLookingRight = isLookingRight;
                break;
            case Characters.tank:
                tankAnimator.isLookingRight = isLookingRight;
                break;
        }
    }

    public void TryMove(float velocity)
    {
        if (currentState != states.jumping)
        {
            currentState = (velocity == 0) ? states.idle : states.running;
        }
        switch (velocity)
        {
            case > 0:
                isLookingRight = true;
                break;
            case < 0:
                isLookingRight = false;
                break;
            default:
                break;
        }
    }
    public void TrySlam()
    {
        Debug.Log("Slam");
        currentState = states.slamming;
        tankAnimator.PlaySlamAnimation();
    }

    void PlayNewAnimation()
    {
        var animationToPlay = rogueAnimator.jumpAnimationName;

        //The handler calls the method in the respective animatorScript to play the desired animation
        switch (activeCharacter)
        {
            case Characters.rogue:
                rogueAnimator.isLookingRight = isLookingRight;
                switch (currentState)
                {
                    case states.running:
                        animationToPlay = rogueAnimator.runAnimationName;
                        break;
                    default:
                        animationToPlay = rogueAnimator.idleAnimationName;
                        break;
                }
                rogueAnimator.PlayAnimation(animationToPlay);
                break;
            case Characters.tank:
                tankAnimator.isLookingRight = isLookingRight;
                switch (currentState)
                {
                    case states.running:
                        animationToPlay = tankAnimator.runAnimationName;
                        break;
                    default:
                        animationToPlay = tankAnimator.idleAnimationName;
                        break;
                }
                tankAnimator.PlayAnimation(animationToPlay);
                break;
        }
    }

    public void ReceiveCharacterChange(Characters charToChangeTo)
    {
        StartCoroutine(CharChangeProcess(charToChangeTo));
    }
    IEnumerator CharChangeProcess(Characters startingChar)
    {
        if (startingChar != Characters.rogue)
        {
            rogueAnimator.PlayAnimationNoLoop(rogueAnimator.teleportOutAnimationName);
            yield return new WaitForSeconds(0.333f);
            activeCharacter = Characters.tank;
            rogueAnimator.enabled = false;
            rogueAnimator.gameObject.GetComponent<MeshRenderer>().enabled = false;
            tankAnimator.enabled = true;
            tankAnimator.gameObject.GetComponent<MeshRenderer>().enabled = true;
            tankAnimator.PlayAnimationNoLoop(tankAnimator.teleportInAnimationName);
            yield return new WaitForSeconds(0.333f);
            currentState = states.idle;
        }
        else
        {
            tankAnimator.PlayAnimationNoLoop(tankAnimator.teleportOutAnimationName);
            yield return new WaitForSeconds(0.333f);
            activeCharacter = Characters.rogue;
            rogueAnimator.enabled = true;
            rogueAnimator.gameObject.GetComponent<MeshRenderer>().enabled = true;
            tankAnimator.enabled = false;
            tankAnimator.gameObject.GetComponent<MeshRenderer>().enabled = false;
            rogueAnimator.PlayAnimationNoLoop(rogueAnimator.teleportInAnimationName);
            yield return new WaitForSeconds(0.333f);
            currentState = states.idle;
        }
    }

    public void TryAttack()
    {
        if (currentState == states.swapping) return;
        if (currentState == states.slamming) return;
        switch (activeCharacter)
        {
            case Characters.tank:
                tankAnimator.PlayMainAttackAnimation();
                break;
            case Characters.rogue:
                rogueAnimator.PlayMainAttackAnimation();
                break;
        }
        soundEffects.clip = attackSound;
        soundEffects.Play();
    }
    public void TryJump()
    {
        if (currentState == states.attacking) return;
        if (currentState == states.slamming) return;
        currentState = states.jumping;
        soundEffects.clip = jumpSound;
        soundEffects.Play();
    }
}
