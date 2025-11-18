using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour
{
    public event Action<GameObject> dropAction;
    public event Action<GameObject, GameObject> changeAction;
    public bool wasDroppedOnValidSlot = false;

    public void CanDeploy(GameObject g)
    {
        dropAction?.Invoke(g);
    }

    public void CanChange(GameObject droppable, GameObject previous)
    {
        changeAction?.Invoke(droppable, previous);
    }

}
