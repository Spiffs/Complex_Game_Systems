using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform Target;

    void FixedUpdate()
    {
        transform.position = Target.position;  
    } // simply alters the position of the camera to line up with the target Object
}
