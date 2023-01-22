using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Canvas
    //Tutorial
    public bool isInTutorial = true;
    public GameObject tutorialBox;
    public TextMeshProUGUI tutorialText;
    public string[] tutorialString;

    //Money
    private int money = 00000;
    public TextMeshProUGUI moneyText;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Tutorial   
    public void DisplayText(int tutorialStringSelected)
    {
        tutorialBox.SetActive(true);
        tutorialText.text = tutorialString[tutorialStringSelected];
    }

    public void CloseText()
    {
        tutorialBox.SetActive(false);
    }

    public void ChangeToGame()
    {
        SceneManager.LoadScene("Game");
    }

}
