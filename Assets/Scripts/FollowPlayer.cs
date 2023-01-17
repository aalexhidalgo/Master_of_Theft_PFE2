using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject cameraPos;
    void Start()
    {
        cameraPos = GameObject.Find("CameraPos");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = cameraPos.transform.position;
    }
}
