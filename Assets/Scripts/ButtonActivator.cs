using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonActivator : MonoBehaviour
{
    [SerializeField] GameObject[] affectedObjects;
    private IActivator activator;
    private int buttonsPressed = 0;

    public void ButtonTracker(bool value)
    {
        if (value)
            buttonsPressed++;
        if (!value)
            buttonsPressed--;
        if (buttonsPressed == 0)
            ObjectStateChanger(false);
        if (buttonsPressed >= 1)
            ObjectStateChanger(true);
    }
    
    void ObjectStateChanger(bool state)
    {
        foreach (GameObject obj in affectedObjects)
        {
            activator = obj.GetComponent<IActivator>();
            if (activator != null&&state)
                activator.Active = true; 
            if (activator != null&&!state)
                activator.Active = false;
        }
    }
}
