using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationThrust = 1000f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        // GetKey -> hold down, GetKeyDown -> one frame
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        }
    }

    void ProcessRotation()
    {
        // Default left
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            ApplyRotation(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            ApplyRotation(Vector3.back);
        }
    }

    void ApplyRotation(Vector3 vector3)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(vector3 * rotationThrust * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing rotation so physics system can take over
    }
}
