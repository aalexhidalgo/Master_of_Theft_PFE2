using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public string keyColor;

    private GameManager GameManagerScript;
    void Start()
    {
        GameManagerScript = FindObjectOfType<GameManager>();
        GameManagerScript.Key_Color(keyColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
