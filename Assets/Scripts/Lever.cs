using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lever : MonoBehaviour
{
    [SerializeField] private Sprite offSprite;
    [SerializeField] private Sprite onSprite;
    private SpriteRenderer spriteRenderer;
    private ButtonActivator buttonActivator;
    private bool flipHappened = false;
    private bool isFlipped = false;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buttonActivator = GetComponentInParent<ButtonActivator>();
    }

    public void LeverFlipHandler(InputAction input)
    {
        if (input.triggered&&!flipHappened)
        {
            FlipLever();
            flipHappened = true;
        }
        else if (input.WasCompletedThisFrame())
            flipHappened = false;
    }
    
    void FlipLever()
    {
        if (!isFlipped)
        {
            buttonActivator.ButtonTracker(true, gameObject);
            spriteRenderer.sprite = onSprite;
            isFlipped = true;
        }
        else if (isFlipped)
        {
            buttonActivator.ButtonTracker(false, gameObject);
            spriteRenderer.sprite = offSprite;
            isFlipped = false;
        }
    }
}
