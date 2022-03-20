using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    const float tau = Mathf.PI * 2;

    Vector3 startingPosition;
    float movementFactor;

    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;

    void Start()
    {
        startingPosition = transform.position;
    }

    // I have set a try / catch in case another math error occurs that I haven't accounted for
    void Update()
    {
        try
        {
            SetMovementFactor();
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            movementFactor = 0f;
        }

        var offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }

    void SetMovementFactor()
    {
        if (period <= Mathf.Epsilon)
        {
            return; // guard against dividing by 0
        }
        
        var cycles = Time.time / period; 
        var rawSineWave = Mathf.Sin(cycles * tau); // will give value between 1 (top of wave) and -1 (bottom of wave)

        movementFactor = (rawSineWave + 1f) / 2f; // transforms -1 to 1 to 0 to 2, so then divide by 2 to get the range (0 - 1)
    }
}
