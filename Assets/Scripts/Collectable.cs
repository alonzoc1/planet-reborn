using UnityEngine;


[CreateAssetMenu(fileName = "Collectable", menuName = "Collectable")]
public class Collectable : ScriptableObject {

    public new string name = "Collectable";
    public Sprite icon = null;

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
	
}