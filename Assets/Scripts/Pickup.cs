using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    GameObject playerObj;
    // Start is called before the first frame update
    void Start()
    {
        playerObj=GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
            PickObjUp();
        if (Input.GetKey(KeyCode.E))
            UnPickObjUp();
    }
    void PickObjUp()
    {
        transform.SetParent(playerObj.transform);
        transform.localPosition=new Vector2(1.5f,-1.0f);
    }
    void UnPickObjUp()
    {
        transform.parent=null;
    }
}

