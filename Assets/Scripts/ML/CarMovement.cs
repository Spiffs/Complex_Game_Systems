using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField]
    [Range(0, 100)]
    private float Speed = 1;

    [SerializeField]
    [Range(0, 100)]
    private float RotateSpeed = 1;

    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // moving with the arrow keys
        transform.position += transform.up * Input.GetAxis("Vertical") * Speed / 50;

        // rotating with the arrow keys
        transform.Rotate(transform.forward, Input.GetAxis("Horizontal") * RotateSpeed);
    }
}
