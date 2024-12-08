using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] musicTracks;
    private AudioSource audioSource;
    private int currentTrackIndex = -1;

    public float fadeDuration = 1.0f;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        PlayRandomTrack();
        LoadMusicVolume();
    }

    public void PlayRandomTrack()
    {
        if (musicTracks.Length == 0)
            return;

        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, musicTracks.Length);
        } while (randomIndex == currentTrackIndex);

        currentTrackIndex = randomIndex;
        StartCoroutine(FadeOutAndIn(musicTracks[randomIndex]));
    }

    IEnumerator FadeOutAndIn(AudioClip newClip)
    {
        yield return StartCoroutine(FadeOut());
        audioSource.clip = newClip;
        audioSource.Play();
        yield return StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    IEnumerator FadeIn()
    {
        audioSource.volume = 0.05f;
        float targetVolume = PlayerPrefs.GetFloat("MusicVolume", 0.25f);

        while (audioSource.volume < targetVolume)
        {
            audioSource.volume += targetVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }

    private void LoadMusicVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.25f);
        audioSource.volume = musicVolume;
    }
}


