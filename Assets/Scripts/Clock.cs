using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private float SpinSpeed = 70f;
    public AudioClip clockSFX;

    private Animator clockAnim;
    private Transform clockChild;

    private PlayerController PlayerControllerScript;
    private GameManager GameManagerScript;
    private TutorialManager TutorialManagerScript;

    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        PlayerControllerScript = FindObjectOfType<PlayerController>();
        TutorialManagerScript = FindObjectOfType<TutorialManager>();

        clockChild = transform.GetChild(0);
        clockAnim = clockChild.GetComponent<Animator>();
    }

    void Update()
    {       

        if (GameManagerScript.gameOver == true || GameManagerScript.pause == true)
        {
            clockAnim.enabled = false;
        }
        else
        {
            clockAnim.enabled = true;
            transform.Rotate(Vector3.up * SpinSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider otherTrigger)
    {
        if (GameManagerScript.isInTutorial == false)
        {
            if (otherTrigger.gameObject.CompareTag("Player"))
            {
                GameManagerScript.TimeCounter(60); //Seconds
                //Partículas y sonido
                Destroy(gameObject);
            }
        }
        else
        {
            if (otherTrigger.gameObject.CompareTag("Player") && GameManagerScript.Keys_Strings.Contains("red"))
            {
                PlayerControllerScript.Clock_Checked = true;
                GameManagerScript.TimeCounter(60); //Seconds
                //Partículas y sonido
                Destroy(gameObject);
            }
        }
        
    }
}
