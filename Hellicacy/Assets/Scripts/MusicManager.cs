using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] musicTracks; // List of all available music tracks
    private AudioSource audioSource;
    private int currentTrackIndex = -1;

    public float fadeDuration = 1.0f; // Duration for fade-in/out (in seconds)

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        PlayRandomTrack(); // Start playing a random track when the game begins
    }

    public void PlayRandomTrack()
    {
        if (musicTracks.Length == 0)
            return;

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, musicTracks.Length);
        } while (randomIndex == currentTrackIndex); // Avoid repeating the same track

        currentTrackIndex = randomIndex;
        StartCoroutine(FadeOutAndIn(musicTracks[randomIndex])); // Fade in the new track
    }

    IEnumerator FadeOutAndIn(AudioClip newClip)
    {
        yield return StartCoroutine(FadeOut()); // Fade out the current music
        audioSource.clip = newClip; // Set the new track
        audioSource.Play(); // Start playing the new track
        yield return StartCoroutine(FadeIn()); // Fade it in
    }

    IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;

        // Fade out the current music
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop(); // Stop the current track once it's faded out
        audioSource.volume = startVolume; // Restore volume for fade-in
    }

    IEnumerator FadeIn()
    {
        audioSource.volume = 0.05f;
        float targetVolume = 0.25f;

        // Fade in the new track
        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
}


