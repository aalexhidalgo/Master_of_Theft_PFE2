using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsOrientation : MonoBehaviour
{
    public Transform playerOrientation;

    // Update is called once per frame
    void Update()
    {
        //Arms follow the camera rotation in the Horizontal Axis
        playerOrientation.rotation = transform.rotation;
    }
}
