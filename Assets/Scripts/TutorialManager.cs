using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;
    [TextArea]
    public string[] tutorialString;

    public int value = 0;

    private bool tutorial_Exit = false;
    private bool tutorial_Enter = true;
    private Animator tutorialAnim;

    private CinemachineVirtualCamera cvCamera;

    //Scripts
    private GameManager GameManagerScript;
    private PlayerController PlayerControllerScript;

    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        PlayerControllerScript = FindObjectOfType<PlayerController>();

        cvCamera = FindObjectOfType<CinemachineVirtualCamera>();

        tutorialAnim = tutorialPanel.GetComponent<Animator>();
        DisplayText();
    }

    void Update()
    {
        if(PlayerControllerScript.move == true)
        {
            StartCoroutine(CloseText());
        }
    }

    void LateUpdate()
    {
        tutorialAnim.SetBool("Tutorial_Exit", tutorial_Exit);
        tutorialAnim.SetBool("Tutorial_Enter", tutorial_Enter);
    }

    public void DisplayText()
    {
        //cvCamera.enabled = false;
        tutorial_Enter = true;
        tutorialPanel.SetActive(true);
        tutorialText.text = tutorialString[value];
    }

    public IEnumerator CloseText()
    {
        //cvCamera.enabled = true;
        PlayerControllerScript.move = false;
        tutorial_Enter = false;
        tutorial_Exit = true;
        yield return new WaitForSeconds(1.5f);
        tutorial_Exit = false;
        tutorialPanel.SetActive(false);
    }

    public void ChangeToGame()
    {
        //StartCoroutine(DisplayText(7, 5));
        DataPersistence.PlayerStats.isInTutorial = 0;
        DataPersistence.PlayerStats.skipTutorial = 0;
        DataPersistence.PlayerStats.SaveForFutureGames();
        SceneManager.LoadScene(2);
    }
}
