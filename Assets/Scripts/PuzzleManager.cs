using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public GameObject[] keysArray;
    public bool redDropped, yellowDropped, greenDropped, blueDropped, purpleDropped = false;

    public float redRoomCounter;

    public float red_BlueRoomCounter;
    public float yellow_BlueRoomCounter;
    public float green_BlueRoomCounter;
    public float blue_BlueRoomCounter;
    public float purple_BlueRoomCounter;

    //Scripts
    private PlayerController PlayerControllerScript;
    private GameManager GameManagerScript;
    private TutorialManager TutorialManagerScript;

    void Start()
    {
        PlayerControllerScript = FindObjectOfType<PlayerController>();
        GameManagerScript = FindObjectOfType<GameManager>();
        TutorialManagerScript = FindObjectOfType<TutorialManager>();
    }

    void Update()
    {
        if(!PlayerControllerScript.isInRedRoom && redRoomCounter == 10)
        {
            keysArray[0].SetActive(true); //Green key released
        }
        if(!PlayerControllerScript.isInGreenRoom && !redDropped && !yellowDropped && !blueDropped)
        {
            keysArray[1].SetActive(true); //Yellow key released
        }
        if(!PlayerControllerScript.isInYellowRoom)
        {
            //keysArray[2].SetActive(true); //Blue key released
        }
        if (!PlayerControllerScript.isInBlueRoom && red_BlueRoomCounter == 3 && yellow_BlueRoomCounter == 3 && green_BlueRoomCounter == 3 && blue_BlueRoomCounter == 3 && purple_BlueRoomCounter == 3)
        {
            keysArray[3].SetActive(true); //Purple key released
        }
        if (!PlayerControllerScript.isInPurpleRoom)
        {
            //keysArray[4].SetActive(true); //Master key released
        }
    }
}
