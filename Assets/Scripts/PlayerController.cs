using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float VerticalInput;
    private float HorizontalInput;
    private Vector3 forwardAxis, rightAxis;

    private float speed = 50f;
    public float jumpSpeed = 2f;
    public float crouchSpeed;
    private float crouchYScale = 0.5f;
    private float startYScale = 1;

    public bool hasMoved;
    public bool hasBeenAttacked = false;

    private Rigidbody playerRigidbody;
    private Vector3 newGravity = new Vector3(0f, -29.4f, 0f);
    public Transform playerOrientation;
    public Transform playerEyes; //For puzzle

    [SerializeField] private bool isOnTheGround; //To avoid double jump
    public bool E_isPressed, F_isPressed, Shift_isPressed;

    //PUZZLE
    public bool isInRedRoom, isInGreenRoom, isInYellowRoom, isInBlueRoom, isInPurpleRoom = false;

    //Audio

    //TUTORIAL
    public bool Key_Checked, Door_Checked, Clock_Checked;
    public bool move, jump, crouch;
    private bool time_Added;

    //Scripts
    private GameManager GameManagerScript;
    private TutorialManager TutorialManagerScript;
    private Object ObjectScript;
    private PickUpObject PickUpObjectScript;
    private PuzzleManager PuzzleManagerScript;

    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        ObjectScript = FindObjectOfType<Object>();
        TutorialManagerScript = FindObjectOfType<TutorialManager>();
        PickUpObjectScript = FindObjectOfType<PickUpObject>();
        PuzzleManagerScript = FindObjectOfType<PuzzleManager>();

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

        if(GameManagerScript.isInTutorial == true) //Messages for the tutorial
        {
            if (Key_Checked == true)
            {
                Key_Checked = false;
                StartCoroutine(TutorialManagerScript.CloseText());
                StartCoroutine(TutorialManagerScript.DisplayText(6, 2));
            }

            if(PickUpObjectScript.pickedUp == true)
            {
                PickUpObjectScript.pickedUp = false;
                StartCoroutine(TutorialManagerScript.CloseText());
            }

            if (Clock_Checked == true)
            {
                Clock_Checked = false;
                time_Added = true;
                StartCoroutine(TutorialManagerScript.CloseText());
                StartCoroutine(TutorialManagerScript.DisplayText(7, 2));
            }

            if (Door_Checked == true)
            {
                Door_Checked = false;
                StartCoroutine(TutorialManagerScript.CloseText());
                StartCoroutine(TutorialManagerScript.ChangeToGame());
            }
        }
    }

    private bool hasJumped = false;

    void FixedUpdate()
    {
        playerRigidbody.AddForce(forwardAxis * speed * VerticalInput);
        playerRigidbody.AddForce(rightAxis * speed * HorizontalInput);

        if(hasJumped == true && move == true)
        {
            playerRigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            hasJumped = false;
        }
    }

    public void Movement()
    {
        if(GameManagerScript.gameOver == false && GameManagerScript.pause == false && GameManagerScript.win == false) //false
        {
            VerticalInput = Input.GetAxisRaw("Vertical");
            HorizontalInput = Input.GetAxisRaw("Horizontal");

            forwardAxis = new Vector3(playerOrientation.forward.x, 0f, playerOrientation.forward.z).normalized;
            rightAxis = new Vector3(playerOrientation.right.x, 0f, playerOrientation.right.z).normalized;

            //playerRigidbody.AddForce(forwardAxis * speed * VerticalInput);
            //playerRigidbody.AddForce(rightAxis * speed * HorizontalInput);

            if((VerticalInput > 0 || HorizontalInput > 0) && GameManagerScript.isInTutorial == true && move == false)
            {
                move = true;
                StartCoroutine(TutorialManagerScript.CloseText());
                StartCoroutine(TutorialManagerScript.DisplayText(1, 2));              
            }

            if (Input.GetButtonDown("Jump") && isOnTheGround == true && move == true)
            {
                hasJumped = true;
                isOnTheGround = false;
                speed = 20f;
                //playerRigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                playerRigidbody.drag = 0;

                if(GameManagerScript.isInTutorial == true && jump == false)
                {
                    jump = true;
                    StartCoroutine(TutorialManagerScript.CloseText());
                    StartCoroutine(TutorialManagerScript.DisplayText(2, 2));
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && jump == true)
            {
                Shift_isPressed = true;
                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                playerRigidbody.AddForce(Vector3.down * crouchSpeed, ForceMode.Impulse);
                speed = 30; //We decrease the speed of the Player for when it has to move in crouching state

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
                speed = 50; //We set the speed of the Player to its maximum
            }

            if (Input.GetKeyDown(KeyCode.E) && crouch == true)
            {
                E_isPressed = true;
            }

            if (Input.GetKeyUp(KeyCode.E))
            {
                E_isPressed = false;
            }
            if (Input.GetKeyDown(KeyCode.F) && time_Added == true && GameManagerScript.isInTutorial == true)
            {
                F_isPressed = true;
            }

            if(Input.GetKeyDown(KeyCode.F) && GameManagerScript.isInTutorial == false)
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
    private void OnCollisionStay(Collision otherCollider)
    {
        //This allow us to jump only if we are standing on the ground, avoiding double jumps
        if (otherCollider.gameObject.CompareTag("Ground"))
        {
            if(Shift_isPressed == false)
            {
                speed = 50f;
            }
            isOnTheGround = true;
            playerRigidbody.drag = 2; //more realistic
        }
    }

    //PUZZLES:
    private void OnTriggerEnter(Collider otherTrigger)
    {
        if(otherTrigger.gameObject.CompareTag("Red_Room"))
        {
            //In this room you will have to steal everything you see to get the green key.
            isInRedRoom = true;
        }
        if (otherTrigger.gameObject.CompareTag("Green_Room"))
        {
            //In this room 3 coloured lecterns will appear, in these 3 you will have to deposit 1 object of the colour of the lectern, once achieved the yellow key will appear.
            isInGreenRoom = true;
        }
        if(otherTrigger.gameObject.CompareTag("Yellow_Room"))
        {
            //In this room you will enter 3 numbers corresponding to the number of objects of each colour that are in the room, once you have done so, the blue key will appear.
            isInYellowRoom = true;
        }
        if(otherTrigger.gameObject.CompareTag("Blue_Room"))
        {
            //In this room you will try to rearrange the coloured objects in their corresponding lectern, once you have succeeded the lilac key will appear.
            PuzzleManagerScript.redDropped = false;
            PuzzleManagerScript.yellowDropped = false;
            PuzzleManagerScript.blueDropped = false;
            isInBlueRoom = true;
        }
        if (otherTrigger.gameObject.CompareTag("Purple_Room"))
        {
            //In this one you will have to enter a code in a certain colour order, each room will have a number, a master key will appear.
            isInPurpleRoom = true;
        }
    }

    private void OnTriggerExit(Collider otherTrigger)
    {
        if (otherTrigger.gameObject.CompareTag("Red_Room"))
        {
            isInRedRoom = false;
        }
        if (otherTrigger.gameObject.CompareTag("Green_Room"))
        {
            isInGreenRoom = false;
        }
        if (otherTrigger.gameObject.CompareTag("Yellow_Room"))
        {
            isInYellowRoom = false;
        }
        if (otherTrigger.gameObject.CompareTag("Blue_Room"))
        {
            isInBlueRoom = true;
        }
        if (otherTrigger.gameObject.CompareTag("Purple_Room"))
        {
            isInPurpleRoom = true;
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
