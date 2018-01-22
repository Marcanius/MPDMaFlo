using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABoard : MonoBehaviour {

	public Tile[,] grid;
	protected int width, height;

	// Variables for tile placement
	// public so can edit in unity
	public float tileWidth, tileHeight;
	public Transform originPos;
	Vector2 origin, direction;



	public int Width { get { return width; } }
	public int Height { get { return height; } }
	public float TileWidth { get { return tileWidth; } }
	public float TileHeight { get { return tileHeight; } }

	public Vector2 LocalPos { get { return new Vector2(transform.position.x, transform.position.z); } }

	protected virtual void Start() {
		GetComponent<Collider>().tag = "Board";

		direction = new Vector2(1, -1);

		origin = new Vector2(originPos.position.x, originPos.position.z);
	}

	protected virtual void Update() {

	}

	public virtual void PlaceTile(Tile tile) {
		Vector3 pos = Extensions.inputSystem.GetCursorScreenPosition(10);
		Vector2 gridPos = WorldToGridPos(pos);
		if ( grid[(int)gridPos.x, (int)gridPos.y] != null )
			return;

		grid[(int)gridPos.x, (int)gridPos.y] = tile;
		tile.transform.position = GridPosToWorld(gridPos, transform.position.y);

		tile.storagePlace = this;
		tile.gridPos = gridPos;
	}

	public virtual void PickupTile(Vector2 gridPos) {
		Tile tile = grid[(int)gridPos.x, (int)gridPos.y];
		if ( tile != null ) {
			tile.storagePlace = null;
			tile.tilePositionState = TilePositionState.Hand;
			grid[(int)gridPos.x, (int)gridPos.y] = null;
		}
		else
			Debug.Log("Tried to remove non-existent tile");
	}

	protected virtual Vector3 GridPosToWorld(Vector2 pos, float depth) {
		return new Vector3(( pos.x * TileWidth * direction.x ) + origin.x, depth,
						   ( pos.y * TileHeight * direction.y ) + origin.y);
	}

	protected virtual Vector2 WorldToGridPos(Vector3 pos) {
		return new Vector2(Mathf.Min(Width - 1, Mathf.Max(0, Mathf.Abs(Mathf.Round(( pos.x - origin.x ) / TileWidth)))),
						   Mathf.Min(Height - 1, Mathf.Max(0, Mathf.Abs(Mathf.Round(( pos.z - origin.y ) / TileHeight)))));
	}
}
