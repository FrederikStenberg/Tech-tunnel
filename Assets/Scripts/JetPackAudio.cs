using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPackAudio : MonoBehaviour {

    public MoveTo agentScript;
    AudioSource jetPackAudio;

	void Start () {
        jetPackAudio = GetComponent<AudioSource>();
	}
	
	void Update () {
		if (agentScript.anim.GetCurrentAnimatorStateInfo(0).IsName("StateController.Flying"))
        {
            jetPackAudio.enabled = true;
        }
        else
        {
            jetPackAudio.enabled = false;
        }
	}
}
