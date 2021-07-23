using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CML;

public class MLBotMovement : MonoBehaviour
{
    // setting the speed of the bot
    [SerializeField]
    private float Speed = 1;

    // hold forward for ML
    [NonSerialized]
    public bool HoldForward = false;

    // script input manager
    private MLInput InputsFromML;

    void Start()
    {
        InputsFromML = GetComponent<MLBotBrain>().MLInput;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Horizontal") > 0)
        {
            // moving with the arrow keys
            transform.position += transform.up * Input.GetAxis("Vertical") * Speed / 100;

            // rotating with the arrow keys
            transform.Rotate(transform.forward, Input.GetAxis("Horizontal") * (Speed / (10 / 3)));
        }
        else
        {
            if (HoldForward)
            {
                transform.position += transform.right * 1 * Speed / 100;
            }

            // using AI inputsS
            float axisChecker = 0;

            // inputs either A or D
            // aleternativly could use IsKeyDown() from MLLib
            if (InputsFromML.GetKeysDown().Contains(KeyCode.A))
                axisChecker = 1;
            else if (InputsFromML.GetKeysDown().Contains(KeyCode.D))
                axisChecker = -1;

            // ml input user
            transform.Rotate(transform.forward, axisChecker * (Speed / (10 / 3)));

            // reset the MLInput using the Update function from MlLib
            InputsFromML.Update();
        }
    }

    public void SetSpeed(float newSpeed)
    {
        Speed = newSpeed;
    }
}
