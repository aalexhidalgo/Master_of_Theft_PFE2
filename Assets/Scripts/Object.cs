using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object: MonoBehaviour
{
    //Tutorial
    public bool Stolen;
    public bool Diamond;

    public Material transMat;

    public int Value;

    public AudioClip objectAudio;
    public ParticleSystem objectParticle;
        private AudioSource gameManagerAudioSource;

    //Scripts
    private GameManager GameManagerScript;
    private PlayerController PlayerControllerScript;

    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        PlayerControllerScript = FindObjectOfType<PlayerController>();

        gameManagerAudioSource = GameManagerScript.GetComponent<AudioSource>();
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider otherTrigger)
    {
        //Tutorial
        if (GameManagerScript.isInTutorial)
        {
            if (otherTrigger.gameObject.CompareTag("Player") && Stolen == false)
            {
                StartCoroutine(GameManagerScript.DisplayText(0));
            }
        }
    }

    private void OnTriggerStay(Collider otherTrigger)
    {
        GameObject objectToStole = transform.GetChild(0).gameObject;
        Material objectMat = objectToStole.GetComponent<Renderer>().material;

        if (otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.E_isPressed == true && Stolen == false)
        {
            Stolen = true;
            objectToStole.GetComponent<Renderer>().material = transMat;

            if(GameManagerScript.SFXToggle.isOn == true)
            {
                gameManagerAudioSource.Stop();
                gameManagerAudioSource.PlayOneShot(objectAudio);
            }

            StartCoroutine(GameManagerScript.AddMoney(Value));

            if (GameManagerScript.isInTutorial == true)
            {
                StartCoroutine(GameManagerScript.CloseText());

                if (Diamond == true)
                {
                    GameManagerScript.isInTutorial = false;
                    GameManagerScript.ChangeToGame();
                }
            }
        }
    }

    private void OnTriggerExit(Collider otherTrigger)
    {
        if (GameManagerScript.isInTutorial == true)
        {
            if (otherTrigger.gameObject.CompareTag("Player"))
            {
                StartCoroutine(GameManagerScript.CloseText());
            }
        }
    }
}
