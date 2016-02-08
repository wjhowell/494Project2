using UnityEngine;
using System.Collections;

public class tile : MonoBehaviour {
    tile instance;
    public int row;
    public int col;
    public int owner;
    public type tile_type;
    public int remaining;

	// Use this for initialization
	void Start () {
        instance = this;
        row = (int)transform.position.y;
        col = (int)transform.position.x;
        
    }
	
	// Update is called once per frame
	void Update () {
        owner = play_data.instance.owner[row, col];
        tile_type = play_data.instance.tile_type[row, col];

        //string sprites = play_data.instance.tile_type[row, col].ToString() + "_" + play_data.instance.owner[row, col];
        //etComponent<SpriteRenderer>().sprite=play_data
        switch (tile_type)
        {
            case type.Empty:
                #region empty tile
                if (owner==-1)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.empty[4];
                else if (owner == 0)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.empty[0];
                else if (owner == 1)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.empty[1];
                else if (owner == 2)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.empty[2];
                else if (owner == 3)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.empty[3];
                #endregion
                break;
            case type.Fire:
                #region fire tile
                if (owner == -1)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.fire[4];
                else if (owner == 0)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.fire[0];
                else if (owner == 1)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.fire[1];
                else if (owner == 2)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.fire[2];
                else if (owner == 3)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.fire[3];
                #endregion
                break;
            case type.Water:
                #region water tile
                if (owner == -1)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.water[4];
                else if (owner == 0)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.water[0];
                else if (owner == 1)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.water[1];
                else if (owner == 2)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.water[2];
                else if (owner == 3)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.water[3];
                #endregion
                break;
            case type.Earth:
                #region earth tile
                if (owner == -1)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.earth[4];
                else if (owner == 0)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.earth[0];
                else if (owner == 1)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.earth[1];
                else if (owner == 2)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.earth[2];
                else if (owner == 3)
                    GetComponent<SpriteRenderer>().sprite = play_data.instance.earth[3];
                #endregion
                break;
        }
    }

    void OnMouseDown()
    {
        play_data.instance.current_select_row = row;
        play_data.instance.current_select_col = col;
    }
}
