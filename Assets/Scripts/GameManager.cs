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

    private bool tutorial_Exit = false;
    private bool tutorial_Start = false;
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
        if(tutorialBox.activeInHierarchy == true && isInTutorial == true)
        {
            tutorialAnim.SetBool("Tutorial_Exit", tutorial_Exit);
            tutorialAnim.SetBool("Tutorial_Start", tutorial_Start);
        }
    }

    //Tutorial   
    public IEnumerator DisplayText(int tutorialStringSelected)
    {
        tutorialBox.SetActive(true);       
        tutorialText.text = tutorialString[tutorialStringSelected];
        tutorial_Start = true;
        yield return new WaitForSeconds(1f);
        tutorial_Start = false;
    }

    public IEnumerator CloseText()
    {
        tutorial_Exit = true;
        yield return new WaitForSeconds (1f);
        tutorial_Exit = false;
        tutorialBox.SetActive(false);
    }

    public void ChangeToGame()
    {
        SceneManager.LoadScene("Game");
    }

}
