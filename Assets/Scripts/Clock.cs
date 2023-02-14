using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private float SpinSpeed = 70f;
    public AudioClip collectedAudio;
    public ParticleSystem collectedParticleSystem;

    private PlayerController PlayerControllerScript;
    private GameManager GameManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        PlayerControllerScript = FindObjectOfType<PlayerController>();   
    }
    void Update()
    {
        transform.Rotate(Vector3.up * SpinSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider otherTrigger)
    {
        if (otherTrigger.gameObject.CompareTag("Player"))
        {
            if(GameManagerScript.isInTutorial)
            {
                PlayerControllerScript.Clock_Checked = true;
                StartCoroutine(GameManagerScript.DisplayText(3));
            }

            GameManagerScript.TimeCounter(60); //Seconds
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider otherTrigger)
    {
        //Tutorial
        if (GameManagerScript.isInTutorial == true)
        {
            if (otherTrigger.gameObject.CompareTag("Player"))
            {
                StartCoroutine(GameManagerScript.CloseText());
            }
        }
    }
}
