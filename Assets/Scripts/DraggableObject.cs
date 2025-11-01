using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    Vector3 defaultPos;
    Vector3 dragScale = new Vector3(1.3f, 1.3f, 1.3f);
    Vector3 defaultScale = new Vector3(1.0f, 1.0f, 1.0f);

    public void OnBeginDrag(PointerEventData eventData)
    {
        defaultPos = this.transform.position;
        transform.localScale = dragScale;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localScale = defaultScale;
    }

}
