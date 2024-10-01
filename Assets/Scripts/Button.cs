using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Button : MonoBehaviour
{
    [SerializeField] private Sprite unpressedSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] GameObject[] affectedObjects;
    private SpriteRenderer spriteRenderer;
    private int itemCount = 0;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = unpressedSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            itemCount++;
            spriteRenderer.sprite = pressedSprite;
            ObjectStateChanger(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            itemCount--;
            if (itemCount == 0)
            {
                spriteRenderer.sprite = unpressedSprite;
                ObjectStateChanger(false);
            }
        }
    }

    void ObjectStateChanger(bool state)
    {
            foreach (GameObject obj in affectedObjects)
            {
                IActivator activator = obj.GetComponent<IActivator>();
                if (activator != null&&state)
                    activator.Active = true;
                if (activator != null&&!state)
                    activator.Active = false;
            }
    }
}
