using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    public DroppableTile currentTile;
    private DraggableObject draggableObject;

    private void Awake()
    {
        draggableObject = GetComponent<DraggableObject>();
        draggableObject.dropAction += AfterDrop;
        draggableObject.changeAction += AfterChange;
    }

    public void AfterDrop(GameObject g)
    {
        if (g.CompareTag("Ally"))
        {
            if (g.GetComponent<Unit>().AddItem(this))
            {
                this.gameObject.SetActive(false);
            }
        }
        else if (g.CompareTag("Inventory"))
        {
            this.transform.position = g.transform.position;

        }
    }

    public void AfterChange(GameObject droppedTile, GameObject previousTile)
    {
        AfterDrop(droppedTile);
        droppedTile.GetComponent<Item>().AfterDrop(previousTile);
    }
}
