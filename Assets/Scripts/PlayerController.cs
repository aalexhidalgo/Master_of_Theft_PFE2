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

    // Start is called before the first frame update
    void Start()
    {
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

        }

        if (Input.GetKeyDown(KeyCode.F))
        {

        }

        if (Input.GetKeyDown(KeyCode.E))
        {

        }
    }

    public void OnCollisionEnter(Collision otherCollider)
    {
        //Si colisiona contra el suelo el jugador puede volver a saltar
        if (otherCollider.gameObject.CompareTag("Ground"))
        {
            IsOnTheGround = true;
        }
    }
}
