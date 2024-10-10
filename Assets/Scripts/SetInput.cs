using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SetInput : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActionAsset;
    void Start()
    {
        inputActionAsset.Enable();
    }
    
}
