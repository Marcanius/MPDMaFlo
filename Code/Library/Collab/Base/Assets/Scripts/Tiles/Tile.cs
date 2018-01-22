using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TilePositionState {
	Store,
	Hand,
	GreenHouse,
	Board
}
public abstract class Tile : MonoBehaviour {

	public TilePositionState tilePositionState;
	Collider collider;
	public ABoard storagePlace;
	public Vector2 gridPos;
	public int price;

	protected virtual void Start() {
		collider = GetComponent<Collider>();
		collider.tag = "Clickable";

	}

	protected virtual void Update() {
		// Move the tile around if it's in the hand
		if ( tilePositionState == TilePositionState.Hand ) {
			transform.localPosition = Extensions.inputSystem.GetCursorScreenPosition(9);
		}
		Debug.Log(storagePlace.ToString());
	}

	/// <summary>
	/// Through a raycast, we found out this tile was clicked. React on this event.
	/// </summary>
	public virtual void OnClick() {
		Debug.Log(this + " clicked");
		if ( tilePositionState == TilePositionState.Hand )
			PutDown();
		else {
			PickUp();
		}
	}

	/// <summary>
	/// This tile was placed somewhere, and then picked up by a player
	/// Put this tile into the players hand, and store where it came from
	/// </summary>
	protected void PickUp() {
		StateManager.ActivePlayer.PickUp(this, storagePlace);
		storagePlace.PickupTile(gridPos);
	}

	protected void PutDown() {
		Ray ray = new Ray(transform.position, -transform.up);

		RaycastHit hit;

		// You will probably hit the board, store, or greenhouse. Check for those.
		if ( Physics.Raycast(ray, out hit) ) {
			var collider = hit.collider;
			if ( collider.tag == "Board" ) {
				var board = collider.GetComponent<ABoard>();
				board.PlaceTile(this);
			}
		}
	}
}
