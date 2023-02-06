using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    #region Buttons
    public AudioMixer myMixer;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup SFXMixer;
    public Slider SFXSlider;
    public Slider musicSlider;

    public Toggle musicToggle;      
    public Toggle SFXToggle;

    private AudioSource myCamAudioSource;
    private AudioSource menuManagerAudioSource;

    #endregion

    public void StartButton()
    {
        DataPersistence.PlayerStats.SaveForFutureGames();
        SceneManager.LoadScene(DataPersistence.PlayerStats.isInTutorial);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    void Start()
    {
        menuManagerAudioSource = GetComponent<AudioSource>();
        myCamAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        LoadData();
    }

    public void Music_Volume(float volume)
    {
        myMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        DataPersistence.PlayerStats.musicVolume = musicSlider.value;
        DataPersistence.PlayerStats.SaveForFutureGames();
    }

    public void SFX_Volume(float volume)
    {
        myMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        DataPersistence.PlayerStats.SFXVolume = SFXSlider.value;
        DataPersistence.PlayerStats.SaveForFutureGames();
    }

    public int BoolToIntMusic(bool active)
    {
        return active ? 1 : 0;
    }

    public int BoolToIntSFX(bool active)
    {
        return active ? 1 : 0;
    }

    public bool IntToBoolMusic(int i)
    {
        return (i == 0 ? false : true);
    }
    
    public bool IntToBoolSFX(int i)
    {
        return (i == 0 ? false : true);
    }

    public void Music_Active()
    {
        DataPersistence.PlayerStats.musicActive = BoolToIntMusic(musicToggle.GetComponent<Toggle>().isOn);
        DataPersistence.PlayerStats.SaveForFutureGames();

        if (musicToggle.isOn == true)
        {
            myCamAudioSource.Play();
        }

        if (musicToggle.isOn == false)
        {
            myCamAudioSource.Pause();
        }
    }

    public void SFX_Active()
    {
        DataPersistence.PlayerStats.SFXActive = BoolToIntSFX(SFXToggle.GetComponent<Toggle>().isOn);
        DataPersistence.PlayerStats.SaveForFutureGames();

        if (SFXToggle.isOn == true)
        {
            menuManagerAudioSource.Play();
        }

        if (SFXToggle.isOn == false)
        {
            menuManagerAudioSource.Pause();
        }
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey("Is_In_Tutorial"))
        {
            DataPersistence.PlayerStats.isInTutorial = PlayerPrefs.GetInt("Is_In_Tutorial");

            DataPersistence.PlayerStats.musicVolume = PlayerPrefs.GetFloat("Music_Volume");
            DataPersistence.PlayerStats.SFXVolume = PlayerPrefs.GetFloat("SFX_Volume");

            musicSlider.value = PlayerPrefs.GetFloat("Music_Volume");
            SFXSlider.value = PlayerPrefs.GetFloat("SFX_Volume");

            Debug.Log($"music: {PlayerPrefs.GetInt("Music_Active")}");
            DataPersistence.PlayerStats.musicActive = PlayerPrefs.GetInt("Music_Active");
            DataPersistence.PlayerStats.SFXActive = PlayerPrefs.GetInt("SFX_Active");

            musicToggle.isOn = IntToBoolMusic(PlayerPrefs.GetInt("Music_Active"));
            SFXToggle.isOn = IntToBoolMusic(PlayerPrefs.GetInt("SFX_Active"));
        }
    }
}
