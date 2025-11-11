using UnityEngine;
using UnityEngine.EventSystems;

public class DroppableObject : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private string acceptableTag;

    public bool canAccept = true;

    public void SetTag(string tag)
    {
        acceptableTag = tag;
    }

    public bool CanAccept(string tag)
    {
        return acceptableTag.Equals(tag) && canAccept;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var g = eventData.pointerDrag.GetComponent<DraggableObject>();
        if (g == null) return;

        if (g.gameObject.CompareTag(acceptableTag))
        {
            g.CanDeploy(this.gameObject);
        }
        else
        {
             g.RollBackPosition();
        }
    }
}
