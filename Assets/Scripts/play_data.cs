using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum type { Fire, Water, Earth, Empty };
public struct coordinate {
    int x;
    int y;
    public coordinate(int a, int b)
    {
        x = a;
        y = b;
    }
}

public class play_data : MonoBehaviour {
    public static play_data instance;

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
    //map sprites
    public Sprite[] empty;
    public Sprite[] fire;
    public Sprite[] water;
    public Sprite[] earth;

    //public coordinate[] player_0;
    //public coordinate[] player_1;
    //public coordinate[] player_2;
    //public coordinate[] player_3;

    //public List<coordinate>[] player_mine = new List<coordinate>[4];

    public int whosturn;


    // Use this for initialization
    void Start () {
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
        remaining[1, 0] = 10;
        remaining[1, 1] = 10;
        remaining[1, 2] = 10;
        remaining[0, 1] = 10;
        remaining[2, 1] = 10;
    }
	
	// Update is called once per frame
	void Update () {
        //print(owner[0,1]);
	}

    public void next_turn()
    {
        whosturn++;
        if (whosturn == 4)
        {
            whosturn = 0;
        }

        for (int p = 0; p < 3; p++)
        {
                player_resource[whosturn, p] += player_income[whosturn, p];
        }
        GameObject panel = GameObject.Find("Panel2");
        

    }

    public void option_0()
    {
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
            player_resource[whosturn, 0] -= 10;
        }
        else //Fire attack
        {
            defense[current_select_row, current_select_col]--;
            player_resource[whosturn, 0] -= 10;
            if (defense[current_select_row, current_select_col]==0)
            {
                defense_type[current_select_row, current_select_col] = type.Empty;
            }
        }
    }
    public void option_1()
    {
        if (owner[current_select_row, current_select_col] == -1) //long-term claim
        {
            owner[current_select_row, current_select_col] = whosturn;
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
            player_resource[whosturn, 1] -= 10;
        }
        else //Water attack
        { 
            defense[current_select_row, current_select_col]--;
            player_resource[whosturn, 1] -= 10;
            if (defense[current_select_row, current_select_col] == 0)
            {
                defense_type[current_select_row, current_select_col] = type.Empty;
            }
        }
    }
    public void option_2()
    {
        if (owner[current_select_row, current_select_col] == whosturn)//Earth Defense
        {
            defense_type[current_select_row, current_select_col] = type.Earth;
            defense[current_select_row, current_select_col]++;
            player_resource[whosturn, 2] -= 10;
        }
        else //Earth attack
        {
            defense[current_select_row, current_select_col]--;
            player_resource[whosturn, 2] -= 10;
            if (defense[current_select_row, current_select_col] == 0)
            {
                defense_type[current_select_row, current_select_col] = type.Empty;
            }
        }
    }
}
