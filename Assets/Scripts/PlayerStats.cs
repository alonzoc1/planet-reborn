using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int health = 100; // Same logic here: since items will affect this, keep it public so everyone and everything can modify it to some degree.
    public Boolean isDead = false; // For testing reasons, I am keeping this modifer public.
    public float movementSpeed = 6.0f; // Adjust the speed as needed. Since this will be affected by items, let's keep it public.
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
