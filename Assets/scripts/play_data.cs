using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum type { Fire, Water, Earth, Empty };
public struct coordinate {
    public int x;
    public int y;
    public coordinate(int a, int b)
    {
        x = a;
        y = b;
    }
}

public class play_data : MonoBehaviour {
    public static play_data instance;

	public tile tilePrefab;
    //map data
    public int[,] owner = new int[3,3];
    public type[,] tile_type = new type[3, 3];
    public int[,] remaining = new int[3, 3];
    public type[,] defense_type = new type[3, 3];
    public int[,] defense = new int[3, 3];
    //select data
    public int current_select_row;
    public int current_select_col;
    //4 player data in order of Fire, Water, Earth
    public int[,] player_resource = new int[4,3];
    public int[,] player_income = new int[4, 3];
    //public List<coordinate>[] player_property = new List<coordinate>[4];
    //turn moves
    public int moves_remain;
    //map sprites
    public Sprite[] empty;
    public Sprite[] fire;
    public Sprite[] water;
    public Sprite[] earth;

    //public coordinate[] player_0;
    //public coordinate[] player_1;
    //public coordinate[] player_2;
    //public coordinate[] player_3;

    public List<coordinate>[] player_mine = new List<coordinate>[4];

    public int whosturn;


    // Use this for initialization
    void Start () {
		SetupBoard (14, 10);
        for(int p=0; p < 4; p++)
        {
            for(int q=0; q < 3; q++)
            {
                player_resource[p, q] = 0;
                player_income[p, q] = 0;
            }
        }

        for (int p = 0; p < 3; p++)
        {
            for (int q = 0; q < 3; q++)
            {
                tile_type[p, q] = type.Empty;
                owner[p, q] = -1;
                defense[p, q] = 0;
                defense_type[p, q] =type.Empty;
            }
        }



        moves_remain = 2;
        whosturn = 0;
        instance = this;
        owner[1,0] = 0;
        owner[1,2] = 1;
        owner[0,1] = 2;
        owner[2,1] = 3;
        tile_type[1, 0] = type.Fire;
        tile_type[1, 1] = type.Water;
        tile_type[1, 2] = type.Earth;
        tile_type[0, 1] = type.Empty;
        tile_type[2, 1] = type.Empty;
        for (int p = 0; p < 3; p++)
        {
            for (int q = 0; q < 3; q++)
            {
                if (tile_type[p, q]!=type.Empty)
                {
                    remaining[p, q] = 10;
                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        //print(owner[0,1]);
	}

	void SetupBoard(int cols, int rows) {
		owner        = new int [cols, rows];
		tile_type    = new type[cols, rows];
		remaining    = new int [cols, rows];
		defense_type = new type[cols, rows];
		defense      = new int [cols, rows];
		for (int x = 0; x != cols; ++x) {
			for (int y = 0; y != rows; ++y) {
				owner [x, y] = -1;
				defense_type [x, y] = tile_type [x, y] = type.Empty;
				remaining [x, y] = 0;
				defense [x, y] = 0;
				tile nt = Instantiate<tile>(tilePrefab);
				nt.GetComponent<Transform> ().position = new Vector3 ((float)x, (float)y, 0f);
				nt.col = x;
				nt.row = y;
				nt.owner = -1;
			}
		}
	}

    public void next_turn()
    {
        whosturn++;
        moves_remain = 2;
        if (whosturn == 4)
        {
            whosturn = 0;
        }

        for (int p = 0; p < 3; p++)
        {
                player_resource[whosturn, p] += player_income[whosturn, p];
        }
        for (int p = 0; p < 3; p++)
        {
            for (int q = 0; q < 3; q++)
            {
                if (owner[p, q] == whosturn && tile_type[p,q]!=type.Empty)
                {
                    remaining[p, q] --;
                    if (remaining[p, q] == 0)
                    {
                        tile_type[p,q] = type.Empty;
                        player_income[whosturn, type2int(tile_type[p, q])]--;
                    }
                }
            }
        }
        //for(int q = 0; q < player_property[whosturn].Count; q++)
        //{
        //    remaining[
        //        player_property[whosturn][q].x, player_property[whosturn][q].y
        //        ]--;
        //    if(remaining[player_property[whosturn][q].x, player_property[whosturn][q].y] == 0)
        //    {
        //        tile_type[player_property[whosturn][q].x, player_property[whosturn][q].y] = type.Empty;
        //        player_property[whosturn].RemoveAt(q);
        //    }
        //}
        //GameObject panel = GameObject.Find("Panel2");


    }

    public void option_0()
    {
        moves_remain--;
        if (owner[current_select_row, current_select_col] == -1) //one-time claim
        {
            owner[current_select_row, current_select_col] = whosturn;
            tile_type[current_select_row, current_select_col] = type.Empty;
            switch (tile_type[current_select_row, current_select_col])
            {
                case type.Fire:
                    player_resource[whosturn, 0] += 10;
                    break;
                case type.Water:
                    player_resource[whosturn, 1] += 10;
                    break;
                case type.Earth:
                    player_resource[whosturn, 2] += 10;
                    break;
            }
        }
        else if (owner[current_select_row, current_select_col] == whosturn) //Fire Defense
        {
            defense_type[current_select_row, current_select_col] = type.Fire;
            defense[current_select_row, current_select_col]++;
            player_resource[whosturn, 0] -= 5;
        }
        else //Fire attack
        {
            defense[current_select_row, current_select_col]--;
            player_resource[whosturn, 0] -= 5;
            if (defense[current_select_row, current_select_col]==0)
            {
                defense_type[current_select_row, current_select_col] = type.Empty;
            }
            if (defense[current_select_row, current_select_col] < 0)
            {
                defense[current_select_row, current_select_col] = 0;
                owner[current_select_row, current_select_col] = whosturn;
            }
        }
    }
    public void option_1()
    {
        moves_remain--;
        if (owner[current_select_row, current_select_col] == -1) //long-term claim
        {
            owner[current_select_row, current_select_col] = whosturn;
            //coordinate temp = new coordinate(current_select_row, current_select_col);
            //player_property[whosturn].Add(temp);
            switch (tile_type[current_select_row, current_select_col])
            {
                case type.Fire:
                    //player_0[0]=new coordinate(1,1);
                    player_resource[whosturn, 0]++;
                    player_income[whosturn, 0] ++;
                    break;
                case type.Water:
                    player_resource[whosturn, 1]++;
                    player_income[whosturn, 1] ++;
                    break;
                case type.Earth:
                    player_resource[whosturn, 2]++;
                    player_income[whosturn, 2] ++;
                    break;
            }

        }
        else if (owner[current_select_row, current_select_col] == whosturn)//Water Defense
        {
            defense_type[current_select_row, current_select_col] = type.Water;
            defense[current_select_row, current_select_col]++;
            player_resource[whosturn, 1] -= 5;
        }
        else //Water attack
        { 
            defense[current_select_row, current_select_col]--;
            player_resource[whosturn, 1] -= 5;
            if (defense[current_select_row, current_select_col] == 0)
            {
                defense_type[current_select_row, current_select_col] = type.Empty;
            }
            if (defense[current_select_row, current_select_col] < 0)
            {
                defense[current_select_row, current_select_col] = 0;
                owner[current_select_row, current_select_col] = whosturn;
            }
        }
    }
    public void option_2()
    {
        moves_remain--;
        if (owner[current_select_row, current_select_col] == whosturn)//Earth Defense
        {
            defense_type[current_select_row, current_select_col] = type.Earth;
            defense[current_select_row, current_select_col]++;
            player_resource[whosturn, 2] -= 5;
        }
        else //Earth attack
        {
            defense[current_select_row, current_select_col]--;
            player_resource[whosturn, 2] -= 5;
            if (defense[current_select_row, current_select_col] == 0)
            {
                defense_type[current_select_row, current_select_col] = type.Empty;            
            }
            if (defense[current_select_row, current_select_col] < 0)
            {
                defense[current_select_row, current_select_col] = 0;
                owner[current_select_row, current_select_col] = whosturn;
            }
        }
    }
    int type2int(type input_type)
    {
        switch (input_type)
        {
            case type.Fire:
                return 0;
            case type.Water:
                return 1;
            case type.Earth:
                return 2;
        }
        return -1;
    }
}
