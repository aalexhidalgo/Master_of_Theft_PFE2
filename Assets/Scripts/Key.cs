using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    public Color keyColor;
    public string keyColorType;
    public bool masterKey;

    //Audio
    public AudioClip keySFX;
    private AudioSource gameManagerAudioSource;

    //Particle System
    private GameObject Key_ParticleSystem;

    //Scripts
    private GameManager GameManagerScript;
    private PlayerController PlayerControllerScript;
    private TutorialManager TutorialManagerScript;
    private Object ObjectScript;

    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        PlayerControllerScript = FindObjectOfType<PlayerController>();
        TutorialManagerScript = FindObjectOfType<TutorialManager>();
        ObjectScript = FindObjectOfType<Object>();

        gameManagerAudioSource = GameManagerScript.GetComponent<AudioSource>();

        Key_ParticleSystem = GameObject.Find($"Key_ParticleSystem ({keyColorType})"); //To find the exact particles of each key
    }

    private void OnTriggerStay(Collider otherTrigger)
    {
        if(GameManagerScript.isInTutorial == false)
        {
            if (otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.E_isPressed == true && masterKey == false)
            {
                GameManagerScript.Key_Color(keyColor);
                GameManagerScript.Keys_Strings.Add(keyColorType); //The key will be added to our inventory

                if (GameManagerScript.SFXToggle.isOn == true)
                {
                    gameManagerAudioSource.Stop();
                    gameManagerAudioSource.PlayOneShot(keySFX); //Grab key sound
                }

                Destroy(Key_ParticleSystem);
                Destroy(gameObject);
            }
            else if (otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.E_isPressed == true && masterKey == true)
            {
                GameManagerScript.Master_Key.SetActive(true); //The key will be added to our inventory
                Image Master_Key_Image = GameManagerScript.Master_Key.GetComponent<Image>();
                Master_Key_Image.color = keyColor;

                if (GameManagerScript.SFXToggle.isOn == true)
                {
                    gameManagerAudioSource.Stop();
                    gameManagerAudioSource.PlayOneShot(keySFX); //Grab key sound
                }

                Destroy(Key_ParticleSystem);
                Destroy(gameObject);
            }
        }       
        else
        {
            if(otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.E_isPressed == true && PlayerControllerScript.Key_Checked == false && ObjectScript.Stolen == true)
            {
                PlayerControllerScript.Key_Checked = true; //The key will be added to our inventory
                GameManagerScript.Key_Color(keyColor);
                GameManagerScript.Keys_Strings.Add(keyColorType);

                if (GameManagerScript.SFXToggle.isOn == true)
                {
                    gameManagerAudioSource.Stop();
                    gameManagerAudioSource.PlayOneShot(keySFX); //Grab key sound
                }

                Destroy(Key_ParticleSystem);
                Destroy(gameObject);
            }
        }
    }
}
