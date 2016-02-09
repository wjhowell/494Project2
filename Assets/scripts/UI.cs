using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI : MonoBehaviour {
	public static UI U;

    public Text playerName;
    public Text tilesOwned;
    public Text fireOwned;
    public Text grassOwned;
    public Text waterOwned;
	public Text timeLeft;
	public float timeRemaining;
    // Use this for initialization
    void Start () {
		U = this;

        playerName = GameObject.Find("Playername").GetComponent<Text>();
        tilesOwned = GameObject.Find("NumTilesOwned").GetComponent<Text>();
        fireOwned = GameObject.Find("FireOwned").GetComponent<Text>();
        grassOwned = GameObject.Find("GrassOwned").GetComponent<Text>();
        waterOwned = GameObject.Find("WaterOwned").GetComponent<Text>();
		timeLeft = GameObject.Find("TimeLeft").GetComponent<Text>();
		timeRemaining = 30f;
    }
	
	// Update is called once per frame
	void Update () {
		timeRemaining -= Time.deltaTime;
		timeLeft.text = "Time Left: " + timeRemaining.ToString("F0");
		if (timeRemaining <= 0) {
			timeRemaining = 30f;
			if (Main.M.current_player == Owner.RED) {
				Main.M.current_player = Owner.BLUE;
			} else {
				Main.M.current_player = Owner.RED;
			}
			Main.M.UpdateSelectableTiles ();
		}

        playerName.text = "Player "+Main.M.current_player.ToString();
		if (Main.M.current_player == Owner.BLUE) {
			playerName.color = Color.blue;
		} else
			playerName.color = Color.red;

        if (Main.M.current_player == Owner.RED)
        {
            tilesOwned.text = "Tiles Owned: " + Main.M.redPlayer.numTilesOwned;
            fireOwned.text = "Fire: " + Main.M.redPlayer.FireOwned;
            grassOwned.text = "Grass: " + Main.M.redPlayer.GrassOwned;
            waterOwned.text = "Water: " + Main.M.redPlayer.WaterOwned;
        }
        else {
            tilesOwned.text = "Tiles Owned: " + Main.M.bluePlayer.numTilesOwned;
            fireOwned.text = "Fire: " + Main.M.bluePlayer.FireOwned;
            grassOwned.text = "Grass: " + Main.M.bluePlayer.GrassOwned;
            waterOwned.text = "Water: " + Main.M.bluePlayer.WaterOwned;
        }
        
    }
}
