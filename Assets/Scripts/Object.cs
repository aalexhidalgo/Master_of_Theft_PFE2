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
    public bool oneMoreMat = false;

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
        //We can only steal the object if we are prerssing the E key
        if (otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.E_isPressed == true && Stolen == false)
        {
            Stolen = true;
            Renderer[] AllChildren = GetComponentsInChildren<Renderer>();

            for (int i = 0; i < AllChildren.Length; i++)
            {
                Material[] mats = AllChildren[i].materials;
                mats[0] = transMat;

                if(oneMoreMat == true)
                {
                    mats[1] = transMat; //Because some models have more than one material inside the mesh renderer
                }   
                
                AllChildren[i].GetComponent<Renderer>().materials = mats; //Makes the object "disappear" with a transparent material
            }

            if (redRoom == true)
            {
                PuzzleManagerScript.redRoomCounter++; //If we steal all the objects (in the stands, not the painting) of the red room, the next key will appear
            }

            if(GameManagerScript.SFXToggle.isOn == true)
            {
                gameManagerAudioSource.Stop();
                gameManagerAudioSource.PlayOneShot(objectSFX); //The SFX of grabing something
            }

            StartCoroutine(GameManagerScript.AddMoney(Value)); //The value of the object added to the money counter

            if (GameManagerScript.isInTutorial == true)
            {
                StartCoroutine(TutorialManagerScript.CloseText());
                StartCoroutine(TutorialManagerScript.DisplayText(4, 2));             
            }
        }
    }
}
