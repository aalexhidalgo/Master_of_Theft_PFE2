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
    public float soundValue;
    public float musicValue;
    public int soundActive;
    public int musicActive;


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

        PlayerPrefs.SetFloat("Sound_Value", soundValue);
        PlayerPrefs.SetFloat("Music_Value", soundValue);

        PlayerPrefs.SetInt("Sound_Active", soundActive);
        PlayerPrefs.SetInt("Music_Active", musicActive);       
    }
}
