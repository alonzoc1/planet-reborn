using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthPickup : Pickup {

    public GameObject player;
    public PlayerStats playerStats;

    // Will upgrade the player's health by 25 when collided with.
    public new void OnTriggerEnter() //Overrides previous OnTriggerEnter
    {
        //Comment below lines if we choose assignment in script over assigning in Unity
        player = GameObject.Find("Player");
        playerStats = player.GetComponent<PlayerStats>();
        playerStats.ChangeMaxHealth(25);
        playerStats.ChangeCurrentHealth(25);
        PickUp();
    }
}
