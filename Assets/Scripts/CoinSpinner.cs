using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpinner : MonoBehaviour {
    public float speed;
    public Vector3 rotate;
    private void Update() {
        gameObject.transform.Rotate(rotate * (speed * Time.deltaTime));
    }
}
