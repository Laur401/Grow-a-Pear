using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float followDistance = 1.0f;
    GameObject player;
    bool pickedUp;
    bool wasKinematic;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        pickedUp = false;
        wasKinematic = rb.isKinematic;
    }

    void Update()
    {
        if (pickedUp)
        {
            transform.position=player.transform.position+player.transform.right*followDistance; //TODO: Have the item "in front" of the player (aka the direction where the player is turned to)
            transform.rotation=player.transform.rotation;
            // TODO: Scale the object to the size of the player.
        }
        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")&&Input.GetKeyDown(KeyCode.Q)&&pickedUp==false)
            PickObjUp(other.gameObject);
        else if (Input.GetKeyDown(KeyCode.Q)&&pickedUp==true) //TODO: Fix player not being able to put the item down if trigger is just out of reach
            UnPickObjUp();
    }

    void PickObjUp(GameObject playerObject)
    {
        pickedUp = true;
        player=playerObject;
        rb.isKinematic=true;
        //TODO: Disable collision if picked up
        //rb.freezeRotation = true;
    }
    void UnPickObjUp()
    {
        pickedUp = false;
        //rb.freezeRotation = false;
        transform.parent=null;
        rb.isKinematic=false;
        player = null;
    }
}

