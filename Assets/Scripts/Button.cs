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
    private int itemCount = 0;
    
    void Start()
    {
        buttonActivator = GetComponentInParent<ButtonActivator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = unpressedSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger==false)
        {
            itemCount++;
            spriteRenderer.sprite = pressedSprite;
            buttonActivator.ButtonTracker(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger==false)
        {
            itemCount--;
            if (itemCount == 0)
            {
                spriteRenderer.sprite = unpressedSprite;
                buttonActivator.ButtonTracker(false);
            }
        }
    }
}
