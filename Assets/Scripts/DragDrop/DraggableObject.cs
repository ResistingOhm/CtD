using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Vector3 defaultPos;
    LayerMask defaultLayer;
    LayerMask draggingLayer;
    Vector3 dragScale = new Vector3(1.3f, 1.3f, 1.3f);
    Vector3 defaultScale = new Vector3(1.0f, 1.0f, 1.0f);

    public event Action<GameObject> dropAction;

    void Start()
    {
        draggingLayer = LayerMask.NameToLayer("Moving");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        defaultPos = this.transform.position;
        defaultLayer = this.gameObject.layer;

        transform.localScale = dragScale;
        this.gameObject.layer = draggingLayer;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = (Vector2) Camera.main.ScreenToWorldPoint(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localScale = defaultScale;
        this.gameObject.layer = defaultLayer;
    }

    public void CanDeploy(GameObject g)
    {
        dropAction?.Invoke(g);
    }

    public void RollBackPosition()
    {
        this.transform.position = defaultPos;
    }

}
