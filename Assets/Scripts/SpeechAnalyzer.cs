using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechAnalyzer : MonoBehaviour {

    public Material mouthSilent;
    public Material mouthSpeaking;
    public int sampleDataLength = 2048;

    Material currentMouth;
    Renderer renderer;

    float updateStep = 0.1f;
    float currentUpdateTime = 0f;

    float clipLoudness;
    float[] clipSampleData;

    AudioSource agentSpeech;

    private void Awake()
    {
        currentMouth = mouthSilent;
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
        agentSpeech = GetComponent<AudioSource>();

        clipSampleData = new float[sampleDataLength];
    }
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<AudioSource>().isPlaying)
        {
            currentUpdateTime += Time.deltaTime;
            if (currentUpdateTime >= updateStep)
            {
                currentUpdateTime = 0f;
                agentSpeech.clip.GetData(clipSampleData, agentSpeech.timeSamples);
                clipLoudness = 0f;
                foreach (var sample in clipSampleData)
                {
                    clipLoudness += Mathf.Abs(sample);
                }
                //Debug.Log(clipLoudness);     
            }

            if (clipLoudness < 1 && currentMouth != mouthSilent)
            {
                //Debug.Log("Im silent");
                renderer.sharedMaterial = mouthSilent;
                currentMouth = mouthSilent;
            }
            else if (clipLoudness > 1 && currentMouth != mouthSpeaking)
            {
                //Debug.Log("Im speaking");
                renderer.sharedMaterial = mouthSpeaking;
                currentMouth = mouthSpeaking;
            }
        }
	}
}
