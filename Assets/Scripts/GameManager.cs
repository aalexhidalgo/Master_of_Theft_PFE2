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
    public GameObject WinPanel, GameOverPanel, PausePanel, PreGamePanel;

    #region UI 
    //Tutorial
    public bool isInTutorial;

    //Audio
    public AudioMixer myMixer;
    public Slider musicSlider;
    public Slider SFXSlider;

    public Toggle musicToggle;
    public Toggle SFXToggle;

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
    public GameObject Master_Key;
    #endregion

    //Animations
    private Animator GameOverAnim;
    private Animator PreGameAnim;
    private bool close = false;

    //WIN & GAMEOVER
    public bool gameOver = false;
    public bool pause = false;
    public bool win = false;
    private CinemachineVirtualCamera cvCamera;

    //WIN
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

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

        moneyAnim = moneyText.GetComponent<Animator>();
        GameOverAnim = GameOverPanel.GetComponent<Animator>();

        PlayerControllerScript = FindObjectOfType<PlayerController>();
        cvCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (DataPersistence.PlayerStats.hasRestarted >= 1 || DataPersistence.PlayerStats.hasBeenInTutorial == 1)
        {
            close = true; //The pregame panel will only appear the first time you start the game, causing the restart not re-run its appearance on the screen.
            PreGamePanel.SetActive(false);
        }

    }

    void Update()
    {
        TimeCounter(0);

        if(Input.GetKeyDown(KeyCode.Escape) && gameOver == false && win == false)
        {
            PauseButton();
        }

        if(close == false)
        {
            StartCoroutine(Pre_Game());
        }

        if (timeCounter <= 0 && gameOver == false)
        {
            timeCounter = 0;
            StartCoroutine(GameOver());
        }
    }

    //Animations
    private void LateUpdate()
    {
        moneyAnim.SetBool("Add_Money", add_Money);

        if(isInTutorial == false)
        {
            GameOverAnim.SetBool("GameOver_Panel", PlayerControllerScript.hasBeenAttacked);
        }
    }

    public IEnumerator GameOver() //Two type of animations
    {
        if(PlayerControllerScript.hasBeenAttacked == true) //The security guard has caught the player
        {
            gameOver = true;
            cvCamera.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            yield return new WaitForSeconds(0.0001f); //Needed
            GameOverAnim.enabled = true;
            gameManagerAudioSource.Pause(); //Detenemos los posibles efectos de sonido en marcha y dejamos solo la música de fondo
        }
        else //Time's up
        {
            gameOver = true;
            cvCamera.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameOverAnim.enabled = true;
            gameManagerAudioSource.Pause(); //Detenemos los posibles efectos de sonido en marcha y dejamos solo la música de fondo
        }
    }

    public void Win()
    {
        win = true;
        cvCamera.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        DataPersistence.PlayerStats.score = money;

        if (DataPersistence.PlayerStats.highScore == 0) //The first match, the money highscore will be the same as the first money score saved
        {
            DataPersistence.PlayerStats.highScore = DataPersistence.PlayerStats.score;
        }
        else
        {
            if (DataPersistence.PlayerStats.highScore < DataPersistence.PlayerStats.score) //If we already played the game, the highscore will be replaced when the money counter hit the highscore.
            {
                DataPersistence.PlayerStats.highScore = DataPersistence.PlayerStats.score;
            }
        }

        scoreText.text = DataPersistence.PlayerStats.score.ToString();
        highScoreText.text = DataPersistence.PlayerStats.highScore.ToString();

        DataPersistence.PlayerStats.SaveInGame();
        WinPanel.SetActive(true);

        gameManagerAudioSource.Pause(); //Detenemos los posibles efectos de sonido en marcha y dejamos solo la música de fondo
    }

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

    //Pre Game Panel
    private IEnumerator Pre_Game() //Before everything else, it shows the player the mission to accomplish during the game
    {
        if(isInTutorial == false && PlayerControllerScript.hasMoved == true)
        {
            PreGamePanel.SetActive(true);           
            PreGameAnim = PreGamePanel.GetComponent<Animator>();
            PreGameAnim.enabled = true;
            yield return new WaitForSeconds(2.1f);
            PreGamePanel.SetActive(false);
            close = true;
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
    void PauseButton()
    {
        if (pause == false)
        {
            cvCamera.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PausePanel.SetActive(true);
            pause = true;
            gameManagerAudioSource.Pause(); //Detenemos los posibles efectos de sonido en marcha y dejamos solo la música de fondo
        }
        else
        {
            ReturnButton();
            gameManagerAudioSource.Play(); //Reanudamos los posibles efectos de sonido en marcha           
        }
    }
    public void RestartButton(int value)
    {
        DataPersistence.PlayerStats.hasRestarted = 1;
        DataPersistence.PlayerStats.SaveInGame();
        SceneManager.LoadScene(value); //Restart the current scene we are playing (tutorial or game)
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

        DataPersistence.PlayerStats.highScore = PlayerPrefs.GetInt("High_Score");
        //timeText.text = string.Format("{0:00}:{1:00}:{2:000}", (PlayerPrefs.GetFloat("Minutes")), (PlayerPrefs.GetFloat("Seconds")), (PlayerPrefs.GetFloat("Miliseconds")));
    }
}
