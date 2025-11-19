using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour
{
    public event Action<GameObject> dropAction;
    public event Action<DroppableTile, DroppableTile> changeAction;
    public bool wasDroppedOnValidSlot = false;

    public void CanDeploy(GameObject g)
    {
        dropAction?.Invoke(g);
    }

    public void CanChange(DroppableTile droppedTile, DroppableTile previousTile)
    {
        changeAction?.Invoke(droppedTile, previousTile);
    }

}
