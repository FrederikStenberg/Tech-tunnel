using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForLight : MonoBehaviour {

	// Use this for initialization
	void Start () {
        foreach (Light light in FindObjectsOfType<Light>())
        {
            Debug.Log(light.name);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
