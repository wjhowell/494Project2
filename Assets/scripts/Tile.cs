using UnityEngine;
using System.Collections;

public enum ResourceType {
	WATER,
	FIRE,
	GRASS,
	NONE
}

public enum StructureType {
	NORMAL    , // capturable
	FORTIFIED , // attackable
	IMPASSABLE
}

public class Util {
	public static Vector2 PointIsIn(Vector2 tap_point) {
		return new Vector2(Mathf.Round(tap_point.x/Tile.GlobalTileSize.x), 
			               Mathf.Round(tap_point.y/Tile.GlobalTileSize.y));
	}

	public static ResourceType GetRandomTileResource() {
		switch (Random.Range (0, 2) % 3) {
		case 0:
			return ResourceType.FIRE;
		case 1:
			return ResourceType.GRASS;
		case 2:
			return ResourceType.WATER;
		}
		throw new UnityException ("Impossible branch");
	}
}

/// <summary>
/// A Tile is a basic element of the game's map. <br />
/// It has a player owner, a resource type, a level, structure type
/// </summary>
public class Tile : MonoBehaviour  {

	/// <summary>
	/// The size of every tile on the map.
	/// </summary>
	static public Vector2 GlobalTileSize = new Vector2(50f, 50f);

	static public Vector2 GlobalPadding = new Vector2 (2f, 2f);

	static public float GlobalResourceProb = 0.1f;

	/// <summary>
	/// Converts owner enum into its flag color.
	/// </summary>
	/// <returns>color of represent owner's flag</returns>
	/// <param name="o">Oh...</param>
	public static Color ConvertOwnerToColor(Owner o) {
		switch (o) {
		case Owner.BLUE:
			return new Color (0f, 0f, 1f);
		case Owner.RED:
			return new Color (1f, 0f, 0f);
		case Owner.NO_ONE:
			return new Color (1f, 1f, 1f);
		}
		throw new UnityException ("Invalid owner, cannot convert to color!");
	}

	Owner current_owner = Owner.NO_ONE;
	public Owner CurrentOwner {
		get { return current_owner; }
		set {
			current_owner = value;
			color = ConvertOwnerToColor (current_owner);
		}
	}

	public Vector2 tile_location;
	public Vector2 PixelLocation {
		get { return new Vector2 (tile_location.x*GlobalTileSize.x, tile_location.y*GlobalTileSize.y); }
		set { tile_location = new Vector2 (Mathf.Round(value.x/GlobalTileSize.x), Mathf.Round(value.y/GlobalTileSize.y)); }
	}

	public Color color = new Color (1f, 1f, 1f);

	bool is_selectable = false;
	public bool IsSelectable {
		get { return is_selectable; }
		set {
			is_selectable = value;
			if (is_selectable && animation_state != null) {
				animation_state.ChangeState (new TileFlashAnimation (this));
			} else {
				animation_state.Reset ();
				color = ConvertOwnerToColor (current_owner);
			}
		}
	}

	StateMachine animation_state;
	ResourceType resource_type;

	// upon instantiation?
	void Awake() {
		animation_state = new StateMachine ();
		if (Random.value < GlobalResourceProb)
			resource_type = Util.GetRandomTileResource ();
		else
			resource_type = ResourceType.NONE;
	}

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		animation_state.Update ();
	}

	// IQpierce on drawing quads
	// http://forum.unity3d.com/threads/draw-a-simple-rectangle-filled-with-a-color.116348/
	private static Texture2D s_rect_texture;
	private static GUIStyle s_rect_style;

	// tile rendering code
	void OnGUI () {
		
		Rect rect_bounds = new Rect (PixelLocation.x, PixelLocation.y, 
			GlobalTileSize.x - GlobalPadding.x, GlobalTileSize.y - GlobalPadding.y);
		if (s_rect_texture == null) {
			s_rect_texture = new Texture2D( 1, 1 );
		}

		if (s_rect_style == null) {
			s_rect_style = new GUIStyle();
		}

		s_rect_texture.SetPixel(0, 0, color);
		s_rect_texture.Apply();

		s_rect_style.normal.background = s_rect_texture;

		GUI.Box(rect_bounds, GUIContent.none, s_rect_style);
		switch (resource_type) {
		case ResourceType.FIRE:
			GUI.TextArea (rect_bounds, "F");
			break;
		case ResourceType.GRASS:
			GUI.TextArea (rect_bounds, "G");
			break;
		case ResourceType.WATER:
			GUI.TextArea (rect_bounds, "W");
			break;
		}
	}
}

public class TileFlashAnimation : State {
	float select_flash_interval = 1f;
	float select_flash_delay = 0f;
	bool select_flash_up = false;
	Tile parent_tile;

	public TileFlashAnimation(Tile parent_) {
		parent_tile = parent_;
	}

	public override void OnUpdate (float time_delta_fraction) {
		select_flash_delay -= Time.deltaTime;
		float multi = (select_flash_delay / select_flash_interval);
		if (select_flash_up)
			multi = 1f - multi;
		multi = 0.5f + multi * 0.5f;
		parent_tile.color = Tile.ConvertOwnerToColor (parent_tile.CurrentOwner);
		parent_tile.color.b *= multi;
		parent_tile.color.r *= multi;
		parent_tile.color.g *= multi;

		if (select_flash_delay < 0f) {
			select_flash_up = !select_flash_up;
			select_flash_delay = select_flash_interval;
		}
	}

}
