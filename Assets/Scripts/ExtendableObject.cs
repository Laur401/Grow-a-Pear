using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendableObject : MonoBehaviour, IActivator
{
    [SerializeField] private Transform stopPoint;
    [SerializeField] private float extendSpeed=1.0f;

    private Vector3 initialPosition;
    private bool active=false;
    public bool Active
    {
        get => active;
        set => active=value;
    }
    
    void Start()
    {
        initialPosition=transform.position;
    }
    
    void Update()
    {
        if (active)
            Extend();
        if (!active)
            Retract();
    }

    private void Extend()
    {
        Vector3 direction=stopPoint.position-transform.position;
        if (direction.magnitude > extendSpeed * Time.deltaTime)
        {
            direction.Normalize();
            transform.position+=direction * (extendSpeed * Time.deltaTime);
        }
    }

    private void Retract()
    {
        Vector3 direction=initialPosition-transform.position;
        if (direction.magnitude > extendSpeed * Time.deltaTime)
        {
            direction.Normalize();
            transform.position+=direction*(extendSpeed*Time.deltaTime);
        }
    }
}
