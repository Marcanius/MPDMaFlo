using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract Board class, used to generalize calls towards all storage places for tiles
/// </summary>
public abstract class ABoard : MonoBehaviour {

	// Variables for the tile grid
	public ATile[,] grid;
	protected int width, height;

	// Variables for tile placement
	public float tileWidth, tileHeight;
	public Transform originPos;

	// Origin is upper left corner(where the Tile would be placed)
	// Direction is always 1, -1 for the upper left origin
	Vector2 origin, direction;

	// Properties for accessibility
	public int Width { get { return width; } }
	public int Height { get { return height; } }
	public float TileWidth { get { return tileWidth; } }
	public float TileHeight { get { return tileHeight; } }
	public Vector2 LocalPos { get { return new Vector2(transform.position.x, transform.position.z); } }

	// Set tag, direction and origin in start
	protected virtual void Start() {
		GetComponent<Collider>().tag = "Board";

		direction = new Vector2(1, -1);

		origin = new Vector2(originPos.position.x, originPos.position.z);
	}

	protected virtual void Update() {

	}

	/// <summary>
	/// Place a tile onto the board
	/// </summary>
	/// <param name="tile">The tile to be place onto the board</param>
	/// <returns></returns>
	public virtual bool PlaceTile(ATile tile) {
		// Get the grid position where the mouse hovers
		Vector3 pos = Extensions.inputSystem.GetCursorScreenPosition(10);
		Vector2 gridPos = WorldToGridPos(pos);

		// If there is already a tile, or if we try to illegally move a tile, return a fail
		if ( grid[(int)gridPos.x, (int)gridPos.y] != null || (tile.owner != StateManager.ActivePlayer && tile.gridPos != gridPos))
			return false;

		// Place tile in the grid and set tile variables
		grid[(int)gridPos.x, (int)gridPos.y] = tile;
		tile.transform.position = GridPosToWorld(gridPos, transform.position.y);
		tile.storagePlace = this;
		tile.gridPos = gridPos;

		// Return succes
		return true;
	}

	/// <summary>
	/// Pick a tile up from the board
	/// </summary>
	/// <param name="gridPos">The position in the grid of the tile we want to pick up</param>
	public virtual void PickupTile(Vector2 gridPos) {
		// Only possible if the player has actions left
		if(StateManager.ActionCounter <= 0)
			return;

		// If there is tile, place it to hand and reset variables
		ATile tile = grid[(int)gridPos.x, (int)gridPos.y];
		if ( tile != null ) {
			tile.storagePlace = null;
			tile.tilePositionState = TilePositionState.Hand;
			grid[(int)gridPos.x, (int)gridPos.y] = null;
		}
		else
			Debug.Log("Tried to remove non-existent tile");
	}

	/// <summary>
	/// Calculate a global world position based on a local grid position.
	/// </summary>
	/// <param name="pos">The position in the grid</param>
	/// <param name="depth">The desired distance from the camera</param>
	protected virtual Vector3 GridPosToWorld(Vector2 pos, float depth) {
		return new Vector3(( pos.x * TileWidth * direction.x ) + origin.x, depth,
						   ( pos.y * TileHeight * direction.y ) + origin.y);
	}

	/// <summary>
	/// Calculate a local grid position based on a global world location
	/// </summary>
	/// <param name="pos">The global world location</param>
	protected virtual Vector2 WorldToGridPos(Vector3 pos) {
		return new Vector2(Mathf.Min(Width - 1, Mathf.Max(0, Mathf.Abs(Mathf.Round(( pos.x - origin.x ) / TileWidth)))),
						   Mathf.Min(Height - 1, Mathf.Max(0, Mathf.Abs(Mathf.Round(( pos.z - origin.y ) / TileHeight)))));
	}
}
