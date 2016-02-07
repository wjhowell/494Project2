using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI : MonoBehaviour {
    public Text playerName;
    public Text tilesOwned;
    public Text fireOwned;
    public Text grassOwned;
    public Text waterOwned;
    // Use this for initialization
    void Start () {
        playerName = GameObject.Find("Playername").GetComponent<Text>();
        tilesOwned = GameObject.Find("NumTilesOwned").GetComponent<Text>();
        fireOwned = GameObject.Find("FireOwned").GetComponent<Text>();
        grassOwned = GameObject.Find("GrassOwned").GetComponent<Text>();
        waterOwned = GameObject.Find("WaterOwned").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
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
