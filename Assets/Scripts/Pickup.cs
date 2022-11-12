using UnityEngine;
using UnityEngine.Serialization;

public class Pickup : MonoBehaviour {

    public Collectable collectable;

    public void OnTriggerEnter()
    {
        PickUp();
    }

    protected void PickUp()
    {
        bool wasPickedUp = Inventory.instance.Add(collectable);

        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
            
    }

}