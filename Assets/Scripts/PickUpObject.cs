using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.E))
        {
            E_released = true;
            Drop();
        }
        if (Input.GetKey(KeyCode.E))
        {
            E_released = false;
        }
    }

    private void OnTriggerEnter(Collider otherTrigger)
    {
        if (otherTrigger.gameObject.CompareTag("Drop_Area") && GameManagerScript.isInTutorial == true && pickedUp == false)
        {
            PuzzleManagerScript.keysArray[0].SetActive(true);
            StartCoroutine(TutorialManagerScript.CloseText());
            StartCoroutine(TutorialManagerScript.DisplayText(5, 2));
        }
    }

    private void OnTriggerStay(Collider otherTrigger)
    {
        if (GameManagerScript.isInTutorial == true && ObjectScript.Stolen == true)
        {
            if (otherTrigger.gameObject.CompareTag("Player") && Input.GetKey(KeyCode.E))
            {
                PickUp();
            }
        }
        else if (GameManagerScript.isInTutorial == false)
        {
            if (otherTrigger.gameObject.CompareTag("Player") && Input.GetKey(KeyCode.E))
            {
                PickUp();
            }
        }
        if (GameManagerScript.isInTutorial == false)
        {
            if (otherTrigger.gameObject.CompareTag($"Drop_Area_red") && color == "red" && E_released == true)
            {
                PuzzleManagerScript.redDropped = true;

                if (PlayerControllerScript.isInBlueRoom == true && hasScored == false)
                {
                    PuzzleManagerScript.red_BlueRoomCounter++;
                    hasScored = true;
                }
            }

            if (otherTrigger.gameObject.CompareTag($"Drop_Area_yellow") && color == "yellow" && E_released == true)
            {
                PuzzleManagerScript.yellowDropped = true;

                if (PlayerControllerScript.isInBlueRoom == true && hasScored == false)
                {
                    PuzzleManagerScript.yellow_BlueRoomCounter++;
                    hasScored = true;
                }
            }

            if (otherTrigger.gameObject.CompareTag($"Drop_Area_blue") && color == "blue" && E_released == true)
            {
                PuzzleManagerScript.blueDropped = true;

                if (PlayerControllerScript.isInBlueRoom == true && hasScored == false)
                {
                    PuzzleManagerScript.blue_BlueRoomCounter++;
                    hasScored = true;
                }
            }

            if (otherTrigger.gameObject.CompareTag($"Drop_Area_green") && color == "green")
            {
                if (PlayerControllerScript.isInBlueRoom == true && hasScored == false)
                {
                    PuzzleManagerScript.green_BlueRoomCounter++;
                    hasScored = true;
                }
            }

            if (otherTrigger.gameObject.CompareTag($"Drop_Area_purple") && color == "purple")
            {
                if (PlayerControllerScript.isInBlueRoom == true && hasScored == false)
                {
                    PuzzleManagerScript.purple_BlueRoomCounter++;
                    hasScored = true;
                }
            }
        }
    }

    public void PickUp()
    {
        pickedUp = true;
        objectRigidbody.useGravity = false;
        Collider childCollider = transform.parent.GetComponent<Collider>();
        childCollider.isTrigger = true;
        transform.position = PlayerControllerScript.playerEyes.position;
        transform.parent.SetParent(PlayerControllerScript.playerEyes);
    }

    public void Drop()
    {
        objectRigidbody.useGravity = true;
        Collider childCollider = transform.parent.GetComponent<Collider>();
        childCollider.isTrigger = false;
        PlayerControllerScript.playerEyes.DetachChildren();
    }
}
