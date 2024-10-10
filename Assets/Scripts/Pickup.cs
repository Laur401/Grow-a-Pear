using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float followDistance = 1.0f;
    GameObject player;
    bool pickedUp=false;
    bool grabHappened = false;

    private Vector3 defaultScale;
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        defaultScale=transform.localScale;
    }

    void Update()
    {
        if (pickedUp)
        {
            transform.position=player.transform.position+followDistance * player.transform.localScale.x * player.transform.right;
            transform.rotation=player.transform.rotation;
            transform.localScale = player.transform.localScale;
        }
    }

    public void PickUpHandler(InputAction grabInput, GameObject playerObject)
    {
        if (grabInput.triggered&&!grabHappened)
        {
            if (!pickedUp)
            {
                PickObjUp(playerObject);
                grabHappened = true;
                Debug.Log("grab");
            }
            else
            {
                UnPickObjUp();
                grabHappened = true;
                Debug.Log("ungrab");
            }
        }
        else if (grabInput.WasCompletedThisFrame())
            grabHappened = false;
    }

    void PickObjUp(GameObject playerObject)
    {
        pickedUp = true;
        player = playerObject;
        rb.isKinematic = true;
        //TODO: Disable collision if picked up
    }
    void UnPickObjUp()
    {
        pickedUp = false;
        player = null;
        rb.isKinematic = false;
        transform.localScale = defaultScale;
    }
}

