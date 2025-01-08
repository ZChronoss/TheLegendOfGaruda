using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundMixerManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Sliders")]
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    private void Start()
    {
        if(!PlayerPrefs.HasKey(AudioStrings.master))
        {
            PlayerPrefs.SetFloat(AudioStrings.master, 1f);
        }

        if (!PlayerPrefs.HasKey(AudioStrings.sfx))
        {
            PlayerPrefs.SetFloat(AudioStrings.sfx, 1f);
        }

        if (!PlayerPrefs.HasKey(AudioStrings.music))
        {
            PlayerPrefs.SetFloat(AudioStrings.music, 1f);
        }

        LoadVolumePrefs();
    }

    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat(AudioStrings.master, Mathf.Log10(level) * 20f);
        SaveVolumePrefs(AudioStrings.master, level);
    }

    public void SetSFXVolume(float level)
    {
        audioMixer.SetFloat(AudioStrings.sfx, Mathf.Log10(level) * 20f);
        SaveVolumePrefs(AudioStrings.sfx, level);
    }

    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat(AudioStrings.music, Mathf.Log10(level) * 20f);
        SaveVolumePrefs(AudioStrings.music, level);
    }

    private void SaveVolumePrefs(string name, float level)
    {
        PlayerPrefs.SetFloat(name, level);
    }

    private void LoadVolumePrefs()
    {
        var masterVolume = PlayerPrefs.GetFloat(AudioStrings.master);
        var sfxVolume = PlayerPrefs.GetFloat(AudioStrings.sfx);
        var musicVolume = PlayerPrefs.GetFloat(AudioStrings.music);

        SetMasterVolume(masterVolume);
        SetSFXVolume(sfxVolume);
        SetMusicVolume(musicVolume);

        masterSlider.value = masterVolume;
        sfxSlider.value = sfxVolume;
        musicSlider.value = musicVolume;
    }
}
