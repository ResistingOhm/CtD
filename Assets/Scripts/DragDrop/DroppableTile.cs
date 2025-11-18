using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (objectNow == null) return;
        defaultPos = this.transform.position;
        objectNow.wasDroppedOnValidSlot = false;

        objectNow.transform.localScale = dragScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (objectNow == null) return;
        objectNow.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (objectNow == null) return;
        if (!objectNow.wasDroppedOnValidSlot)
        {
            objectNow.transform.localScale = defaultScale;
            objectNow.transform.position = defaultPos;
            return;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        var g = eventData.pointerDrag.GetComponent<DroppableTile>().objectNow;
        if (g == null) return;

        var gt = g.tag;

        g.transform.localScale = defaultScale;

        if (gt.Equals("Item") && objectNow != null && objectNow.tag.Equals("Ally"))
        {
            g.CanDeploy(objectNow.gameObject);
            return;
        }

        if (CanAccept(gt))
        {
            g.wasDroppedOnValidSlot = true;
            g.CanDeploy(this.gameObject);
            return;
        }

        if (CanChange(gt))
        {
            g.wasDroppedOnValidSlot = true;
            g.CanChange(this.gameObject, g.gameObject);
            return;
        }
    }
}
