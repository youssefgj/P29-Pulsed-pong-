using UnityEngine;
using System.Collections;

public class BackgroundMusicController : MonoBehaviour
{
    [Header("References")]
    public PersonStateController stateController; // Source of current PersonState
    public AudioSource audioSource;               // AudioSource for background music

    [Header("Music Clips")]
    public AudioClip relaxedClip; // Music for Relax state
    public AudioClip normalClip;  // Music for Normal state
    public AudioClip tenseClip;   // Music for Tense state

    [Header("Settings")]
    public float fadeDuration = 1f; // Fade in/out duration when switching music

    private PersonState lastState;
    private float originalVolume;

    private bool isPaused = false;

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            originalVolume = audioSource.volume;
            audioSource.loop = true;
        }
    }

    void Start()
    {
        if (stateController == null || audioSource == null)
            return;

        // Initialize music based on the current state
        lastState = stateController.CurrentState;
        SetClipImmediate(lastState);
    }

    void Update()
    {
        if (stateController == null || audioSource == null || isPaused)
            return;

        PersonState currentState = stateController.CurrentState;

        // Switch music only when state changes
        if (currentState != lastState)
        {
            StopAllCoroutines();
            StartCoroutine(SwitchMusicCoroutine(currentState));
            lastState = currentState;
        }
    }

    // Immediately sets the correct music (used on start)
    private void SetClipImmediate(PersonState state)
    {
        audioSource.volume = originalVolume;
        audioSource.clip = GetClipForState(state);

        if (audioSource.clip != null)
            audioSource.Play();
    }

    // Smoothly fades out current music, switches clip, then fades in
    private IEnumerator SwitchMusicCoroutine(PersonState newState)
    {
        AudioClip nextClip = GetClipForState(newState);
        if (nextClip == null)
            yield break;

        // Fade out
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(originalVolume, 0f, t / fadeDuration);
            yield return null;
        }

        // Switch clip
        audioSource.clip = nextClip;
        audioSource.Play();

        // Fade in
        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, originalVolume, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = originalVolume;
    }

    // Returns the correct music clip for a given PersonState
    private AudioClip GetClipForState(PersonState state)
    {
        switch (state)
        {
            case PersonState.Relax:
                return relaxedClip;
            case PersonState.Normal:
                return normalClip;
            case PersonState.Tense:
                return tenseClip;
            default:
                return normalClip;
        }
    }
    public void PauseMusic()
    {
        isPaused = true;
        StopAllCoroutines();   // ⛔ stop fades immediately
        if (audioSource != null)
            audioSource.Pause();

    }
    public void ResumeMusic()
    {
        isPaused = false;

        if (audioSource != null)
            audioSource.UnPause();
    }
}
