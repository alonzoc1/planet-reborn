using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AbilityCooldowns : MonoBehaviour {
    public enum AbilitySlots {
        LeftSlot,
        RightSlot
    }
    
    public Image ability1Icon;
    public Image ability2Icon;
    public Image ability1Cooldown;
    public Image ability2Cooldown;
    private float ability1CooldownMax;
    private float ability2CooldownMax;
    [SerializeField] private float ability1CooldownValue;
    [SerializeField] private float ability2CooldownValue;
    [SerializeField] private bool ability1Ready;
    [SerializeField] private bool ability2Ready;
    private bool showAbility1CD;
    private bool showAbility2CD;
    
    private void Start() {
        ability1CooldownValue = 0f;
        ability2CooldownValue = 0f;
        ability1Ready = true;
        ability2Ready = true;
        // Dynamically load ability icons
        PlayerAbilities playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();
        SetAbilityIcons(playerAbilities);
        playerAbilities.AbilityCooldownsReady(); // Run this after all other loading done
    }

    private void Update() {
        if (!ability1Ready) {
            ability1CooldownValue -= Time.deltaTime;
            if (ability1CooldownValue <= 0f)
                ability1Ready = true;
            if (showAbility1CD)
                SetUIFill(AbilitySlots.LeftSlot);
        }
        if (!ability2Ready) {
            ability2CooldownValue -= Time.deltaTime;
            if (ability2CooldownValue <= 0f)
                ability2Ready = true;
            if (showAbility2CD)
                SetUIFill(AbilitySlots.RightSlot);
        }
    }
    
    public void StartCooldown(AbilitySlots slot, float cooldown, bool showCDUI) {
        switch (slot) {
            case AbilitySlots.LeftSlot:
                ability1CooldownMax = cooldown;
                ability1CooldownValue = cooldown;
                ability1Ready = false;
                showAbility1CD = showCDUI;
                break;
            case AbilitySlots.RightSlot:
                ability2CooldownMax = cooldown;
                ability2CooldownValue = cooldown;
                ability2Ready = false;
                showAbility2CD = showCDUI;
                break;
        }
    }

    public bool GetAbilityReady(AbilitySlots slot) {
        return slot switch {
            AbilitySlots.LeftSlot => ability1Ready,
            AbilitySlots.RightSlot => ability2Ready,
            _ => false
        };
    }

    private void SetUIFill(AbilitySlots slot) {
        switch (slot) {
            case AbilitySlots.LeftSlot when ability1Ready:
                ability1Cooldown.fillAmount = 0f;
                break;
            case AbilitySlots.LeftSlot:
                ability1Cooldown.fillAmount = ability1CooldownValue / ability1CooldownMax;
                break;
            case AbilitySlots.RightSlot when ability2Ready:
                ability2Cooldown.fillAmount = 0f;
                break;
            case AbilitySlots.RightSlot:
                ability2Cooldown.fillAmount = ability2CooldownValue / ability2CooldownMax;
                break;
        }
    }

    private void SetAbilityIcons(PlayerAbilities playerAbilities) {
        ability1Icon.sprite = playerAbilities.GetIcon(AbilitySlots.LeftSlot);
        ability2Icon.sprite = playerAbilities.GetIcon(AbilitySlots.RightSlot);
    }
}
