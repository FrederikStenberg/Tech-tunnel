using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadChange : MonoBehaviour {

    Renderer rend;
    public Material[] mat;
    public bool talking;
    void Start ()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = mat[0];
    }
	
	void Update () {
		if (talking)
        {
            rend.sharedMaterial = mat[1];
        }
        else
        {
            rend.sharedMaterial = mat[0];
        }
	}
}
