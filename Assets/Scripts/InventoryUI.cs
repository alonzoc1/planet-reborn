using UnityEngine;
using UnityEngine.Serialization;


public class InventoryUI : MonoBehaviour {

    public Transform collectablesParent;
    public GameObject inventoryUI;

    Inventory inventory;

    InventorySlot[] slots;

    private void Start() 
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
        slots = collectablesParent.GetComponentsInChildren<InventorySlot>();
    }
	
    private void Update() 
    {
        if ((Input.GetKeyDown(KeyCode.Tab) && Time.timeScale != 0f) || (Input.GetKeyDown(KeyCode.Tab) && inventoryUI.activeSelf))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.Collectables.Count)
            {
                slots[i].AddItem(inventory.Collectables[i]);
            } else
            {
                slots[i].ClearSlot();
            }
        }
    }
}    