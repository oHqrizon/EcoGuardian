using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] backgroundAudioTracks;
    private AudioSource audioSource;
    private SpiderAttack spider;
    public AudioClip bossFightMusic;
    public Transform var;
    private int currentTrackIndex = 0;
    private bool isTransitioning = false;
    private bool canTransition = true;
    private float transitionThreshold = 200f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spider = GameObject.FindGameObjectWithTag("Spider").GetComponent<SpiderAttack>();
        // Start playing the initial background audio track
        PlayAudioTrack(currentTrackIndex);
    }

    private void Update()
    {
        // Track player position and trigger audio transition if needed
        float playerXPosition = transform.position.x;
        float distanceToObject = Vector3.Distance(transform.position, var.transform.position);

        if (distanceToObject <= 40f)
        {
            PlayBossMusic();
        }
        else if (playerXPosition > transitionThreshold && canTransition && !isTransitioning && spider.inRange == false)
        {
            canTransition = false;
            isTransitioning = true;
            StartCoroutine(TransitionToNextTrack());
        }
        else if (playerXPosition <= transitionThreshold && !canTransition && spider.inRange == false)
        {
            canTransition = true;
            StartCoroutine(TransitionToPreviousTrack());
        }
    }

    private IEnumerator TransitionToNextTrack()
    {
        // Smoothly fade out the current track
        float fadeDuration = 2.0f; // Adjust the duration as needed
        float startVolume = 0.075f;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        // Play the next background audio track
        currentTrackIndex = (currentTrackIndex + 1) % backgroundAudioTracks.Length;
        PlayAudioTrack(currentTrackIndex);

        // Smoothly fade in the new track
        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        // Allow further transitions after the transition is complete
        isTransitioning = false;
    }

    private IEnumerator TransitionToPreviousTrack()
    {
        // Smoothly fade out the current track
        float fadeDuration = 2.0f; // Adjust the duration as needed
        float startVolume = 0.0375f;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        // Play the previous background audio track
        currentTrackIndex = (currentTrackIndex - 1 + backgroundAudioTracks.Length) % backgroundAudioTracks.Length;
        PlayAudioTrack(currentTrackIndex);

        // Smoothly fade in the new track
        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        // Allow further transitions after the transition is complete
        isTransitioning = false;
    }

    private void PlayAudioTrack(int index)
    {
        audioSource.Stop(); // Stop the current audio before changing the clip
        audioSource.clip = backgroundAudioTracks[index];
        audioSource.Play();
    }

    private void PlayBossMusic()
    {
        // Check if the boss music is not already playing
        if (!audioSource.isPlaying || audioSource.clip != bossFightMusic)
        {
            // Stop the current audio and play the boss fight music
            audioSource.Stop();
            audioSource.clip = bossFightMusic;
            audioSource.Play();
        }
    }
}