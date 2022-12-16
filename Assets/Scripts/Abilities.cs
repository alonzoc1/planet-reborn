using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>Abilities</c> Stores a direct reference to all ability GameObjects, maintains the player's aim
/// </summary>
public class Abilities : MonoBehaviour {
    public GameObject flamethrower;
    public GameObject piercingLaser;
    public GameObject rapidFire;
    public GameObject electrorang;
    public GameObject plasmaBurst;
    public GameObject chargeField;

    public PlayerMovement playerMovement;

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
            PlayerAbilities.AllAbilities.PiercingLaser => piercingLaser,
            PlayerAbilities.AllAbilities.RapidFire => rapidFire,
            PlayerAbilities.AllAbilities.Electrorang => electrorang,
            PlayerAbilities.AllAbilities.PlasmaBurst => plasmaBurst,
            PlayerAbilities.AllAbilities.ChargeField => chargeField,
            _ => null
        };
    }

    public void ApplyDamageMod(float damageMod) {
        var tools = flamethrower.GetComponent<AbilityTools>();
        tools.damage = (int)Math.Round(tools.damage * damageMod);
        tools = piercingLaser.GetComponent<AbilityTools>();
        tools.damage = (int)Math.Round(tools.damage * damageMod);
        tools = rapidFire.GetComponent<AbilityTools>();
        tools.damage = (int)Math.Round(tools.damage * damageMod);
        tools = electrorang.GetComponent<AbilityTools>();
        tools.damage = (int)Math.Round(tools.damage * damageMod);
        tools = plasmaBurst.GetComponent<AbilityTools>();
        tools.damage = (int)Math.Round(tools.damage * damageMod);
        tools = chargeField.GetComponent<AbilityTools>();
        tools.damage = (int)Math.Round(tools.damage * damageMod);
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
        playerMovement.SwivelModel(aimedAt);
    }

    public Vector3 GetAim() {
        return aimedAt;
    }

    public List<GameObject> GetAimedTarget() {
        List<GameObject> result = new List<GameObject>();
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        //if (Physics.RaycastAll(ray, out RaycastHit[] hit, float.MaxValue, ~0, QueryTriggerInteraction.Ignore)) {
        RaycastHit[] hits = Physics.RaycastAll(ray, 10000f, ~0, QueryTriggerInteraction.Ignore);
        foreach (RaycastHit hit in hits) {
            result.Add(hit.collider.gameObject);
        }
        return result;
    }
}
