using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public GameObject[] keysArray;
    public bool redDropped, yellowDropped, blueDropped = false;

    public float redRoomCounter;

    public float red_BlueRoomCounter;
    public float yellow_BlueRoomCounter;
    public float green_BlueRoomCounter;
    public float blue_BlueRoomCounter;
    public float purple_BlueRoomCounter;

    private bool greenKeyCollected, yellowKeyCollected, purpleKeyCollected = false;
    public bool blueKeyCollected, masterKeyCollected = false;

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
        if(PlayerControllerScript.isInRedRoom == true && redRoomCounter == 10 && greenKeyCollected == false)
        {
            greenKeyCollected = true;
            keysArray[0].SetActive(true); //Green key released
        }
        if(PlayerControllerScript.isInGreenRoom == true && redDropped == true && yellowDropped == true && blueDropped == true && yellowKeyCollected == false)
        {
            yellowKeyCollected = true;
            keysArray[1].SetActive(true); //Yellow key released
        }
        if (PlayerControllerScript.isInBlueRoom == true && red_BlueRoomCounter == 3 && yellow_BlueRoomCounter == 3 && green_BlueRoomCounter == 3 && blue_BlueRoomCounter == 3 && purple_BlueRoomCounter == 3 && purpleKeyCollected == false)
        {
            purpleKeyCollected = true;
            keysArray[3].SetActive(true); //Purple key released
        }
    }
}
