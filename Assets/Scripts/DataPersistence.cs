using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistence : MonoBehaviour
{
    //Shared instance
    public static DataPersistence PlayerStats;

    #region Variables
    public int isInTutorial = 1;

    //Music
    public float SFXVolume = 1;
    public float musicVolume = 1;
    public int SFXActive = 0;
    public int musicActive = 0;

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

        PlayerPrefs.SetFloat("SFX_Volume", SFXVolume);
        PlayerPrefs.SetFloat("Music_Volume", musicVolume);

        Debug.Log($"PlayerPrefs\n Music: {DataPersistence.PlayerStats.musicActive} SFX: {DataPersistence.PlayerStats.SFXActive}");
        PlayerPrefs.SetInt("SFX_Active", SFXActive);
        PlayerPrefs.SetInt("Music_Active", musicActive);       
    }
}
