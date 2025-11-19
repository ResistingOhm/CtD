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

    public void InitialSetting(ItemData i, DroppableTile tile)
    {
        itemData = i;
        currentTile = tile;
        if (currentTile != null)
        {
            currentTile.NowFilled(draggableObject);
            this.transform.position = currentTile.transform.position;
        }
    }

    public void DeleteAll()
    {
        itemData = null;
        currentTile.NowEmpty();
        currentTile = null;

        this.gameObject.SetActive(false);
    }

    public void AfterDrop(GameObject g)
    {

        if (g.CompareTag("Ally"))
        {
            if (g.GetComponent<Unit>().AddItem(this.itemData))
            {
                DeleteAll();
                return;
            } else
            {
                draggableObject.wasDroppedOnValidSlot = false;
                return;
            }
        }

        if (currentTile != null && currentTile.objectNow == draggableObject)
        {
            currentTile.NowEmpty();
            currentTile = null;
        }

        if (g.CompareTag("Inventory"))
        {
            this.transform.position = g.transform.position;
            currentTile = g.GetComponent<DroppableTile>();
            currentTile.NowFilled(draggableObject);
        }
    }

    public void AfterChange(DroppableTile droppedTile, DroppableTile previousTile)
    {
        droppedTile.objectNow.GetComponent<Item>().AfterDrop(previousTile.gameObject);
        AfterDrop(droppedTile.gameObject);
    }
}
