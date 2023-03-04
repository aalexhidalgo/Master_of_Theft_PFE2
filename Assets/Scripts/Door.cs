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
    public bool masterDoor;

    //Audio
    public AudioClip[] doorSFX;

    //Scripts
    private GameManager GameManagerScript;
    private PlayerController PlayerControllerScript;
    private TutorialManager TutorialManagerScript;
    private Key KeyScript;

    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        PlayerControllerScript = FindObjectOfType<PlayerController>();
        TutorialManagerScript = FindObjectOfType<TutorialManager>();
        //KeyScript = FindObjectOfType<Key>();

        doorChild = transform.GetChild(1);
        doorChildAnim = doorChild.GetComponent<Animator>(); //Acces to the specific part of the door
    }

    private void OnTriggerStay(Collider otherTrigger)
    {
        if (otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.F_isPressed == true && PlayerControllerScript.Door_Checked == false)
        {
            if(GameManagerScript.Keys_Strings.Contains(doorColorType) && masterDoor == false) //Si la llave coincide con la puerta es cuando la abrimos
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
                    
                }
            }
            else if(GameManagerScript.Master_Key.activeInHierarchy == true && masterDoor == true)
            {
                PlayerControllerScript.Door_Checked = true;
                Image Key_Image = GameManagerScript.Master_Key.GetComponent<Image>();
                Key_Image.color = new Vector4(Key_Image.color.r, Key_Image.color.g, Key_Image.color.b, 0.3f);
                GameObject Check_Image = GameManagerScript.Master_Key.transform.GetChild(0).gameObject;
                Check_Image.SetActive(true);

                doorChildAnim.enabled = true;

                if (GameManagerScript.isInTutorial == true)
                {
                                       
                }

                GameManagerScript.Win();
            }

            else
            {
                Debug.Log("Mmmm..., it looks like you have to find the key"); //UI
            }
        }
    }
}
