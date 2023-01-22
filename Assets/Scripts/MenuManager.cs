using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    #region Buttons

    #endregion

    public void StartButton()
    {
        DataPersistence.PlayerStats.SaveForFutureGames();
        SceneManager.LoadScene("Tutorial");
    }

    public void ExitButton()
    {
        
    }

    public void ReturnButton()
    {
        DataPersistence.PlayerStats.SaveForFutureGames();
    }

    void Start()
    {
        Load_Data();
    }

    public void Load_Data() //To load the data saved in PlayerPrefs
    {
        if (PlayerPrefs.HasKey("Is_In_Tutorial"))
        {
            DataPersistence.PlayerStats.isInTutorial = PlayerPrefs.GetInt("Is_In_Tutorial");
        }
    }
}
