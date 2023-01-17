using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;
    public float jumpSpeed = 2f;

    private Rigidbody playerRigidbody;
    public Transform playerOrientation;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float VerticalInput = Input.GetAxisRaw("Vertical");
        float HorizontalInput = Input.GetAxisRaw("Horizontal");

        playerRigidbody.AddForce(playerOrientation.transform.forward * speed * VerticalInput, ForceMode.Impulse);
        playerRigidbody.AddForce(playerOrientation.transform.right * speed * HorizontalInput, ForceMode.Impulse);

        if(Input.GetButtonDown("Jump"))
        {
            playerRigidbody.AddForce(playerOrientation.transform.up * jumpSpeed, ForceMode.Impulse);
        }


    }
}
