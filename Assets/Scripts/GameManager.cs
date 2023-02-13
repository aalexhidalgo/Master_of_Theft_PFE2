using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public GameObject WinPanel, GameOverPanel, PausePanel;

    #region UI 
    //Tutorial
    public bool isInTutorial;
    public GameObject tutorialBox;
    public TextMeshProUGUI tutorialText;
    public string[] tutorialString;

    private bool tutorial_Exit = false;
    private bool tutorial_Start = false;
    private Animator tutorialAnim;

    //Audio
    public AudioMixer myMixer;
    public Slider musicSlider;
    public Slider SFXSlider;

    public Toggle musicToggle;
    public Toggle SFXToggle;

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

    //WIN & GAMEOVER
    public bool gameOver = false;
    private CinemachineVirtualCamera cvCamera;

    //Scripts
    private PlayerController PlayerControllerScript;

    private AudioSource myCamAudioSource;
    private AudioSource gameManagerAudioSource;

    void Start()
    {
        gameManagerAudioSource = GetComponent<AudioSource>();
        myCamAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();

        LoadData(); //Data Persistence & PlayerPrefs data

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        tutorialAnim = tutorialBox.GetComponent<Animator>();

        PlayerControllerScript = FindObjectOfType<PlayerController>();
        cvCamera = FindObjectOfType<CinemachineVirtualCamera>();        
    }

    void Update()
    {
        TimeCounter(0);

        if(Input.GetKeyDown(KeyCode.T))
        {
            PauseButton();
        }
    }

    void PauseButton()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PausePanel.SetActive(true);
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

    public void GameOver()
    {
        gameOver = true;
        cvCamera.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameOverPanel.SetActive(true);
    }

    #region Tutorial   
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
        DataPersistence.PlayerStats.isInTutorial = 0;
        DataPersistence.PlayerStats.skipTutorial = 0;
        DataPersistence.PlayerStats.SaveForFutureGames();
        SceneManager.LoadScene(2);
    }
    #endregion

    #region UI
    //Money
    public void AddMoney(int value)
    {
        money += value;
        moneyText.text = money.ToString();
    }

    //Time
    public void TimeCounter(float time)
    {
        if(gameOver == false)
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
    }

    //Key
    public void Key_Color(Color colorType)
    {
        Key_Collected++;

        if (Key_Collected >= 0) //Shows the player in the UI the key collected and the color attached to the key
        {
            Key_GameObject[Key_Collected].SetActive(true);
            Image Key_Image = Key_GameObject[Key_Collected].GetComponent<Image>();
            Key_Colors = colorType;
            Key_Image.color = Key_Colors;
        }
    }
    #endregion

    #region Options

    public void Music_Volume(float volume)
    {
        myMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        DataPersistence.PlayerStats.musicVolume = musicSlider.value;
    }

    public void SFX_Volume(float volume)
    {
        myMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        DataPersistence.PlayerStats.SFXVolume = SFXSlider.value;
    }

    public int BoolToInt(bool value)
    {
        return value ? 1 : 0;
    }
    public bool IntToBool(int value)
    {
        return !(value == 0);
    }

    public void Music_Active(bool isActive)
    {
        DataPersistence.PlayerStats.musicActive = BoolToInt(isActive);

        if (isActive == true)
        {
            myCamAudioSource.Play();
        }

        if (isActive == false)
        {
            myCamAudioSource.Pause();
        }
    }

    public void SFX_Active(bool isActive)
    {
        DataPersistence.PlayerStats.SFXActive = BoolToInt(isActive);

        if (isActive == true)
        {
            gameManagerAudioSource.Play();
        }

        if (isActive == false)
        {
            gameManagerAudioSource.Pause();
        }
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(DataPersistence.PlayerStats.isInTutorial);
    }
    #endregion

    public void LoadData()
    {
        musicSlider.value = PlayerPrefs.GetFloat("Music_Volume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFX_Volume");

        float MusicVolume = PlayerPrefs.GetFloat("Music_Volume");
        float sfxVolume = PlayerPrefs.GetFloat("SFX_Volume");

        myMixer.SetFloat("MusicVolume", Mathf.Log10(MusicVolume) * 20);
        myMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);

        if (DataPersistence.PlayerStats.isInTutorial == 0)
        {
            isInTutorial = false;
        }
        if (DataPersistence.PlayerStats.isInTutorial == 1)
        {
            isInTutorial = true;
        }

        DataPersistence.PlayerStats.musicActive = PlayerPrefs.GetInt("Music_Active");
        DataPersistence.PlayerStats.SFXActive = PlayerPrefs.GetInt("SFX_Active");

        musicToggle.isOn = IntToBool(PlayerPrefs.GetInt("Music_Active"));
        SFXToggle.isOn = IntToBool(PlayerPrefs.GetInt("SFX_Active"));
    }
}
