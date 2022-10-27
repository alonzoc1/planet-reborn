using UnityEngine;
using UnityEngine.UI;

/* Sits on all InventorySlots. */

public class InventorySlot : MonoBehaviour {

    public Image icon;

    Collectable _collectable;

    public void AddItem(Collectable collectable)
    {
        _collectable = collectable;

        icon.sprite = _collectable.icon;
        icon.enabled = true;
    }

    // Clear the slot
    public void ClearSlot()
    {
        _collectable = null;

        icon.sprite = null;
        icon.enabled = false;
    }

    public void onRemove()
    {
        Inventory.instance.Remove(_collectable);
    }

}