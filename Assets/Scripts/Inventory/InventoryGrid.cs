using UnityEngine;

public class InventoryGrid : MonoBehaviour
{
    void Start()
    {
        DroppableTile t = GetComponent<DroppableTile>();
        t.filledAction += OnFilled;
        t.emptyAction += OnEmpty;
    }

    public void OnFilled()
    {

    }

    public void OnEmpty()
    {

    }
}
