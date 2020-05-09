

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;
using CodeMonkey;

public class StatsHandler : MonoBehaviour {
    private static StatsHandler _instance;
    public static StatsHandler Instance { get { return _instance; } }

    private static string _cyclesText = "Cycles: ";
    private static string _walkedText = "Tiles Walked: ";

    private int cycles = 0;
    private int walked = 0;

    private Text cycleCounter;
    private Text walkedCounter;
    private TextMeshProUGUI logs;
    private ScrollRect scrollRect;

    private void Awake() {
        if (_instance != null && _instance != this) {
           Destroy(this.gameObject);
        } else {
            _instance = this;
        }
        cycleCounter = GameObject.Find("Text_Cycles").GetComponent<Text>();
        walkedCounter = GameObject.Find("Text_Walked").GetComponent<Text>();
        logs = GameObject.Find("Text_Logs").GetComponent<TextMeshProUGUI>();
        scrollRect = GameObject.Find("Scroll_Logs").GetComponent<ScrollRect>();
        
        // Setup Logging
        logs.text = " > Logger Awake";
        log("StatsHandler Awake");
    }

    public void Update() {
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    public void updateCycles() {
        this.cycles++;
        cycleCounter.text = _cyclesText + this.cycles.ToString();
    }

    public void log(string text) {
        logs.text = logs.text + "\n [ Cycle " + this.cycles.ToString() + " ] " + text;
    }

    public void updateTilesWalked(Grid<PathNode> grid) {
        this.walked++;
        walkedCounter.text = _walkedText + this.walked.ToString() + "/" + grid.getSize().ToString();
    }
}
