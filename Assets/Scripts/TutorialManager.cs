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
    public GameObject welcomeText;
    public GameObject actionText;
    public TextMeshProUGUI tutorialText;
    [TextArea]
    public string[] tutorialString;

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
        StartCoroutine(DisplayText(0, 0));
    }

    void LateUpdate()
    {
        tutorialAnim.SetBool("Tutorial_Exit", tutorial_Exit);
        tutorialAnim.SetBool("Tutorial_Enter", tutorial_Enter);
    }

    public IEnumerator DisplayText(int tutorialStringSelected, float time)
    {
        //cvCamera.enabled = false;
        yield return new WaitForSeconds(time);
        tutorial_Enter = true;
        tutorialPanel.SetActive(true);
        tutorialText.text = tutorialString[tutorialStringSelected];
    }

    public IEnumerator CloseText()
    {
        //cvCamera.enabled = true;
        tutorial_Enter = false;
        tutorial_Exit = true;
        yield return new WaitForSeconds(1.5f);
        tutorial_Exit = false;
        welcomeText.SetActive(false);
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
