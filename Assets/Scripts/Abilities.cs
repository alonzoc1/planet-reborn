using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Abilities</c> Stores a direct reference to all ability GameObjects, maintains the player's aim
/// </summary>
public class Abilities : MonoBehaviour {
    public GameObject flamethrower;
    public GameObject sniperShot;

    private const float DefaultAimDistance = 25;
    private Camera mainCamera;
    [SerializeField] private Vector3 aimedAt;

    private void Start() {
        mainCamera = Camera.main;
    }

    public GameObject GetAbilityGameObject(PlayerAbilities.AllAbilities ability) {
        // This is faster than doing a GameObject.Find, but requires a little more maintenance
        return ability switch {
            PlayerAbilities.AllAbilities.Flamethrower => flamethrower,
            PlayerAbilities.AllAbilities.SniperShot => sniperShot,
            _ => null
        };
    }

    private void Update() {
        // Check what the camera crosshair is aiming at
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, ~0, QueryTriggerInteraction.Ignore))
            // Aim at what the crosshair is pointed at
            aimedAt = hit.point;
        else
            // If aimed at nothing, aim some units away from the camera (hopefully this never happens since we have invisible ceilings/walls)
            aimedAt = ray.GetPoint(DefaultAimDistance);
    }

    public Vector3 GetAim() {
        return aimedAt;
    }
}
