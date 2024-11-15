using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderDisabler : MonoBehaviour
{
    [SerializeField] Collider2D objectToDisable;

    private Collider2D objectCollider;
    
    void OnTriggerStay2D(Collider2D other)
    {
        Physics2D.IgnoreCollision(objectToDisable, other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Physics2D.IgnoreCollision(objectToDisable, other, false);
    }
}
