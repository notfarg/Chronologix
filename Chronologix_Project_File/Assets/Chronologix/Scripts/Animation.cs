using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Animation : MonoBehaviour
{
    /* <summary>
      Everything needed to animate the character is here.
      Depending on the state of the character, a different animation should be playing.
      CURRENTLY, there needs to be a trigger that will prompt the state change.
      For example, if a character is moving and [MoveSpeed > 0], then it should switch to "Running" animation state. The player should default to "Idle" if nothing is triggering a state change.

      Not sure how to get this working with the player controller, but it's fairly straight forward and references should be set properly.
      </summary> */

    // Animation
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset idle, running, startJump, jumping, landing;
    public string currentState;
    public string currentAnimation;

    // Start is called before the first frame update
    void Awake()
    {
        currentState = "Idle";
        SetCharacterState(currentState);
    }

    // Update is called once per frame
    void Update()
    {
        /* // Starting jump animation. NEEDS WORK.
        if (isGrounded == false)
        {
            SetCharacterState("Jumping");
        } else
        {
            SetCharacterState("Idle");
        }

        // Sets character's animation state to running, then to idle if not. NEEDS WORK.
        if ()
        {
            SetCharacterState("Running");
        } else
        {
            SetCharacterState("Idle");
        }

        // Flips character when moved. It WAS using player controller's [currentMoveDirection] variable, but that was being wonky. Might have to put it on inputs? NEEDS WORK.
        if (currentMoveDirection.x == -1)
        {
            skeletonAnimation.transform.localScale = new Vector2(1f, 1f);
        } else if (currentMoveDirection.x == 1)
        {
            skeletonAnimation.transform.localScale = new Vector2(-1f, 1f);
        } */
    }

    // Calls animations for the character.
    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        if (animation.name.Equals(currentAnimation))
        {
            return;
        }
        skeletonAnimation.state.SetAnimation(0, animation, loop).TimeScale = timeScale;
        currentAnimation = animation.name;
    }

    // Identifying state of the character.
    public void SetCharacterState(string state)
    {
        if (state.Equals("Running"))
        {
            SetAnimation(running, true, 1f);
        }
        else if (state.Equals("Start Jump"))
        {
            SetAnimation(startJump, false, 1f);
        }
        else if (state.Equals("Jumping"))
        {
            SetAnimation(jumping, true, 1f);
        }
        else if (state.Equals("Landing"))
        {
            SetAnimation(landing, false, 1f);
        }
        else
        {
            SetAnimation(idle, true, 1f);
        }
    }
}
