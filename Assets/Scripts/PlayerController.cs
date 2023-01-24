using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;
    public float jumpSpeed = 2f;
    public float crouchSpeed;
    private float crouchYScale = 0.5f;
    private float startYScale = 1;
    public bool hasMoved;

    private Rigidbody playerRigidbody;
    private Vector3 newGravity = new Vector3(0f, -29.4f, 0f);
    public Transform playerOrientation;

    [SerializeField] private bool isOnTheGround; //To avoid double jump
    private bool E_isPressed, F_isPressed, Shift_isPressed;

    //TUTORIAL
    private bool Key_Checked, Door_Checked, Clock_Checked;

    public Material transMat;

    //Scripts
    private GameManager GameManagerScript;
    private Object ObjectScript;

    // Start is called before the first frame update
    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        ObjectScript = FindObjectOfType<Object>();

        playerRigidbody = GetComponent<Rigidbody>();
        Physics.gravity = newGravity;
    }

    // Update is called once per frame
    void Update()
    {
        float VerticalInput = Input.GetAxisRaw("Vertical");
        float HorizontalInput = Input.GetAxisRaw("Horizontal");

        Vector3 forwardAxis = new Vector3 (playerOrientation.forward.x, 0f, playerOrientation.forward.z).normalized;
        Vector3 rightAxis = new Vector3(playerOrientation.right.x, 0f, playerOrientation.right.z).normalized;

        playerRigidbody.AddForce(forwardAxis * speed * VerticalInput);
        playerRigidbody.AddForce(rightAxis * speed * HorizontalInput);

        if (Input.GetButtonDown("Jump") && isOnTheGround == true)
        {
            isOnTheGround = false;
            playerRigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            playerRigidbody.drag = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Shift_isPressed = true;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            playerRigidbody.AddForce(Vector3.down * crouchSpeed, ForceMode.Impulse);
            speed = 5; //We decrease the speed of the Player for when it has to move in crouching state
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Shift_isPressed = false;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            playerRigidbody.AddForce(Vector3.up * crouchSpeed, ForceMode.Impulse);
            speed = 10; //We set the speed of the Player to its maximum
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
            F_isPressed = true;
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            F_isPressed = false;
        }
       
        SpeedControl();

        if(Input.anyKey)
        {
            hasMoved = true;
        }
    }

    #region Collision and Trigger System
    private void OnCollisionEnter(Collision otherCollider)
    {
        //This allow us to jump only if we are standing on the ground, avoiding double jumps
        if (otherCollider.gameObject.CompareTag("Ground"))
        {
            isOnTheGround = true;
            playerRigidbody.drag = 2; //more realistic
        }
    }

    private void OnTriggerEnter(Collider otherTrigger)
    {
        //Tutorial
        if (GameManagerScript.isInTutorial)
        {
            if (otherTrigger.gameObject.CompareTag("Object") && otherTrigger.GetComponent<Object>().Stolen == false)
            {
                StartCoroutine(GameManagerScript.DisplayText(0));
            }

            if (otherTrigger.gameObject.CompareTag("Key") && Key_Checked == false)
            {
                StartCoroutine(GameManagerScript.DisplayText(1));
            }

            if (otherTrigger.gameObject.CompareTag("Door") && Door_Checked == false)
            {
                StartCoroutine(GameManagerScript.DisplayText(2));
            }
        }

        //Game
        if (otherTrigger.gameObject.CompareTag("Clock") && Clock_Checked == false)
        {
            Clock_Checked = true;
            StartCoroutine(GameManagerScript.DisplayText(3));
            GameManagerScript.TimeCounter(60); //Seconds
            Destroy(otherTrigger.gameObject);
        }
             
    }

    private void OnTriggerExit(Collider otherTrigger)
    {
        if(GameManagerScript.isInTutorial == true)
        {
            if (otherTrigger.gameObject.CompareTag("Object") || otherTrigger.gameObject.CompareTag("Key") || otherTrigger.gameObject.CompareTag("Door"))
            {
                StartCoroutine(GameManagerScript.CloseText());
            }
        }
    }

    private void OnTriggerStay(Collider otherTrigger)
    {
        GameObject otherGameObject = otherTrigger.transform.GetChild(0).gameObject;
        Material otherTriggerMat = otherGameObject.GetComponent<Renderer>().material;

        if (otherTrigger.gameObject.CompareTag("Object") && E_isPressed == true && otherTrigger.GetComponent<Object>().Stolen == false)
        {
            //Destroy(otherTrigger.gameObject);
            otherTrigger.GetComponent<Object>().Stolen = true;
            otherGameObject.GetComponent<Renderer>().material = transMat;

            GameManagerScript.AddMoney(otherTrigger.GetComponent<Object>().Value);

            if(GameManagerScript.isInTutorial == true)
            {
                StartCoroutine(GameManagerScript.CloseText());

                if (otherTrigger.GetComponent<Object>().Diamond == true)
                {
                    GameManagerScript.isInTutorial = false;
                    GameManagerScript.ChangeToGame();
                }
            }
        }

        if (otherTrigger.gameObject.CompareTag("Key") && E_isPressed == true)
        {
            GameManagerScript.Key_Collected++;
            Destroy(otherTrigger.gameObject); //Guardar (funci�n en el GameManager)

            if (GameManagerScript.isInTutorial == true)
            {
                Key_Checked = true;
                StartCoroutine(GameManagerScript.CloseText());
            }
        }

        if (otherTrigger.gameObject.CompareTag("Door") && F_isPressed == true)
        {
            //Setear animacion de abrir puerta
            if (GameManagerScript.isInTutorial == true)
            {
                Door_Checked = true;
                StartCoroutine(GameManagerScript.CloseText());
            }
        }
    }
    #endregion

    private void SpeedControl()
    {
        //To avoid acceleration in x and z axis
        Vector3 velocity = new Vector3(playerRigidbody.velocity.x, 0f, playerRigidbody.velocity.z);

        if(velocity.magnitude > speed)
        {
            Vector3 limitedVelocity = velocity.normalized * speed; //We set the rigidbody velocity to the max speed of the Player
            playerRigidbody.velocity = new Vector3 (limitedVelocity.x, playerRigidbody.velocity.y, limitedVelocity.z);
        }
    }
}
