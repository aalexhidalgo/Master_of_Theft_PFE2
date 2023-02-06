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
    public Slider SFXSlider;
    public Slider musicSlider;

    public Toggle musicToggle;      
    public Toggle SFXToggle;

    //private AudioSource myCamAudioSource;
    //private AudioSource gameManagerAudioSource;

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
        LoadData();

        //gameManagerAudioSource = GetComponent<AudioSource>();
        //myCamAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
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

    public int IntToBool(bool active)
    {
        return active ? 0 : 1;
    }

    public bool IntToBool(int i)
    {
        return !(i == 0);
    }

    public void Music_SFX_Active()
    {
        DataPersistence.PlayerStats.musicActive = IntToBool(musicToggle.GetComponent<Toggle>().isOn);
        DataPersistence.PlayerStats.SFXActive = IntToBool(SFXToggle.GetComponent<Toggle>().isOn);
        DataPersistence.PlayerStats.SaveForFutureGames();
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

            DataPersistence.PlayerStats.musicActive = PlayerPrefs.GetInt("Music_Active");
            DataPersistence.PlayerStats.SFXActive = PlayerPrefs.GetInt("SFX_Active");

            musicToggle.isOn = IntToBool(PlayerPrefs.GetInt("Music_Active"));
            SFXToggle.isOn = IntToBool(PlayerPrefs.GetInt("SFX_Active"));
        }
    }
}
