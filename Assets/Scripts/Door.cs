using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public string doorColorType;
    private Animator doorChildAnim;
    private int doorKey;
    private Transform doorChild;

    //Scripts
    private GameManager GameManagerScript;
    private PlayerController PlayerControllerScript;
    private Key KeyScript;

    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        PlayerControllerScript = FindObjectOfType<PlayerController>();
        KeyScript = FindObjectOfType<Key>();

        doorChild = transform.GetChild(1);
        doorChildAnim = doorChild.GetComponent<Animator>(); //Acces to the specific part of the door
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
        if (otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.F_isPressed == true && PlayerControllerScript.Door_Checked == false)
        {
            if(GameManagerScript.Keys_Strings.Contains(doorColorType)) //Si la llave coincide con la puerta es cuando la abrimos
            {
                PlayerControllerScript.Door_Checked = true;
                doorKey = GameManagerScript.Keys_Strings.IndexOf(doorColorType);
                //GameManagerScript.Key_GameObject[doorKey].SetActive(false);               
                
                Image Key_Image = GameManagerScript.Key_GameObject[doorKey].GetComponent<Image>();
                Key_Image.color = new Vector4(Key_Image.color.r, Key_Image.color.g, Key_Image.color.b, 0.3f);
                GameObject Check_Image = GameManagerScript.Key_GameObject[doorKey].transform.GetChild(0).gameObject;
                Check_Image.SetActive(true);

                doorChildAnim.enabled = true;               

                if (GameManagerScript.isInTutorial == true)
                {
                    StartCoroutine(GameManagerScript.CloseText());
                }
            }

            else
            {
                Debug.Log("Mmmm..., te falta una llave"); // A�adir algo por UI
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
