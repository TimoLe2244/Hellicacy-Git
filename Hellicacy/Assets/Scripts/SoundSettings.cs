using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
   [SerializeField] AudioMixer audioMixer;
   [SerializeField] Slider masterVolumeSlider;
   [SerializeField] Slider sfxVolumeSlider;
   [SerializeField] Slider musicVolumeSlider;

   string masterVolume = "MasterVolume";
   string sfxVolume = "SFXVolume";
   string musicVolume = "MusicVolume";

   void Start(){
      masterVolumeSlider.value = PlayerPrefs.GetFloat(masterVolume, .25f);
      sfxVolumeSlider.value = PlayerPrefs.GetFloat(sfxVolume, .25f);
      musicVolumeSlider.value = PlayerPrefs.GetFloat(musicVolume, .25f);
      SetMasterVolume();
      SetSFXVolume();
      SetMusicVolume();
   }

   public void SetMasterVolume()
   {
    SetVolume("MasterVolume", masterVolumeSlider.value);
    PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
   }

   public void SetSFXVolume()
   {
    SetVolume("SFXVolume", sfxVolumeSlider.value);
    PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
   }

   public void SetMusicVolume()
   {
    SetVolume("MusicVolume", musicVolumeSlider.value);
    PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
   }

   void SetVolume(string groupName, float value)
   {
      float adjustedVolume = Mathf.Log10(value) * 20;
      if(value == 0)
      {
         adjustedVolume = -80;
      }
      audioMixer.SetFloat(groupName, adjustedVolume);
   }
}
