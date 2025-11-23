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
    int defaultLayer;
    int dragLayer = 11;
    Vector3 dragScale = new Vector3(1.3f, 1.3f, 1.3f);
    Vector3 defaultScale = new Vector3(1.0f, 1.0f, 1.0f);

    public event Action filledAction;
    public event Action emptyAction;

    public DraggableObject objectNow;

    public bool canDrag = true;
    public bool canAccept = true;
    public bool canChange = true;

    public void SetTag(string tag)
    {
        acceptableTag = tag;
    }

    public void SetDrag(bool t)
    {
        canDrag = t;
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
        if (!canDrag) return;
        if (objectNow == null) return;
        defaultPos = this.transform.position;
        defaultLayer = objectNow.gameObject.layer;
        objectNow.wasDroppedOnValidSlot = false;

        objectNow.transform.localScale = dragScale;
        objectNow.gameObject.layer = dragLayer;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;
        if (objectNow == null) return;
        objectNow.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag) return;
        if (objectNow == null) return;

        if (!objectNow.wasDroppedOnValidSlot)
        {
            objectNow.gameObject.layer = defaultLayer;
            objectNow.transform.localScale = defaultScale;
            objectNow.transform.position = defaultPos;
            return;
        }
    }

    public void NowFilled(DraggableObject g)
    {
        canAccept = false;
        objectNow = g;
        filledAction?.Invoke();
    }

    public void NowEmpty()
    {
        canAccept = true;
        objectNow = null;
        emptyAction?.Invoke();
    }

    public void Refresh()
    {
        canAccept = (objectNow == null);
    }

    public void OnDrop(PointerEventData eventData)
    {
        var t = eventData.pointerDrag.GetComponent<DroppableTile>();
        if (!t.canDrag) return;
        var g = t.objectNow;
        if (g == null) return;

        var gt = g.tag;

        g.transform.localScale = defaultScale;
        g.gameObject.layer = defaultLayer;

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
            g.CanChange(this, t);
            return;
        }
    }
}
