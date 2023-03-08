using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public GameObject welcomeText;
    public TextMeshProUGUI actionText;
    public TextMeshProUGUI tutorialText;
    [TextArea]
    public string[] tutorialString;
    public string[] actionString;

    private bool tutorial_Exit = false;
    private bool tutorial_Enter = true;
    private Animator tutorialAnim;

    //Scripts
    private GameManager GameManagerScript;
    private PlayerController PlayerControllerScript;

    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        PlayerControllerScript = FindObjectOfType<PlayerController>();

        tutorialAnim = tutorialPanel.GetComponent<Animator>();

        if(GameManagerScript.isInTutorial == true)
        {
            StartCoroutine(DisplayText(0, 0)); //First message
        }
    }

    void LateUpdate()
    {
        if(GameManagerScript.isInTutorial == true)
        {
            tutorialAnim.SetBool("Tutorial_Exit", tutorial_Exit);
            tutorialAnim.SetBool("Tutorial_Enter", tutorial_Enter);
        }
    }

    public IEnumerator DisplayText(int tutorialStringSelected, float time) //To display the message
    {
        yield return new WaitForSeconds(time); //Time between and action and the next message
        tutorial_Enter = true;
        tutorialPanel.SetActive(true);
        tutorialText.text = tutorialString[tutorialStringSelected]; //The explanation
        actionText.text = actionString[tutorialStringSelected]; //The step you have to do;
    }

    public IEnumerator CloseText() //To close the message: When the action has been executed by the player
    {
        tutorial_Enter = false;
        tutorial_Exit = true;
        yield return new WaitForSeconds(1.5f);
        tutorial_Exit = false;
        welcomeText.SetActive(false);
        tutorialPanel.SetActive(false);
    }

    public IEnumerator ChangeToGame() //End of tutorial
    {
        StartCoroutine(DisplayText(8, 2));
        yield return new WaitForSeconds(7f);
        DataPersistence.PlayerStats.isInTutorial = 0;
        DataPersistence.PlayerStats.skipTutorial = 0;
        DataPersistence.PlayerStats.hasBeenInTutorial = 1;
        DataPersistence.PlayerStats.SaveForGameScene();
        DataPersistence.PlayerStats.SaveForFutureGames();
        SceneManager.LoadScene(2);
    }
}
