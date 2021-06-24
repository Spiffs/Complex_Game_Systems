using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float Speed = 1;

    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // applying force to move the car forward
        rb.AddForce(transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * Speed, ForceMode2D.Force);

    }
}
