using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentAnimationController : MonoBehaviour {

    MoveTo script;
    string state;
    Animator anim;

	// Use this for initialization
	void Start () {
        script = transform.parent.GetComponent<MoveTo>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        state = script.agentState;

        switch(state)
        {
            case "Idle":
                break;

            case "FlyingTowards":
                break;

            case "Waving":
                anim.SetTrigger("Waving");
                state = "Idle";
                break;

            case "Spin":
                break;

            case "ToSleep":
                break;
        }
	}
}
