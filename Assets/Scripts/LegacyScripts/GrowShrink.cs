using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowShrink : MonoBehaviour
{
    [SerializeField] private bool player2;
    [SerializeField] private float growFactor;
    [SerializeField] private float shrinkFactor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (player2)
                Shrink();
            else Grow();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (player2)
                Grow();
            else Shrink();
        }
    }

    private void Grow()
    {
        // Only change the y scale, maintain the x scale
        Vector3 newScale = new Vector3(transform.localScale.x * growFactor, transform.localScale.y * growFactor, 1);
        if (newScale.y <= 3.0f) // Set a max scale limit
        {
            transform.localScale = newScale; // Adjust scaling
        }
    }

    private void Shrink()
    {
        // Only change the y scale, maintain the x scale
        Vector3 newScale = new Vector3(transform.localScale.x * shrinkFactor, transform.localScale.y * shrinkFactor, 1);
        if (newScale.y >= 0.5f) // Set a min scale limit
        {
            transform.localScale = newScale; // Return to normal size
        }
    }
}
