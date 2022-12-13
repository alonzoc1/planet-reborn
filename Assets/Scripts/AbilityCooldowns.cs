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
    private bool ability1Manual;
    private bool ability2Manual;
    private bool showAbility1CD;
    private bool showAbility2CD;
    
    private void Start() {
        ability1CooldownValue = 0f;
        ability2CooldownValue = 0f;
        ability1Ready = true;
        ability2Ready = true;
        ability1Manual = false;
        ability2Manual = false;
        // Dynamically load ability icons
        PlayerAbilities playerAbilities = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>();
        SetAbilityIcons(playerAbilities);
        playerAbilities.AbilityCooldownsReady(); // Run this after all other loading done
    }

    private void Update() {
        if (!ability1Ready) {
            if (!ability1Manual) {
                ability1CooldownValue -= Time.deltaTime;
                if (ability1CooldownValue <= 0f)
                    ability1Ready = true;
            }
            if (showAbility1CD)
                SetUIFill(AbilitySlots.LeftSlot, false);
        }
        if (!ability2Ready) {
            if (!ability2Manual) {
                ability2CooldownValue -= Time.deltaTime;
                if (ability2CooldownValue <= 0f)
                    ability2Ready = true;
            }
            if (showAbility2CD)
                SetUIFill(AbilitySlots.RightSlot, false);
        }
    }
    
    public void StartCooldown(AbilitySlots slot, float cooldown, bool showCDUI, bool manualControl) {
        switch (slot) {
            case AbilitySlots.LeftSlot:
                ability1CooldownMax = cooldown;
                ability1CooldownValue = cooldown;
                ability1Ready = false;
                showAbility1CD = showCDUI;
                ability1Manual = manualControl;
                break;
            case AbilitySlots.RightSlot:
                ability2CooldownMax = cooldown;
                ability2CooldownValue = cooldown;
                ability2Ready = false;
                showAbility2CD = showCDUI;
                ability2Manual = manualControl;
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

    public void ManualReadyAbility(AbilitySlots slot) {
        if (slot == AbilitySlots.LeftSlot) {
            ability1Ready = true;
            ability1CooldownValue = 0f;
            ability1CooldownMax = 1f;
        }
        else if (slot == AbilitySlots.RightSlot) {
            ability2Ready = true;
            ability2CooldownValue = 0f;
            ability1CooldownMax = 1f;
        }
        SetUIFill(slot, false);
    }

    /*
    // Saving this in case we need something like it later, but for now don't use this since it looks dumb
    public void PulseAbilityUse(AbilitySlots slot) {
        if (slot == AbilitySlots.LeftSlot) {
            StartCoroutine(PulseAbilityUI(slot, .1f));
        } else if (slot == AbilitySlots.RightSlot) {
            StartCoroutine(PulseAbilityUI(slot, .1f));
        }
    }

    private IEnumerator PulseAbilityUI(AbilitySlots slot, float pulseTime) {
        SetUIFill(slot, true);
        yield return new WaitForSeconds(pulseTime);
        SetUIFill(slot, false);
    }
    */

    private void SetUIFill(AbilitySlots slot, bool pulse) {
        // If pulse we just want to visually shade over the icon for a bit, not actually show the correct CD value
        if (pulse) { // sorry this is lazy and could be cleaner :) - Alonzo
            switch (slot) {
                case AbilitySlots.LeftSlot:
                    ability1Cooldown.fillAmount = 1f;
                    break;
                case AbilitySlots.RightSlot:
                    ability2Cooldown.fillAmount = 1f;
                    break;
            }
        }
        else {
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
    }

    private void SetAbilityIcons(PlayerAbilities playerAbilities) {
        ability1Icon.sprite = playerAbilities.GetIcon(AbilitySlots.LeftSlot);
        ability2Icon.sprite = playerAbilities.GetIcon(AbilitySlots.RightSlot);
    }
}
