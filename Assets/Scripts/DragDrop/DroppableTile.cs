using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditorInternal.ReorderableList;

public class DroppableTile : MonoBehaviour, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField]
    private string acceptableTag;

    Vector3 defaultPos;
    Vector3 dragScale = new Vector3(1.3f, 1.3f, 1.3f);
    Vector3 defaultScale = new Vector3(1.0f, 1.0f, 1.0f);

    public event Action<GameObject> dropAction;
    public event Action<GameObject, GameObject> changeAction;

    public DraggableObject objectNow;

    public bool canAccept = true;
    public bool canChange = true;

    public void SetTag(string tag)
    {
        acceptableTag = tag;
    }

    public bool CanAccept(string tag)
    {
        return acceptableTag.Equals(tag) && canAccept;
    }

    public bool CanChange(string tag)
    {
        return acceptableTag.Equals(tag) && canChange;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        defaultPos = this.transform.position;
        objectNow.wasDroppedOnValidSlot = false;

        objectNow.transform.localScale = dragScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        objectNow.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        objectNow.transform.localScale = defaultScale;

        if (!objectNow.wasDroppedOnValidSlot)
        {
            objectNow.transform.position = defaultPos;
            return;
        }


    }

    public void OnDrop(PointerEventData eventData)
    {
        var g = eventData.pointerDrag.GetComponent<DroppableTile>().objectNow.GetComponent<DraggableObject>();
        if (g == null) return;

        if (CanAccept(g.tag))
        {
            g.wasDroppedOnValidSlot = true;
            g.CanDeploy(this.gameObject);
            return;
        }

        if (CanChange(g.tag))
        {
            g.wasDroppedOnValidSlot = true;
            g.CanChange(this.gameObject, objectNow.gameObject);
            return;
        }
    }
}
