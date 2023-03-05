using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public bool pickUpObject;

    private PlayerController PlayerControllerScript;
    // Start is called before the first frame update
    void Start()
    {
        PlayerControllerScript = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider otherTrigger)
    {
        if (otherTrigger.gameObject.CompareTag("Drop_Area") && pickUpObject == true)
        {
            Debug.Log("Hola");
            PlayerControllerScript.playerEyes.DetachChildren();
            transform.position = otherTrigger.transform.position;
            transform.rotation = otherTrigger.transform.rotation;
        }
    }
}
