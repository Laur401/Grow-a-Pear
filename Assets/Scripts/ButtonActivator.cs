using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonActivator : MonoBehaviour
{
    [SerializeField] GameObject[] affectedObjects;
    private IActivator activator;
    private List<GameObject> buttons = new List<GameObject>();

    public void ButtonTracker(bool value, GameObject button)
    {
        if (value)
            if (!buttons.Contains(button))
                buttons.Add(button);
        if (!value)
            if (buttons.Contains(button))
                buttons.Remove(button);
        if (buttons.Count == 0)
            ObjectStateChanger(false);
        if (buttons.Count >= 1)
            ObjectStateChanger(true);
        Debug.Log($"This is the number of buttons: {buttons.Count}");
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
