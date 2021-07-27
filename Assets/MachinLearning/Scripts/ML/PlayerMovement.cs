using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CML;

public class PlayerMovement : MonoBehaviour
{
    // speed
    [SerializeField]
    [Range(0, 50)]
    public float Speed = 1;

    // hold forward for ML
    [NonSerialized]
    public bool HoldForward = false;

    public bool CanPlay = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CanPlay)
        {
            // moving with the arrow keys
            transform.position += transform.right * Input.GetAxis("Vertical") * Speed / 100;

            // rotating with the arrow keys
            transform.Rotate(transform.forward, -Input.GetAxis("Horizontal") * (Speed / (10)));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "MLTarget")
        {
            CanPlay = false;
        } // if the AI fails on step
    }
}
