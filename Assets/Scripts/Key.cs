using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public Color keyColor;
    public string keyColorType;

    public AudioClip keySound;
    public ParticleSystem keyParticle;

    //Scripts
    private GameManager GameManagerScript;
    private PlayerController PlayerControllerScript;

    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        PlayerControllerScript = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider otherTrigger)
    {
        //Tutorial
        if (GameManagerScript.isInTutorial)
        {
            if (otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.Key_Checked == false)
            {
                StartCoroutine(GameManagerScript.DisplayText(1));
            }
        }
    }

    private void OnTriggerStay(Collider otherTrigger)
    {
        if (otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.E_isPressed == true)
        {
            GameManagerScript.Key_Collected++;
            GameManagerScript.Key_Color(keyColor);
            GameManagerScript.Keys_Strings.Add(keyColorType);

            Destroy(gameObject);

            if (GameManagerScript.isInTutorial == true)
            {
                PlayerControllerScript.Key_Checked = true;
                StartCoroutine(GameManagerScript.CloseText());
            }

        }
    }

    private void OnTriggerExit(Collider otherTrigger)
    {
        //Tutorial
        if (GameManagerScript.isInTutorial == true)
        {
            if (otherTrigger.gameObject.CompareTag("Player"))
            {
                StartCoroutine(GameManagerScript.CloseText());
            }
        }
    }
}
