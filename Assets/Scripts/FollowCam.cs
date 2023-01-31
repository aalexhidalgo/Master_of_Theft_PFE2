using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    private GameObject myCam;
    void Start()
    {
        myCam = GameObject.Find("Main Camera");
    }

    void Update()
    {
        transform.position = myCam.transform.position; //Why? Because of the animation of the camera holder when the guard attacks the player
    }
}
