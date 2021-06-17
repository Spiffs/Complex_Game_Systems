using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{

    [SerializeField]
    private float MaxSpeed = 10; // maximum speed the car can travel

    [SerializeField]
    private float Acceleration = 1; // maximum speed of acceleration 


    [SerializeField]
    private float MaxSteerAngle = 30; // maximum angle the car can steer

    [SerializeField]
    private float SteerAcceleration = 1; // maximum speed the steering can change;

    // script values
    public float CurrentSpeed = 0;
    public float CurrentSteerSpeed = 0;

    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // get inputs and alter the speed based on the inputs
        ApplyAcceneration(1);

        ApplyAccelerationAndSteering();
    }

    private void ApplyAcceneration(float Input)
    {
        CurrentSpeed += Acceleration * Input;
        CurrentSpeed = (Acceleration * Input / 10) * Mathf.Pow(CurrentSpeed, 3);

        if (CurrentSpeed > MaxSpeed)
            CurrentSpeed = MaxSpeed;
    }

    private void ApplyAccelerationAndSteering()
    {
        transform.position += transform.up * Time.fixedDeltaTime * CurrentSpeed;
        transform.eulerAngles = Vector3.forward * CurrentSteerSpeed;
    }
}
