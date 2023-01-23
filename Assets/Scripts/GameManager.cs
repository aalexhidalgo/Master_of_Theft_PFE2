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
    private bool tutorial_Close;
    private Animator tutorialAnim;

    //Money
    private int money = 00000;
    public TextMeshProUGUI moneyText;

    #endregion

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        tutorialAnim = tutorialBox.GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    //Animations
    private void LateUpdate()
    {
        tutorialAnim.SetBool("Tutorial_Close", tutorial_Close);
        //tutorialAnim.SetBool("Tutorial_Enter", tutorial_Close);
    }

    //Tutorial   
    public void DisplayText(int tutorialStringSelected)
    {
        tutorialBox.SetActive(true);
        tutorialText.text = tutorialString[tutorialStringSelected];
    }



    public void CloseText() //PASAR A CORRUTINA
    {
        tutorial_Close = true;
        //yield return new WaitForSeconds (1f);
        //tutorialBox.SetActive(false);
    }

    public void ChangeToGame()
    {
        SceneManager.LoadScene("Game");
    }

}
