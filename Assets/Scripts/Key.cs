using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    public Color keyColor;
    public string keyColorType;
    public bool masterKey;

    public AudioClip keySound;
    public ParticleSystem keyParticle;

    //Scripts
    private GameManager GameManagerScript;
    private PlayerController PlayerControllerScript;
    private TutorialManager TutorialManagerScript;
    private Object ObjectScript;

    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        PlayerControllerScript = FindObjectOfType<PlayerController>();
        TutorialManagerScript = FindObjectOfType<TutorialManager>();
        ObjectScript = FindObjectOfType<Object>();
    }

    private void OnTriggerStay(Collider otherTrigger)
    {
        if(GameManagerScript.isInTutorial == false)
        {
            if (otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.E_isPressed == true && masterKey == false)
            {
                GameManagerScript.Key_Color(keyColor);
                GameManagerScript.Keys_Strings.Add(keyColorType);

                //Partículas y sonido
                Destroy(gameObject);
            }
            else if (otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.E_isPressed == true && masterKey == true)
            {
                GameManagerScript.Master_Key.SetActive(true);
                Image Master_Key_Image = GameManagerScript.Master_Key.GetComponent<Image>();
                Master_Key_Image.color = keyColor;

                //Partículas y sonido
                Destroy(gameObject);
            }
        }       
        else
        {
            if(otherTrigger.gameObject.CompareTag("Player") && PlayerControllerScript.E_isPressed == true && PlayerControllerScript.Key_Checked == false && ObjectScript.Stolen == true)
            {
                PlayerControllerScript.Key_Checked = true;
                GameManagerScript.Key_Color(keyColor);
                GameManagerScript.Keys_Strings.Add(keyColorType);

                //Partículas y sonido
                Destroy(gameObject);
            }
        }
    }
}
