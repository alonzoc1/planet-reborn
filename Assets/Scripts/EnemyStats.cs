using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : BaseStats {
    public GameObject healthBarPrefab;
    public Vector3 healthBarPosOffset;
    public float healthBarScale;
    public Color healthBarColor;
    public float attackSpeed = 0.3f;
    public float sightRange = 10.0f;
    public float attackRange = 5.0f;

    private HealthBarUI healthBarUI;

    private void Start() {
        healthBarUI = Instantiate(healthBarPrefab, gameObject.transform).GetComponent<HealthBarUI>();
        healthBarUI.Enable(gameObject, healthBarScale, healthBarColor, healthBarPosOffset);
        currentHealth = maxHealth;
    }

    public void Cleanup() {
        Destroy(healthBarUI.gameObject);
    }
    
    public override State ChangeCurrentHealth(int modifyBy) {
        // Update current health, and if we died on this instance of damage, report it to the event manager
        if ((state != base.ChangeCurrentHealth(modifyBy)) && (state == State.Dead))
            EventManager.ReportEnemyDeath();
        healthBarUI.SetValue((float) currentHealth /  maxHealth);
        return state;
    }
}
