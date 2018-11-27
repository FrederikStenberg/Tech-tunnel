using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEventSelector : StateMachineBehaviour {

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        EventSelector eSel = animator.gameObject.GetComponent<EventSelector>();
        eSel.startCour();
    }
}
