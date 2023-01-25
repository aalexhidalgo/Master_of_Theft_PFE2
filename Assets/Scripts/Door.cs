using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public string doorColorType;
    private Animator doorAnim;

    //Scripts
    private GameManager GameManagerScript;
    private PlayerController PlayerControllerScript;
    private Key KeyScript;

    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        PlayerControllerScript = FindObjectOfType<PlayerController>();
        KeyScript = FindObjectOfType<Key>();
        //doorAnim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider otherTrigger)
    {
        //Tutorial
        if (GameManagerScript.isInTutorial)
        {
            if (otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.Door_Checked == false)
            {
                StartCoroutine(GameManagerScript.DisplayText(2));
            }
        }
    }

    private void OnTriggerStay(Collider otherTrigger)
    {
        if (otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.F_isPressed == true)
        {
            if(GameManagerScript.Keys_Strings.Contains(doorColorType)) //Si la llave coincide con la puerta es cuando la abrimos
            {
                int doorKey = GameManagerScript.Keys_Strings.IndexOf(doorColorType);
                GameManagerScript.Key_GameObject[doorKey].SetActive(false);
                Debug.Log("Yei, has abierto la puerta");
                //Hacer true la animación
                if (GameManagerScript.isInTutorial == true)
                {
                    PlayerControllerScript.Door_Checked = true;
                    StartCoroutine(GameManagerScript.CloseText());
                }
            }

            else
            {
                Debug.Log("Mmmm..., te falta una llave");
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
