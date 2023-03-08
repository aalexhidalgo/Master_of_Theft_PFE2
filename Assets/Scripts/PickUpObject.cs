using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    private bool dropped = false;
    public bool pickedUp;
    public string color;
    public bool hasScored = false;

    private bool E_released = true;

    private Rigidbody objectRigidbody;

    private PlayerController PlayerControllerScript;
    private GameManager GameManagerScript;
    private TutorialManager TutorialManagerScript;
    private PuzzleManager PuzzleManagerScript;
    private Object ObjectScript;

    void Start()
    {
        PlayerControllerScript = FindObjectOfType<PlayerController>();
        GameManagerScript = FindObjectOfType<GameManager>();
        TutorialManagerScript = FindObjectOfType<TutorialManager>();
        PuzzleManagerScript = FindObjectOfType<PuzzleManager>();
        ObjectScript = FindObjectOfType<Object>();

        objectRigidbody = transform.parent.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.E)) //To drop the object
        {
            E_released = true;
            Drop();
        }
        if (Input.GetKey(KeyCode.E)) //To hold on hands the objects
        {
            E_released = false;
        }
    }

    private void OnTriggerStay(Collider otherTrigger)
    {
        if (GameManagerScript.isInTutorial == true && ObjectScript.Stolen == true)
        {
            if (otherTrigger.gameObject.CompareTag("Player") && Input.GetKey(KeyCode.E))
            {
                PickUp(); //To hold the object
            }

            if (otherTrigger.gameObject.CompareTag("Drop_Area_red") && color == "red" && E_released == true && dropped == false)
            {
                dropped = true;
                PuzzleManagerScript.keysArray[0].SetActive(true);
                StartCoroutine(TutorialManagerScript.CloseText());
                StartCoroutine(TutorialManagerScript.DisplayText(5, 2));
                transform.parent.position = otherTrigger.transform.position; //To attach the object to the drop area
                transform.parent.rotation = otherTrigger.transform.rotation;
            }
        }

        if (GameManagerScript.isInTutorial == false)
        {
            if (otherTrigger.gameObject.CompareTag("Player") && Input.GetKey(KeyCode.E))
            {
                PickUp();
            }

            if (otherTrigger.gameObject.CompareTag($"Drop_Area_red") && color == "red" && E_released == true)
            {
                PuzzleManagerScript.redDropped = true; //To know if the object has been put in the correct stand (green room);
                transform.parent.position = otherTrigger.transform.position;
                transform.parent.rotation = otherTrigger.transform.rotation;

                if (PlayerControllerScript.isInBlueRoom == true && hasScored == false)
                {
                    PuzzleManagerScript.red_BlueRoomCounter++; //To know if every object has been put in the blue room
                    hasScored = true;
                }
            }

            if (otherTrigger.gameObject.CompareTag($"Drop_Area_yellow") && color == "yellow" && E_released == true)
            {
                PuzzleManagerScript.yellowDropped = true; //To know if the object has been put in the correct stand (green room);
                transform.parent.position = otherTrigger.transform.position;
                transform.parent.rotation = otherTrigger.transform.rotation;

                if (PlayerControllerScript.isInBlueRoom == true && hasScored == false)
                {
                    PuzzleManagerScript.yellow_BlueRoomCounter++; //To know if every object has been put in the blue roo
                    hasScored = true;
                }
            }

            if (otherTrigger.gameObject.CompareTag($"Drop_Area_blue") && color == "blue" && E_released == true && dropped == false)
            {
                PuzzleManagerScript.blueDropped = true; //To know if the object has been put in the correct stand (green room);
                transform.parent.position = otherTrigger.transform.position;
                transform.parent.rotation = otherTrigger.transform.rotation;

                if (PlayerControllerScript.isInBlueRoom == true && hasScored == false)
                {
                    PuzzleManagerScript.blue_BlueRoomCounter++; //To know if every object has been put in the blue roo
                    hasScored = true;
                }
            }

            if (otherTrigger.gameObject.CompareTag($"Drop_Area_green") && color == "green")
            {
                transform.parent.position = otherTrigger.transform.position;
                transform.parent.rotation = otherTrigger.transform.rotation;

                if (PlayerControllerScript.isInBlueRoom == true && hasScored == false)
                {
                    PuzzleManagerScript.green_BlueRoomCounter++; //To know if every object has been put in the blue roo
                    hasScored = true;
                }
            }

            if (otherTrigger.gameObject.CompareTag($"Drop_Area_purple") && color == "purple")
            {
                transform.parent.position = otherTrigger.transform.position;
                transform.parent.rotation = otherTrigger.transform.rotation;

                if (PlayerControllerScript.isInBlueRoom == true && hasScored == false)
                {
                    PuzzleManagerScript.purple_BlueRoomCounter++; //To know if every object has been put in the blue roo
                    hasScored = true;
                }
            }
        }
    }

    public void PickUp()
    {
        if(dropped == false)
        {
            pickedUp = true;
            objectRigidbody.useGravity = false;
            Collider childCollider = transform.parent.GetComponent<Collider>();
            childCollider.isTrigger = true;
            transform.position = PlayerControllerScript.playerEyes.position;
            transform.parent.SetParent(PlayerControllerScript.playerEyes);
        }
    }

    public void Drop()
    {
        objectRigidbody.useGravity = true;
        Collider childCollider = transform.parent.GetComponent<Collider>();
        childCollider.isTrigger = false;
        PlayerControllerScript.playerEyes.DetachChildren();
    }
}
