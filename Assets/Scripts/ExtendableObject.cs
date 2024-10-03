using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendableObject : MonoBehaviour, IActivator
{
    [SerializeField] private Transform stopPoint;
    [SerializeField] private float extendSpeed=1.0f;
    //[SerializeField] GameObject spriteMaskGameObject;
    
    private Vector3 initialPosition;
    //private BoxCollider2D objectCollider;
    private SpriteMask spriteMask;
    private bool active=false;
    public bool Active
    {
        get => active;
        set => active=value;
    }
    
    void Start()
    {
        initialPosition=transform.position;
        //objectCollider=GetComponent<BoxCollider2D>();
        //spriteMask=spriteMaskGameObject.GetComponent<SpriteMask>();
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
            //ColliderEdit();
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

    /*private void ColliderEdit()
    {
        var spriteMaskBounds = spriteMask.sprite.bounds;
        
        if (spriteMaskBounds.Contains(objectCollider.bounds.min)&&spriteMaskBounds.Contains(objectCollider.bounds.max))
            objectCollider.enabled=false;
        else if (transform.position.x < spriteMaskBounds.max.x && transform.position.x > spriteMaskBounds.min.x)
        {
            objectCollider.enabled=true;
            float difference = objectCollider.size.x - transform.position.x;
            objectCollider.size = new Vector2(difference, objectCollider.size.y);
            objectCollider.offset = new Vector2(objectCollider.offset.x + difference / 2, objectCollider.offset.y);
        }
        
    }*/
}
