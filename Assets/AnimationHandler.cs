using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class AnimationHandler : MonoBehaviour
{
    public enum Characters
    {
        rogue,
        tank
    }
    Characters activeCharacter;
    public enum states
    {
        idle,
        running,
        dashing,
        jumping,
        attacking,
        slamming
    }
    public states currentState;
    states previousState;
    TankAnimator tankAnimator;
    RogueAnimator rogueAnimator;
    public bool isLookingRight;
    // Start is called before the first frame update
    void Start()
    {
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
                    case states.jumping:
                        rogueAnimator.PlayJumpAnimation();
                        return;
                    case states.running:
                        animationToPlay = rogueAnimator.runAnimationName;
                        break;
                    case states.attacking:
                        rogueAnimator.PlayMainAttackAnimation();
                        return;
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
                    case states.jumping:
                        tankAnimator.PlayJumpAnimation();
                        return;
                    case states.running:
                        animationToPlay = tankAnimator.runAnimationName;
                        break;
                    case states.attacking:
                        tankAnimator.PlayMainAttackAnimation();
                        return;
                    default:
                        animationToPlay = tankAnimator.idleAnimationName;
                        break;
                }
                tankAnimator.PlayAnimation(animationToPlay);
                break;
        }
    }

    public void ReceiveCharacterChange(Characters charToChangeTo)
    { // Receive's the info that the character has changed from the player controller and enables/disables the rogue/tank's animations and animator scripts accordingly
        activeCharacter = charToChangeTo;
        if (activeCharacter == Characters.tank)
        {
            rogueAnimator.enabled = false;
            rogueAnimator.gameObject.GetComponent<MeshRenderer>().enabled = false;
            tankAnimator.enabled = true;
            tankAnimator.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            rogueAnimator.enabled = true;
            rogueAnimator.gameObject.GetComponent<MeshRenderer>().enabled = true;
            tankAnimator.enabled = false;
            tankAnimator.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
