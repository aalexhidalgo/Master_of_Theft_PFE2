using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;
    public float jumpSpeed = 2f;

    private Rigidbody playerRigidbody;
    private Vector3 newGravity = new Vector3(0f, -29.4f, 0f);
    public Transform playerOrientation;

    [SerializeField] private bool IsOnTheGround;
    private bool E_isPressed, F_isPressed, Shift_isPressed;

    //Scripts
    private GameManager GameManagerScript;
    private ObjectToStole ObjectToStoleScript;

    // Start is called before the first frame update
    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        ObjectToStoleScript = FindObjectOfType<ObjectToStole>();

        playerRigidbody = GetComponent<Rigidbody>();
        Physics.gravity = newGravity;
    }

    // Update is called once per frame
    void Update()
    {
        float VerticalInput = Input.GetAxisRaw("Vertical");
        float HorizontalInput = Input.GetAxisRaw("Horizontal");

        playerRigidbody.AddForce(playerOrientation.transform.forward * speed * VerticalInput);
        playerRigidbody.AddForce(playerOrientation.transform.right * speed * HorizontalInput);

        if (Input.GetButtonDown("Jump"))
        {
            IsOnTheGround = false;
            playerRigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Shift_isPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Shift_isPressed = false;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            E_isPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            E_isPressed = false;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            E_isPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            E_isPressed = false;
        }
    }

    private void OnCollisionEnter(Collision otherCollider)
    {
        //Si colisiona contra el suelo el jugador puede volver a saltar
        if (otherCollider.gameObject.CompareTag("Ground"))
        {
            IsOnTheGround = true;
        }
    }

    private void OnTriggerEnter(Collider otherTrigger)
    {
        //Tutorial
        if (GameManagerScript.isInTutorial)
        {
            if (otherTrigger.gameObject.CompareTag("Object"))
            {
                GameManagerScript.DisplayText(0);
            }

            if (otherTrigger.gameObject.CompareTag("Key"))
            {
                GameManagerScript.DisplayText(1);
            }

            if (otherTrigger.gameObject.CompareTag("Door"))
            {
                GameManagerScript.DisplayText(2);
            }
        }     
    }

    private void OnTriggerExit(Collider otherTrigger)
    {
        if(GameManagerScript.isInTutorial)
        {
            if (otherTrigger.gameObject.CompareTag("Object") || otherTrigger.gameObject.CompareTag("Key") || otherTrigger.gameObject.CompareTag("Door"))
            {
                GameManagerScript.CloseText();
            }
        }
    }

    private void OnTriggerStay(Collider otherTrigger)
    {
        if (otherTrigger.gameObject.CompareTag("Object") && E_isPressed == true)
        {
            Destroy(otherTrigger.gameObject);

            if (ObjectToStoleScript.Diamond == true)
            {
                GameManagerScript.isInTutorial = false;
                GameManagerScript.ChangeToGame();
            }
        }

        if (otherTrigger.gameObject.CompareTag("Key") && E_isPressed == true)
        {
            Destroy(otherTrigger.gameObject);
        }

        if (otherTrigger.gameObject.CompareTag("Door") && F_isPressed == true)
        {
                
        }
    }
}
