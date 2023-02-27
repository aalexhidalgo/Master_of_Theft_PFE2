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

    public Image pauseButton;
    public Sprite[] pauseSprites;

    //Money
    private int money = 00000;
    public TextMeshProUGUI moneyText;
    private bool add_Money = false;
    private Animator moneyAnim;

    //Time
    private float timeCounter = 1200;
    public TextMeshProUGUI timeText;

    //Keys
    public int Key_Collected = -1;
    public GameObject[] Key_GameObject;
    private Color Key_Colors;
    public List<string> Keys_Strings = new List<string>();
    #endregion

    //Animations
    private Animator GameOverAnim;

    //WIN & GAMEOVER
    public bool gameOver = false;
    public bool pause = false;
    private CinemachineVirtualCamera cvCamera;

    //Scripts
    private PlayerController PlayerControllerScript;

    private AudioSource myCamAudioSource;
    private AudioSource gameManagerAudioSource;
    private AudioSource guardAudioSource;

    void Start()
    {
        gameManagerAudioSource = GetComponent<AudioSource>();
        myCamAudioSource = GameObject.Find("Main Camera").GetComponent<AudioSource>();
        guardAudioSource = GameObject.Find("Guard").GetComponent<AudioSource>();

        LoadData(); //Data Persistence & PlayerPrefs data

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        tutorialAnim = tutorialBox.GetComponent<Animator>();
        moneyAnim = moneyText.GetComponent<Animator>();

        PlayerControllerScript = FindObjectOfType<PlayerController>();
        cvCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    void Update()
    {
        TimeCounter(0);

        if(Input.GetKeyDown(KeyCode.Escape) && gameOver == false)
        {
            PauseButton();
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

        moneyAnim.SetBool("Add_Money", add_Money);
    }

    public IEnumerator GameOver()
    {
        gameOver = true;
        cvCamera.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;   
        GameOverPanel.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        GameOverAnim = GameOverPanel.GetComponent<Animator>();
        GameOverAnim.enabled = true;
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
    public IEnumerator AddMoney(int value)
    {
        money += value;
        moneyText.text = money.ToString();
        add_Money = true;
        yield return new WaitForSeconds (0.725f);
        add_Money = false;
    }

    //Time
    public void TimeCounter(float time)
    {
        if(gameOver == false && pause == false)
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
            guardAudioSource.Pause();
        }
    }
    void PauseButton()
    {
        if (pause == false)
        {
            cvCamera.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PausePanel.SetActive(true);
            pause = true;
            pauseButton.sprite = pauseSprites[1];
            gameManagerAudioSource.Pause(); //Detenemos los posibles efectos de sonido en marcha y dejamos solo la música de fondo
            guardAudioSource.Pause();
        }
        else
        {
            ReturnButton();
            gameManagerAudioSource.Play(); //Reanudamos los posibles efectos de sonido en marcha           
            guardAudioSource.UnPause();                                                           
        }
    }
    public void RestartButton()
    {
        SceneManager.LoadScene(DataPersistence.PlayerStats.isInTutorial); //Restart the current scene we are playing (tutorial or game)
    }

    public void MenuButton()
    {
        SceneManager.LoadScene(0); //Menu
    }

    public void ReturnButton()
    {
        cvCamera.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PausePanel.SetActive(false);
        pause = false;
        pauseButton.sprite = pauseSprites[0];
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
