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

    //Time
    private float timeCounter = 1200;
    public TextMeshProUGUI timeText;

    //Keys
    public int Key_Collected = -1;
    public GameObject[] Key_GameObject;
    private Color Key_Colors;
    public List<string> Keys_Strings = new List<string>();

    #endregion

    //Scripts
    private PlayerController PlayerControllerScript;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        tutorialAnim = tutorialBox.GetComponent<Animator>();

        PlayerControllerScript = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        TimeCounter(0);

        if(Key_Collected >= 0) //Shows the player in the UI the key collected and the color attached to the key
        {
            Key_GameObject[Key_Collected].SetActive(true);
            Image Key_Image = Key_GameObject[Key_Collected].GetComponent<Image>();
            Key_Image.color = Key_Colors;
        }
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

    //Money
    public void AddMoney(int value)
    {
        money += value;
        moneyText.text = money.ToString();
    }

    //Time
    public void TimeCounter(float time)
    {
        timeCounter += time;
        float minutes = Mathf.FloorToInt(timeCounter / 60);
        float seconds = Mathf.FloorToInt(timeCounter % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (PlayerControllerScript.hasMoved == true) //Starts when the Player has already moved
        {
            timeCounter -= Time.deltaTime;
        }
    }

    //Key
    public void Key_Color(Color colorType)
    {
       Key_Colors = colorType;            
    }
}
