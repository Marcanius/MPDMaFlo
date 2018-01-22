using UnityEngine;

// Enum for position states
public enum TilePositionState {
	Store,
	Hand,
	GreenHouse,
	Board
}

/// <summary>
/// An abstract tile that provides all functionality, children of this class only specify the price and reward.
/// </summary>
public abstract class ATile : MonoBehaviour {

	// Variables for positioning
	public TilePositionState tilePositionState;
	public ABoard storagePlace;
	public Vector2 gridPos;
	
	// Awereness variables
	public Player owner;
	Collider collider;
	
	// Variables for the game systems
	public int placedWeek = 0;
	protected int price, reward;

	// Properties
	public int Price { get { return price; } }
	public int Reward { get { return reward; } }

	// On awake assign tag
	protected virtual void Awake() {
		collider = GetComponent<Collider>();
		collider.tag = "Clickable";
	}

	protected virtual void Update() {
		// Lock tile to cursor while it's in hand
		if ( tilePositionState == TilePositionState.Hand ) {
			transform.localPosition = Extensions.inputSystem.GetCursorScreenPosition(9);
		}
	}

	/// <summary>
	/// Through a raycast, we found out this tile was clicked. React to this event.
	/// </summary>
	public virtual void OnClick() {
		if ( tilePositionState == TilePositionState.Hand ) { PutDown(); }
		else { PickUp(); }
	}

	/// <summary>
	/// We have been told by the inputmanager we were picked up
	/// </summary>
	protected void PickUp() {
		storagePlace.PickupTile(gridPos);
	}

	/// <summary>
	/// We have been told by the inputmanager we were placed onto something
	/// </summary>
	protected void PutDown() {
		// Cast ray
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
