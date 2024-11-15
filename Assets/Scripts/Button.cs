using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Button : MonoBehaviour
{
    [SerializeField] private Sprite unpressedSprite;
    [SerializeField] private Sprite pressedSprite;

    private SpriteRenderer spriteRenderer;
    private ButtonActivator buttonActivator;
    private Dictionary<GameObject,int> pressingObjects = new Dictionary<GameObject,int>();
    
    void Start()
    {
        buttonActivator = GetComponentInParent<ButtonActivator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = unpressedSprite;
    }

    //private void Update() => Debug.Log(pressingObjects.Count);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger) return;
        if (pressingObjects.TryAdd(other.gameObject, 1))
        {
            buttonActivator.ButtonTracker(true, gameObject);
            spriteRenderer.sprite = pressedSprite;
        }
        else pressingObjects[other.gameObject]++;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger) return;
        if (pressingObjects.ContainsKey(other.gameObject))
        {
            pressingObjects[other.gameObject]--;
            if (pressingObjects[other.gameObject] == 0)
                pressingObjects.Remove(other.gameObject);
        }
        if (pressingObjects.Count==0)
        {
            spriteRenderer.sprite = unpressedSprite;
            buttonActivator.ButtonTracker(false, gameObject);
        }
    }
}
