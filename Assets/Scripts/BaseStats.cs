using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <c>BaseStats</c> Provides base class for all character GameObjects and contains commonly used stats
/// </summary>
public class BaseStats : MonoBehaviour {
    // State enum
    public enum State {
        Normal,
        Stunned,
        Dead
    }
    
    public float movementSpeed = 5.0f;
    public int health = 100;
    public float jumpPower = 5.0f;
    public float gravity = 9.81f;
    public State state = State.Normal;
}
