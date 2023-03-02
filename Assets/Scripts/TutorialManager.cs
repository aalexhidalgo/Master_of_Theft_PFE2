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
    public string[] tutorialString;

    private bool tutorial_Exit = false;
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
        DisplayText(0);
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        tutorialAnim.SetBool("Tutorial_Exit", tutorial_Exit);
    }

    public void DisplayText(int tutorialStringSelected)
    {
        //cvCamera.enabled = false;
        tutorialPanel.SetActive(true);
        tutorialText.text = tutorialString[tutorialStringSelected];
    }

    public IEnumerator CloseText()
    {
        //cvCamera.enabled = true;
        tutorial_Exit = true;
        yield return new WaitForSeconds(1.25f);
        tutorial_Exit = false;
        tutorialPanel.SetActive(false);
    }

    public void ChangeToGame()
    {
        DataPersistence.PlayerStats.isInTutorial = 0;
        DataPersistence.PlayerStats.skipTutorial = 0;
        DataPersistence.PlayerStats.SaveForFutureGames();
        SceneManager.LoadScene(2);
    }
}
