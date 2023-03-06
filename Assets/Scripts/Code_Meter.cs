using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class Code_Meter : MonoBehaviour
{
    public GameObject codeMeter_YellowRoomPanel;
    public GameObject codeMeter_PurpleRoomPanel;

    public TextMeshProUGUI firstOptionText_YellowRoom;
    public TextMeshProUGUI secondOptionText_YellowRoom;
    public TextMeshProUGUI thirdOptionText_YellowRoom;

    public TextMeshProUGUI firstOptionText_PurpleRoom;
    public TextMeshProUGUI secondOptionText_PurpleRoom;
    public TextMeshProUGUI thirdOptionText_PurpleRoom;
    public TextMeshProUGUI fourthOptionText_PurpleRoom;

    private int firstOptionValue_YellowRoom;
    private int secondOptionValue_YellowRoom;
    private int thirdOptionValue_YellowRoom;

    private int firstOptionValue_PurpleRoom;
    private int secondOptionValue_PurpleRoom;
    private int thirdOptionValue_PurpleRoom;
    private int fourthOptionValue_PurpleRoom;

    private CinemachineVirtualCamera cvCamera;

    //Scripts
    private PlayerController PlayerControllerScript;
    private PuzzleManager PuzzleManagerScript;

    void Start()
    {
        PlayerControllerScript = FindObjectOfType<PlayerController>();
        PuzzleManagerScript = FindObjectOfType<PuzzleManager>();
        cvCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if(PlayerControllerScript.isInYellowRoom == true && firstOptionValue_YellowRoom == 4 && secondOptionValue_YellowRoom == 1 && thirdOptionValue_YellowRoom == 3)
        {
            PuzzleManagerScript.keysArray[2].SetActive(true); //Blue key released
            codeMeter_YellowRoomPanel.SetActive(false);
        }

        if (PlayerControllerScript.isInPurpleRoom == true && firstOptionValue_PurpleRoom == 6 && secondOptionValue_PurpleRoom == 2 && thirdOptionValue_PurpleRoom == 3 && fourthOptionValue_PurpleRoom == 7)
        {
            PuzzleManagerScript.keysArray[4].SetActive(true); //Master key released
            codeMeter_PurpleRoomPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider otherTrigger)
    {
        if (otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.isInYellowRoom == true)
        {
            codeMeter_YellowRoomPanel.SetActive(true);
            PlayerControllerScript.enabled = false;
            cvCamera.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.isInPurpleRoom == true)
        {
            codeMeter_PurpleRoomPanel.SetActive(true);
            PlayerControllerScript.enabled = false;
            cvCamera.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void CloseCode_Meter()
    {
        cvCamera.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerControllerScript.enabled = true;

        if(PlayerControllerScript.isInYellowRoom == true)
        {
            codeMeter_YellowRoomPanel.SetActive(false);
        }
        if(PlayerControllerScript.isInPurpleRoom == true)
        {
            codeMeter_PurpleRoomPanel.SetActive(false);
        }
    }

    public void SwitchNumber_Button(string selectedButton)
    {
        //YELLOW ROOM
        if(selectedButton == "Up_FirstOption_YellowRoom")
        {
            firstOptionValue_YellowRoom++;

            if (firstOptionValue_YellowRoom > 10)
            {
                firstOptionValue_YellowRoom = 0;
            }

            firstOptionText_YellowRoom.text = firstOptionValue_YellowRoom.ToString();

        }
        if (selectedButton == "Down_FirstOption_YellowRoom")
        {
            firstOptionValue_YellowRoom--;           

            if (firstOptionValue_YellowRoom < 0)
            {
                firstOptionValue_YellowRoom = 10;
            }

            firstOptionText_YellowRoom.text = firstOptionValue_YellowRoom.ToString();
        }

        if(selectedButton == "Up_SecondOption_YellowRoom")
        {
            secondOptionValue_YellowRoom++;
            
            if (secondOptionValue_YellowRoom > 10)
            {
                secondOptionValue_YellowRoom = 0;
            }

            secondOptionText_YellowRoom.text = secondOptionValue_YellowRoom.ToString();
        }
        if(selectedButton == "Down_SecondOption_YellowRoom")
        {
            secondOptionValue_YellowRoom--;

            if (secondOptionValue_YellowRoom < 0)
            {
                secondOptionValue_YellowRoom = 10;
            }

            secondOptionText_YellowRoom.text = secondOptionValue_YellowRoom.ToString();
        }

        if (selectedButton == "Up_ThirdOption_YellowRoom")
        {
            thirdOptionValue_YellowRoom++;

            if (thirdOptionValue_YellowRoom > 10)
            {
                thirdOptionValue_YellowRoom = 0;
            }

            thirdOptionText_YellowRoom.text = thirdOptionValue_YellowRoom.ToString();
        }
        if(selectedButton == "Down_ThirdOption_YellowRoom")
        {
            thirdOptionValue_YellowRoom--;

            if (thirdOptionValue_YellowRoom < 0)
            {
                thirdOptionValue_YellowRoom = 10;
            }

            thirdOptionText_YellowRoom.text = thirdOptionValue_YellowRoom.ToString();
        }

        //PURPLE ROOM
        if (selectedButton == "Up_FirstOption_PurpleRoom")
        {
            firstOptionValue_PurpleRoom++;

            if (firstOptionValue_PurpleRoom > 10)
            {
                firstOptionValue_PurpleRoom = 0;
            }

            firstOptionText_PurpleRoom.text = firstOptionValue_PurpleRoom.ToString();
        }
        if (selectedButton == "Down_FirstOption_PurpleRoom")
        {
            firstOptionValue_PurpleRoom--;

            if (firstOptionValue_PurpleRoom < 0)
            {
                firstOptionValue_PurpleRoom = 10;
            }

            firstOptionText_PurpleRoom.text = firstOptionValue_PurpleRoom.ToString();
        }

        if (selectedButton == "Up_SecondOption_PurpleRoom")
        {
            secondOptionValue_PurpleRoom++;

            if (secondOptionValue_PurpleRoom > 10)
            {
                secondOptionValue_PurpleRoom = 0;
            }

            secondOptionText_PurpleRoom.text = secondOptionValue_PurpleRoom.ToString();
        }
        if (selectedButton == "Down_SecondOption_PurpleRoom")
        {
            secondOptionValue_PurpleRoom--;

            if (secondOptionValue_PurpleRoom < 0)
            {
                secondOptionValue_PurpleRoom = 10;
            }

            secondOptionText_PurpleRoom.text = secondOptionValue_PurpleRoom.ToString();
        }

        if (selectedButton == "Up_ThirdOption_PurpleRoom")
        {
            thirdOptionValue_PurpleRoom++;

            if (thirdOptionValue_PurpleRoom > 10)
            {
                thirdOptionValue_PurpleRoom = 0;
            }

            thirdOptionText_PurpleRoom.text = thirdOptionValue_PurpleRoom.ToString();
        }
        if (selectedButton == "Down_ThirdOption_PurpleRoom")
        {
            thirdOptionValue_PurpleRoom--;

            if (thirdOptionValue_PurpleRoom < 0)
            {
                thirdOptionValue_PurpleRoom = 10;
            }

            thirdOptionText_PurpleRoom.text = thirdOptionValue_PurpleRoom.ToString();
        }
        if (selectedButton == "Up_FourthOption_PurpleRoom")
        {
            fourthOptionValue_PurpleRoom++;

            if (fourthOptionValue_PurpleRoom > 10)
            {
                fourthOptionValue_PurpleRoom = 0;
            }

            fourthOptionText_PurpleRoom.text = fourthOptionValue_PurpleRoom.ToString();
        }
        if (selectedButton == "Down_FourthOption_PurpleRoom")
        {
            fourthOptionValue_PurpleRoom--;

            if (fourthOptionValue_PurpleRoom < 0)
            {
                fourthOptionValue_PurpleRoom = 10;
            }

            fourthOptionText_PurpleRoom.text = fourthOptionValue_PurpleRoom.ToString();
        }
    }
}
