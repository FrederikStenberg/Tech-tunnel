using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechAnalyzer : MonoBehaviour {

    public Material smileSilent;
    public Material smileSpeaking;
    public Material noSmileSilent;
    public Material noSmileSpeaking;
    public int sampleDataLength = 2048;
    public float speakingFrequencyAdjuster;

    Material currentMouth;
    Renderer renderer;

    float updateStep = 0.1f;
    float currentUpdateTime = 0f;

    float clipLoudness;
    float[] clipSampleData;

    public MoveTo agentScript;

    AudioSource agentSpeech;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        renderer.enabled = true;
        agentSpeech = GetComponent<AudioSource>();
        clipSampleData = new float[sampleDataLength];
        if (agentScript.lookAwayOffsetEnabled)
            currentMouth = noSmileSilent;
        else
            currentMouth = smileSilent;
        renderer.sharedMaterial = currentMouth;
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
            }

            if (agentScript.lookAwayOffsetEnabled)
            {
                if (clipLoudness <= speakingFrequencyAdjuster && currentMouth != noSmileSilent)
                {
                    renderer.sharedMaterial = noSmileSilent;
                    currentMouth = noSmileSilent;
                }
                else if (clipLoudness > speakingFrequencyAdjuster && currentMouth != noSmileSpeaking)
                {
                    renderer.sharedMaterial = noSmileSpeaking;
                    currentMouth = noSmileSpeaking;
                }
            }
            else
            {
                if (clipLoudness <= speakingFrequencyAdjuster && currentMouth != smileSilent)
                {
                    renderer.sharedMaterial = smileSilent;
                    currentMouth = smileSilent;
                }
                else if (clipLoudness > speakingFrequencyAdjuster && currentMouth != smileSpeaking)
                {
                    renderer.sharedMaterial = smileSpeaking;
                    currentMouth = smileSpeaking;
                }
            }
        }
	}
}
