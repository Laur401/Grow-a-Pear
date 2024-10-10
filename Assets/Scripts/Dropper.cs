using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Dropper : MonoBehaviour, IActivator
{
    [SerializeField] private GameObject itemToDrop;
    private GameObject droppedItem;
    private SpriteRenderer spriteRenderer;
    
    private bool active = false;
    private bool dispensed = false;
    public bool Active
    {
        get => active;
        set
        {
            if (value && !active && droppedItem != null)
            {
                RemoveDroppedItem();
                dispensed = false;
            }
            active = value;
        } 
    }
    void Start()
    {
        spriteRenderer=GetComponent<SpriteRenderer>(); //To be used for animations
    }
    
    void Update()
    {
        if (active&&!dispensed)
            Dispense();
    }

    void Dispense()
    {   
        Vector3 dispensePosition=new Vector3(transform.localPosition.x,transform.localPosition.y-itemToDrop.transform.localScale.y,0f);
        droppedItem=Instantiate(itemToDrop,dispensePosition,Quaternion.identity);
        dispensed = true;
    }
    void RemoveDroppedItem() => Destroy(droppedItem);
}
