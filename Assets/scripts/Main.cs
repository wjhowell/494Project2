using UnityEngine;
using System.Collections;

public enum Owner {
	NO_ONE,
	RED,
	BLUE
};

public class Main : MonoBehaviour {

    public static Main M;
    public Player redPlayer;
    public Player bluePlayer;

	public Tile TileSpacePrefab;

	Tile[,] board;

	void SplayTiles(Rect range) {
		int x_max = Mathf.RoundToInt (range.max.x);
		int y_max = Mathf.RoundToInt (range.max.y);
		int x_min = Mathf.RoundToInt (range.min.x);
		int y_min = Mathf.RoundToInt (range.min.y);

		board = new Tile[Mathf.RoundToInt (range.max.x - range.min.x), Mathf.RoundToInt (range.max.y - range.min.y)];
		for (int x = x_min; x < x_max; ++x) {
			for (int y = y_min; y < y_max; ++y) {
				Tile new_tile = Instantiate<Tile> (TileSpacePrefab);
				new_tile.tile_location = new Vector2 ((float)x, (float)y);
				board [x, y] = new_tile;
			}
		}
	}

	StateMachine gui_control;
	public Owner current_player = Owner.RED;

	// Use this for initialization
	void Start () {
        M = this;
        redPlayer = new Player();
        bluePlayer = new Player();
        SplayTiles (new Rect (0f, 0f, 10f, 10f));
		// mark starting tile
		board [1, 1].CurrentOwner = Owner.BLUE;
		board [8, 6].CurrentOwner = Owner.RED;
		UpdateSelectableTiles ();
	}

	public void UpdateSelectableTiles() {
		UI.U.timeRemaining = 30f;
		// highlight all selectable tiles
		for (int x = 0; x < board.GetLength (0); ++x) {
			for (int y = 0; y < board.GetLength (1); ++y) {
				board [x, y].IsSelectable = false;
			}
		}

		for (int x = 0; x < board.GetLength(0); ++x) {
			for (int y = 0; y < board.GetLength(1); ++y) {
				
				if (board [x, y].CurrentOwner != current_player)
					continue;

				if (x > 0)
					board [x - 1, y].IsSelectable = true;
				if (x + 1 < board.GetLength(0))
					board [x + 1, y].IsSelectable = true;
				if (y > 0)
					board [x, y - 1].IsSelectable = true;
				if (y + 1 < board.GetLength(1))
					board [x, y + 1].IsSelectable = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1)) {
			HandlePressEvent (new Vector2 (Input.mousePosition.x, Input.mousePosition.y));
		} else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
			HandlePressEvent (new Vector2 (Input.GetTouch(0).position.x, Input.GetTouch(0).position.y));
		}
	}

	void HandlePressEvent(Vector2 loc) {
		int x = Mathf.FloorToInt(loc.x/Tile.GlobalTileSize.x);
		int y = board.GetLength(1) - Mathf.FloorToInt(loc.y/Tile.GlobalTileSize.y) - 1;
		TakeTile (x, y);
	}

	void TakeTile(int x, int y){
		if (!board [x, y].IsSelectable)
			return;
		bool stolen = (board[x, y].CurrentOwner != current_player && board[x, y].CurrentOwner != Owner.NO_ONE);
		board [x, y].CurrentOwner = current_player;
		if (current_player == Owner.RED)
		{
			current_player = Owner.BLUE;
			redPlayer.numTilesOwned++;
			if (stolen) bluePlayer.numTilesOwned--;
			switch(board[x, y].resource_type)
			{
			case ResourceType.FIRE:
				redPlayer.FireOwned++;
				if (stolen) bluePlayer.FireOwned--;
				break;
			case ResourceType.GRASS:
				redPlayer.GrassOwned++;
				if (stolen) bluePlayer.GrassOwned--;
				break;
			case ResourceType.WATER:
				redPlayer.WaterOwned++;
				if (stolen) bluePlayer.WaterOwned--;
				break;
			}

		}
		else if (current_player == Owner.BLUE)
		{
			current_player = Owner.RED;
			bluePlayer.numTilesOwned++;
			if (stolen) redPlayer.numTilesOwned--;
			switch (board[x, y].resource_type)
			{
			case ResourceType.FIRE:
				bluePlayer.FireOwned++;
				if (stolen) redPlayer.FireOwned--;
				break;
			case ResourceType.GRASS:
				bluePlayer.GrassOwned++;
				if (stolen) redPlayer.GrassOwned--;
				break;
			case ResourceType.WATER:
				bluePlayer.WaterOwned++;
				if (stolen) redPlayer.WaterOwned--;
				break;
			}
		}
		UpdateSelectableTiles ();
	}
}
