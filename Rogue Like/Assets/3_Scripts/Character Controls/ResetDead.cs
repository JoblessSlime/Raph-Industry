using UnityEngine;

public class ResetDead : StateMachineBehaviour
{
    private bool hasFired = false;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Check if animation has played once and the event hasn't fired
        if (!hasFired && stateInfo.normalizedTime >= 0.98f)
        {
            hasFired = true;
            animator.SetBool("PlayerDead", false);  // Or trigger any event
        }
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasFired = false; // Reset flag when entering the state
    }
}
