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
    
    private void Start() {
        ability1CooldownValue = 0f;
        ability2CooldownValue = 0f;
        ability1Ready = true;
        ability2Ready = true;
        // Dynamically load ability icons
        StartCoroutine(SetAbilityIcons(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAbilities>()));
    }

    private void Update() {
        if (!ability1Ready) {
            ability1CooldownValue -= Time.deltaTime;
            if (ability1CooldownValue <= 0f)
                ability1Ready = true;
            SetUIFill(AbilitySlots.LeftSlot);
        }
        if (!ability2Ready) {
            ability2CooldownValue -= Time.deltaTime;
            if (ability2CooldownValue <= 0f)
                ability2Ready = true;
            SetUIFill(AbilitySlots.RightSlot);
        }
    }
    
    public void StartCooldown(AbilitySlots slot, float cooldown) {
        switch (slot) {
            case AbilitySlots.LeftSlot:
                ability1CooldownMax = cooldown;
                ability1CooldownValue = cooldown;
                ability1Ready = false;
                break;
            case AbilitySlots.RightSlot:
                ability2CooldownMax = cooldown;
                ability2CooldownValue = cooldown;
                ability2Ready = false;
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

    private IEnumerator SetAbilityIcons(PlayerAbilities playerAbilities) {
        AsyncOperationHandle<Sprite> leftIconHandle = Addressables.LoadAssetAsync<Sprite>(playerAbilities.GetIconName(AbilitySlots.LeftSlot));
        AsyncOperationHandle<Sprite> rightIconHandle = Addressables.LoadAssetAsync<Sprite>(playerAbilities.GetIconName(AbilitySlots.RightSlot));
        yield return leftIconHandle;
        yield return rightIconHandle;
        if (leftIconHandle.Status == AsyncOperationStatus.Succeeded)
            ability1Icon.sprite = leftIconHandle.Result;
        if (rightIconHandle.Status == AsyncOperationStatus.Succeeded)
            ability2Icon.sprite = rightIconHandle.Result;
        Addressables.Release(leftIconHandle);
        Addressables.Release(rightIconHandle);
    }
    
}
