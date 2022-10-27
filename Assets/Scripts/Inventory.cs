using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public delegate void OnItemChanged();

    public int space = 20;

    public List<Collectable> Collectables = new();
    public OnItemChanged onItemChangedCallback;

    public static Inventory instance;
    private void Awake()
    {
        instance = this;
    }
    public bool Add(Collectable collectable)
    {
        if (Collectables.Count >= space)
        {
            return false;
        }

        Collectables.Add(collectable);
        onItemChangedCallback?.Invoke();

        return true;
    }

    public void Remove(Collectable collectable)
    {
        Collectables.Remove(collectable);

        onItemChangedCallback?.Invoke();
    }
}