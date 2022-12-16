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
        { "shop", "Buy upgrades!"}
    };

    private const string BaseMessage = "Objective: ";

    public void SetMessage(string messageKey) {
        if (messageKey == "hide")
            trackerText.text = "";
        else if (objectiveMessages.ContainsKey(messageKey))
            trackerText.text = BaseMessage + objectiveMessages[messageKey];
        else
            Debug.Log("Could not find Objective message key: " + messageKey);
    }
}
