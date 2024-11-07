using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private float followDistance = 1.0f;
    GameObject player;
    public bool pickedUp = false;
    bool grabHappened = false;
    bool cont = false;

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
            transform.position=player.transform.position + (followDistance * player.transform.localScale.x + transform.localScale.x) * player.transform.right;
            transform.rotation=player.transform.rotation;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(player.transform.localScale.x);
            transform.localScale = scale;
        }
    }

    public int PickUpHandler(InputAction grabInput, GameObject playerObject)
    {
        if (grabInput.triggered)
        {
            if (!pickedUp)
            {
                StartCoroutine(PickObjUp(playerObject));
                grabHappened = true;
                return 1; //1 for picked up, 2 for unpicked, 0 for no change
                //Debug.Log("grab");
            }
            else
            {
                //UnPickObjUp();
                cont = true;
                grabHappened = true;
                return 2;
                //Debug.Log("ungrab");
            }
        }
        return 0;
    }

    public IEnumerator ObjectRemover()
    {
        cont = true;
        yield return new WaitUntil(()=>pickedUp=false);
        Destroy(gameObject);
    }
    
    IEnumerator PickObjUp(GameObject playerObject)
    {
        pickedUp = true;
        player = playerObject;
        rb.isKinematic = true;
        
        yield return new WaitUntil(() => cont);
        cont = false;
        
        pickedUp = false;
        player = null;
        rb.isKinematic = false;
        transform.localScale = defaultScale;
    }

   /* void PickObjUp(GameObject playerObject)
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
    }*/
   
}

