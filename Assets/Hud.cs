using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hud : MonoBehaviour {

    public Text location_text;
    public Text owner_text;
    public Text type_text;
    public Text remaining_text;
    public Text defense_text;
    public Text info_text;
    public Text option_0_text;
    public Text option_1_text;
    public Text option_2_text;

    public Button option_0;
    public Button option_1;
    public Button option_2;
    public Button end_turn;
    

    // Use this for initialization
    void Start () {
        end_turn.onClick.AddListener(() => play_data.instance.next_turn());
        option_0.onClick.AddListener(() => play_data.instance.option_0());
        option_1.onClick.AddListener(() => play_data.instance.option_1());
        option_2.onClick.AddListener(() => play_data.instance.option_2());
    }
	
	// Update is called once per frame
	void Update () {
        location_text.text = "Row: " + play_data.instance.current_select_row.ToString() + "      Column: " + play_data.instance.current_select_col.ToString();
        if (play_data.instance.owner[play_data.instance.current_select_row, play_data.instance.current_select_col]==-1)
        {
            owner_text.text = "Owners: None";
        }
        else
        {
            owner_text.text = "Owners: Player_" + play_data.instance.owner[play_data.instance.current_select_row, play_data.instance.current_select_col].ToString();
        } 
        type_text.text = "Type: " + play_data.instance.tile_type[play_data.instance.current_select_row, play_data.instance.current_select_col].ToString();
        remaining_text.text = "Remaining: " + play_data.instance.remaining[play_data.instance.current_select_row, play_data.instance.current_select_col].ToString();
        if (play_data.instance.defense_type[play_data.instance.current_select_row, play_data.instance.current_select_col] == type.Empty)
        {
            defense_text.text = "Defense: None";
        }
        else
        {
            defense_text.text = "Defense: " + play_data.instance.defense[play_data.instance.current_select_row, play_data.instance.current_select_col].ToString()
                          + "     type: " + play_data.instance.defense_type[play_data.instance.current_select_row, play_data.instance.current_select_col].ToString();
        }

        info_text.fontSize = 8;
        info_text.text = "Player_" + play_data.instance.whosturn.ToString() + " 's turn"
                        + "\nFire: " + play_data.instance.player_resource[play_data.instance.whosturn, 0].ToString()+"/"+ play_data.instance.player_income[play_data.instance.whosturn, 0].ToString()
                        + "\nWater: " + play_data.instance.player_resource[play_data.instance.whosturn, 1].ToString() + "/" + play_data.instance.player_income[play_data.instance.whosturn, 1].ToString()
                        + "\nEarth: " + play_data.instance.player_resource[play_data.instance.whosturn, 2].ToString() + "/" + play_data.instance.player_income[play_data.instance.whosturn, 2].ToString();

        //need path_find function to determine if it's accessible
        if (play_data.instance.owner[play_data.instance.current_select_row, play_data.instance.current_select_col] == -1) //if the tile has no owner
        {
            if (play_data.instance.tile_type[play_data.instance.current_select_row, play_data.instance.current_select_col]==type.Empty)
            {
                option_0_text.text = "Claim";
                option_1_text.text = "";
                option_2_text.text = "";
                option_0.interactable = true;
                option_1.interactable = false;
                option_2.interactable = false;
            }
            else
            {
                option_0_text.text = "Claim one-time";
                option_1_text.text = "Claim long-term";
                option_2_text.text = "";
                option_0.interactable = true;
                option_1.interactable = true;
                option_2.interactable = false;
            }
            
        }
        else if (play_data.instance.owner[play_data.instance.current_select_row, play_data.instance.current_select_col] == play_data.instance.whosturn)// Defense (tile's owner = player of current turn)
        {
            option_0_text.text = "Fire Defend";
            option_1_text.text = "Water Defend";
            option_2_text.text = "Earth Defend";
            switch(play_data.instance.defense_type[play_data.instance.current_select_row, play_data.instance.current_select_col])
            {
                case type.Fire:
                    option_0.interactable = true;
                    option_1.interactable = false;
                    option_2.interactable = false;
                    break;
                case type.Water:
                    option_0.interactable = false;
                    option_1.interactable = true;
                    option_2.interactable = false;
                    break;
                case type.Earth:
                    option_0.interactable = false;
                    option_1.interactable = false;
                    option_2.interactable = true;
                    break;
                case type.Empty:
                    option_0.interactable = true;
                    option_1.interactable = true;
                    option_2.interactable = true;
                    break;
            }
        }
        else //Attack(tile's owner != player of current turn)
        {
            option_0_text.text = "Fire Attack";
            option_1_text.text = "Water Attack";
            option_2_text.text = "Earth Attack";
            option_0.interactable = true;
            option_1.interactable = true;
            option_2.interactable = true;
        }
    }
}
