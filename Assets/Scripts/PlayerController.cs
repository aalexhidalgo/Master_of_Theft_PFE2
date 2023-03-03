using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 15f;
    public float jumpSpeed = 2f;
    public float crouchSpeed;
    private float crouchYScale = 0.5f;
    private float startYScale = 1;

    public bool hasMoved;
    public bool hasBeenAttacked = false;

    private Rigidbody playerRigidbody;
    private Vector3 newGravity = new Vector3(0f, -29.4f, 0f);
    public Transform playerOrientation;

    [SerializeField] private bool isOnTheGround; //To avoid double jump
    public bool E_isPressed, F_isPressed, Shift_isPressed;

    //TUTORIAL
    public bool Key_Checked, Door_Checked, Clock_Checked;
    public bool move, jump, crouch;

    //Scripts
    private GameManager GameManagerScript;
    private TutorialManager TutorialManagerScript;
    private Object ObjectScript;

    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        ObjectScript = FindObjectOfType<Object>();
        TutorialManagerScript = FindObjectOfType<TutorialManager>();

        playerRigidbody = GetComponent<Rigidbody>();
        Physics.gravity = newGravity;        
    }

    void Update()
    {
        Movement();     
        SpeedControl();

        if(Input.anyKey)
        {
            hasMoved = true;
        }
    }

    public void Movement()
    {
        if(GameManagerScript.gameOver == false && GameManagerScript.pause == false && GameManagerScript.win == false) //false
        {
            float VerticalInput = Input.GetAxisRaw("Vertical");
            float HorizontalInput = Input.GetAxisRaw("Horizontal");

            Vector3 forwardAxis = new Vector3(playerOrientation.forward.x, 0f, playerOrientation.forward.z).normalized;
            Vector3 rightAxis = new Vector3(playerOrientation.right.x, 0f, playerOrientation.right.z).normalized;

            playerRigidbody.AddForce(forwardAxis * speed * VerticalInput);
            playerRigidbody.AddForce(rightAxis * speed * HorizontalInput);

            if((VerticalInput > 0 || HorizontalInput > 0) && GameManagerScript.isInTutorial == true && move == false)
            {
                move = true;
                StartCoroutine(TutorialManagerScript.CloseText());
                StartCoroutine(TutorialManagerScript.DisplayText(1, 2));              
            }

            if (Input.GetButtonDown("Jump") && isOnTheGround == true && move == true)
            {
                isOnTheGround = false;
                playerRigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                playerRigidbody.drag = 0;

                if(GameManagerScript.isInTutorial == true && jump == false)
                {
                    jump = true;
                    StartCoroutine(TutorialManagerScript.CloseText());
                    StartCoroutine(TutorialManagerScript.DisplayText(2, 2));
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && move == true && jump == true)
            {
                Shift_isPressed = true;
                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                playerRigidbody.AddForce(Vector3.down * crouchSpeed, ForceMode.Impulse);
                speed = 7; //We decrease the speed of the Player for when it has to move in crouching state

                if (GameManagerScript.isInTutorial == true && crouch == false)
                {
                    crouch = true;
                    StartCoroutine(TutorialManagerScript.CloseText());
                    StartCoroutine(TutorialManagerScript.DisplayText(3, 2));
                }
            }

            if (Input.GetKeyUp(KeyCode.LeftShift) && jump == true)
            {
                Shift_isPressed = false;
                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
                playerRigidbody.AddForce(Vector3.up * crouchSpeed, ForceMode.Impulse);
                speed = 15; //We set the speed of the Player to its maximum
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
