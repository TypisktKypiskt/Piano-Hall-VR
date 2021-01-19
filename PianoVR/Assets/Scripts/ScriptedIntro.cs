using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ScriptedIntro : MonoBehaviour
{
    [SerializeField]
    private AudioSource applauseAudioSource;
    [SerializeField]
    private Animation titleFade;

    void Start()
    {
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(FadeApplause(0f, 1f, 2f));
        
        yield return new WaitForSeconds(6.0f);
        StartCoroutine(FadeApplause(1f, 0f, 6f));
        
        yield return new WaitForSeconds(1.0f);
        titleFade.Play();
    }

    private IEnumerator FadeApplause(float from, float to, float fadeInTime)
    {
        float timeSinceFadeStart = 0f;
        applauseAudioSource.volume = from;

        if (!applauseAudioSource.isPlaying)
            applauseAudioSource.Play();

        while (!Mathf.Approximately(to, applauseAudioSource.volume))
        {
            timeSinceFadeStart += Time.deltaTime;
            applauseAudioSource.volume = Mathf.Lerp(from, to, timeSinceFadeStart / fadeInTime);

            yield return null;
        }
    }
}
