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

    public TMP_Dropdown graphicsDropdown;
    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;
    string defaultResolution = "1920 x 1080";
    private List<TMP_Dropdown.OptionData> resolutionOptions;

    public Slider brightnessSlider;

    public Toggle fullScreenToggle;

    public Toggle skipTutorialToggle;

    private AudioSource myCamAudioSource;
    private AudioSource menuManagerAudioSource;

    #endregion

    public void StartButton()
    {
        if(DataPersistence.PlayerStats.skipTutorial == 0)
        {
            DataPersistence.PlayerStats.isInTutorial = 1;
            SceneManager.LoadScene(1); //LoadTutorial =            
        }
        if (DataPersistence.PlayerStats.skipTutorial == 1)
        {
            DataPersistence.PlayerStats.isInTutorial = 0;
            SceneManager.LoadScene(2); //
        }


        DataPersistence.PlayerStats.SaveForFutureGames();
    }

    public void ReturnButton()
    {
        DataPersistence.PlayerStats.SaveForFutureGames();
    }
    public void ExitButton()
    {
        Application.Quit();
    }

    public void SkipTutorial(bool isActive)
    {
        DataPersistence.PlayerStats.skipTutorial = BoolToInt(isActive);
    }

    void Start()
    {       
        menuManagerAudioSource = GetComponent<AudioSource>();
        myCamAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();

        resolutionOptions = resolutionDropdown.options; //resolutions options of the dropdown
        
        Resolution();
        LoadData();        
    }

    #region Music System
    public void Music_Volume(float volume)
    {
        myMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        DataPersistence.PlayerStats.musicVolume = musicSlider.value;       
    }

    public void SFX_Volume(float volume)
    {
        myMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        DataPersistence.PlayerStats.SFXVolume = SFXSlider.value;       
    }

    public int BoolToInt(bool value)
    {
        return value ? 1 : 0;
    }
    public bool IntToBool(int value)
    {
        return !(value == 0);
    }

    public void Music_Active(bool isActive)
    {
        DataPersistence.PlayerStats.musicActive = BoolToInt(isActive);      

        if (isActive == true)
        {
            myCamAudioSource.Play();
        }

        if (isActive == false)
        {
            myCamAudioSource.Pause();
        }      
    }

    public void SFX_Active(bool isActive)
    {
        DataPersistence.PlayerStats.SFXActive = BoolToInt(isActive);      

        if (isActive == true)
        {
            menuManagerAudioSource.Play();
        }

        if (isActive == false)
        {
            menuManagerAudioSource.Pause();
        }      
    }
    #endregion

    #region Video Graphics

    public void Graphics(int qualityType)
    {
        QualitySettings.SetQualityLevel(qualityType);
        DataPersistence.PlayerStats.graphics = qualityType;       
    }

    public void FullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        DataPersistence.PlayerStats.fullScreen = BoolToInt(isFullScreen);
    }

    public void Resolution()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions(); //Delete previous resolutions
        List<string> options = new List<string>();
        int currentResolutionIndx = 0;

        //Resolutions availables for your pc
        for (int i = 0; i < resolutions.Length; i ++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height; //String displayed on the dropdown
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndx = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndx;
        resolutionDropdown.RefreshShownValue();
    }

    public void UpdateResolution(int resolutionIndx)
    {
        Resolution resolution = resolutions[resolutionIndx];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        DataPersistence.PlayerStats.resolution = resolutionIndx;
    }

    public void Brightness(float value)
    {
        Screen.brightness = value;
        DataPersistence.PlayerStats.brightness = brightnessSlider.value;
    }
    #endregion

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

            DataPersistence.PlayerStats.graphics = PlayerPrefs.GetInt("Graphics");
            QualitySettings.SetQualityLevel(DataPersistence.PlayerStats.graphics);
            graphicsDropdown.value = DataPersistence.PlayerStats.graphics;

            DataPersistence.PlayerStats.fullScreen = PlayerPrefs.GetInt("FullScreen");
            fullScreenToggle.isOn = IntToBool(PlayerPrefs.GetInt("FullScreen"));

            resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");

            DataPersistence.PlayerStats.brightness = PlayerPrefs.GetFloat("Brightness");
            brightnessSlider.value = PlayerPrefs.GetFloat("Brightness");

            DataPersistence.PlayerStats.skipTutorial = PlayerPrefs.GetInt("Skip_Tutorial");
            skipTutorialToggle.isOn = IntToBool(PlayerPrefs.GetInt("Skip_Tutorial"));

            DataPersistence.PlayerStats.highScore = PlayerPrefs.GetInt("High_Score");
        }

        resolutionOptions = resolutionOptions.FindAll(option => option.text.IndexOf(defaultResolution) >= 0); //1920 x 1080 it will be the default setting of the screen
    }
   
}
