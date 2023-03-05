using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public GameObject follow;

    void Update()
    {
        transform.position = follow.transform.position; //Why? Because of the animation of the camera holder when the guard attacks the player or because the trigger of the objects to be picked up
    }
}
