using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistence : MonoBehaviour
{
    //Shared instance
    public static DataPersistence PlayerStats;

    #region Variables
    public int isInTutorial = 1;
    public int skipTutorial = 0;

    //Music
    public float SFXVolume = 1;
    public float musicVolume = 1;
    public int SFXActive = 1;
    public int musicActive = 1;

    //Graphics
    public int graphics = 2;
    public int fullScreen = 0;
    public int resolution;
    public float brightness = 0.5f;

    //Other
    public int hasRestarted = 0;
    public int score;
    public int highScore = 0;

    #endregion

    void Awake()
    {
        //If the instance doesn't exist
        if (PlayerStats == null)
        {
            //We set the instance
            PlayerStats = this;
            //We make sure to not destroy it after the scene change
            DontDestroyOnLoad(PlayerStats);
        }
        else
        {
            //Because a instance already exists, we deastroy the cloned one
            Destroy(this);
        }
    }

    public void SaveForFutureGames()
    {
        PlayerPrefs.SetInt("Is_In_Tutorial", isInTutorial);
        PlayerPrefs.SetInt("Skip_Tutorial", skipTutorial);

        PlayerPrefs.SetFloat("SFX_Volume", SFXVolume);
        PlayerPrefs.SetFloat("Music_Volume", musicVolume);

        PlayerPrefs.SetInt("SFX_Active", SFXActive);
        PlayerPrefs.SetInt("Music_Active", musicActive);

        PlayerPrefs.SetInt("Graphics", graphics);
        PlayerPrefs.SetInt("FullScreen", fullScreen);
        PlayerPrefs.SetInt("Resolution", resolution);
        PlayerPrefs.SetFloat("Brightness", brightness);
    }

    public void SaveInGame()
    {
        PlayerPrefs.SetInt("Has_Restarted", hasRestarted);
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("High_Score", highScore);
    }

    void OnApplicationQuit()
    {
        SaveForFutureGames();
    }
}
