using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {
    public Image fill;

    private GameObject character;
    private Camera mainCamera;
    private Vector3 posOffset;

    private void Start() {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    public void Enable(GameObject target, float scale, Color color, Vector3 offset) {
        fill.color = color;
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);
        posOffset = offset;
        character = target;
        gameObject.transform.Translate(posOffset);
    }

    private void LateUpdate() {
        if (!character)
            return;
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }

    public void SetValue(float value) {
        fill.fillAmount = value;
    }
}
