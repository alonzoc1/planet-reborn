using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveTrackerUI : MonoBehaviour {
    public TextMeshProUGUI trackerText;

    private Dictionary<string, string> objectiveMessages = new Dictionary<string, string>() {
        { "exit", "Get to the exit" },
        { "defeatAll", "Defeat all enemies"},
        { "shop", "Buy upgrades!"},
        { "activate", "Activate the teleporter (press 'E' near control panel)"},
        { "defendTeleporter", "Stay near the teleporter for {{NUMBER}} seconds" }
    };

    private const string BaseMessage = "Objective: ";

    public void SetMessage(string messageKey, int seconds=0) {
        if (messageKey == "hide")
            trackerText.text = "";
        else if (messageKey == "defendTeleporter") {
            string text = objectiveMessages[messageKey].Replace("{{NUMBER}}", seconds.ToString());
            trackerText.text = text;
        }
        else if (objectiveMessages.ContainsKey(messageKey))
            trackerText.text = BaseMessage + objectiveMessages[messageKey];
        else
            Debug.Log("Could not find Objective message key: " + messageKey);
    }
}
