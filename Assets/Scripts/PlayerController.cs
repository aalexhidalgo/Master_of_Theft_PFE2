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
    private float maxHeightJump = 7f;

    [SerializeField] private bool isOnTheGround;
    private bool E_isPressed, F_isPressed, Shift_isPressed;

    public Material transMat;

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
            isOnTheGround = false;
            playerRigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            playerRigidbody.drag = 0;
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
       
        SpeedControl();
        //Debug.Log(playerRigidbody.velocity.magnitude);
        /*
        if (transform.position.y > maxHeightJump)
        {
            playerRigidbody.AddForce(Vector3.down * 0.25f, ForceMode.Impulse);  PREGUNTAR MAÑANA (Se suman las fuerzas)       
        }*/
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

            if (otherTrigger.gameObject.CompareTag("Clock"))
            {
                Destroy(otherTrigger.gameObject); //SE suma al contador de tiempo (función en el gameManager)
            }
        }     
    }

    private void OnTriggerExit(Collider otherTrigger)
    {
        if(GameManagerScript.isInTutorial == true)
        {
            if (otherTrigger.gameObject.CompareTag("Object") || otherTrigger.gameObject.CompareTag("Key") || otherTrigger.gameObject.CompareTag("Door"))
            {
                GameManagerScript.CloseText();
            }
        }
    }

    private void OnTriggerStay(Collider otherTrigger)
    {
        GameObject otherGameObject = otherTrigger.transform.GetChild(0).gameObject;
        Material otherTriggerMat = otherGameObject.GetComponent<Renderer>().material;

        if (otherTrigger.gameObject.CompareTag("Object") && E_isPressed == true)
        {
            //Destroy(otherTrigger.gameObject);
            //otherTriggerMat.color = new Vector4(otherTriggerMat.color.r, otherTriggerMat.color.g, otherTriggerMat.color.b, 0.05f); //PREGUNTAR MAÑANA
            otherGameObject.GetComponent<Renderer>().material = transMat;

            if (otherTrigger.GetComponent<ObjectToStole>().Diamond == true && GameManagerScript.isInTutorial == true)
            {
                GameManagerScript.isInTutorial = false;
                GameManagerScript.ChangeToGame();
            }
        }

        if (otherTrigger.gameObject.CompareTag("Key") && E_isPressed == true)
        {
            Destroy(otherTrigger.gameObject); //Guardar (función en el GameManager)
        }

        if (otherTrigger.gameObject.CompareTag("Door") && F_isPressed == true)
        {
            //Setear animacion de abrir puerta
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
