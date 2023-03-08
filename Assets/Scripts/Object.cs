using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object: MonoBehaviour
{
    //Tutorial
    public bool Stolen;
    public Material transMat;

    public int Value;
    public bool redRoom;

    //Audio
    public AudioClip objectSFX;   
    private AudioSource gameManagerAudioSource;

    //Scripts
    private GameManager GameManagerScript;
    private PlayerController PlayerControllerScript;
    private TutorialManager TutorialManagerScript;
    private PuzzleManager PuzzleManagerScript;

    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        PlayerControllerScript = FindObjectOfType<PlayerController>();
        TutorialManagerScript = FindObjectOfType<TutorialManager>();
        PuzzleManagerScript = FindObjectOfType<PuzzleManager>();

        gameManagerAudioSource = GameManagerScript.GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider otherTrigger)
    {

        if (otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.E_isPressed == true && Stolen == false)
        {
            Stolen = true;

            Renderer[] AllChildren = GetComponentsInChildren<Renderer>();

            for (int i = 0; i < AllChildren.Length; i++)
            {
                AllChildren[i].material = transMat;
            }

            if (redRoom == true)
            {
                PuzzleManagerScript.redRoomCounter++;
            }

            if(GameManagerScript.SFXToggle.isOn == true)
            {
                gameManagerAudioSource.Stop();
                gameManagerAudioSource.PlayOneShot(objectSFX);
            }

            StartCoroutine(GameManagerScript.AddMoney(Value));

            if (GameManagerScript.isInTutorial == true)
            {
                StartCoroutine(TutorialManagerScript.CloseText());
                StartCoroutine(TutorialManagerScript.DisplayText(4, 2));               
            }
        }
    }
}
