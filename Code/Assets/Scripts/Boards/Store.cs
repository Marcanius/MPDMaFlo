using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : ABoard {

	public ATile[] prefabs;


	// Set variables at the Start
	protected override void Start() {
		base.Start();
		width = 3;
		height = 2;
		grid = new ATile[width, height];

		tileWidth = 1.25f;
		tileHeight = 1.2f;

		GenerateTiles();
	}

	/// <summary>
	/// Generates the tile in its grid
	/// </summary>
	void GenerateTiles() {
		// Iterate over all prefabs, calculating the gridpos and setting up the rest
		for ( int i = 0; i < prefabs.Length; i++ ) {
			Vector2 pos = new Vector2(i % width, i/ width);
			ATile tile = GenerateTile(i, pos);
			grid[(int)pos.x, (int)pos.y] = tile;
		}
	}

	/// <summary>
	/// Generates a tile
	/// </summary>
	/// <param name="i">The iteration, used to get the right tile</param>
	/// <param name="pos">The grid position for the new tile</param>
	ATile GenerateTile(int i, Vector2 pos) {
		ATile tile = Instantiate(prefabs[i]);
		tile.transform.position = GridPosToWorld(pos, transform.position.y);
		tile.storagePlace = this;
		tile.gridPos = pos;
		tile.tilePositionState = TilePositionState.Store;
		return tile;
	}

	/// <summary>
	/// Place a tile onto the board
	/// </summary>
	/// <param name="tile">The tile to be place onto the board</param>
	/// <returns></returns>
	public override bool PlaceTile(ATile tile) {
		// During the buy phase, placing a tile removes its cost from the debt of the active player, and destroys the tile
		if ( StateManager.State == GameState.BUY_STATE ) {
			StateManager.ActivePlayer.Debt += tile.Price;
			Destroy(tile.gameObject);
		}

		// During the place phase, placing a tile deletes it if its owner is not the active player
		else {
			if ( tile.owner != StateManager.ActivePlayer ) {
				Destroy(tile.gameObject);
				StateManager.ActionCounter--;
			}
			else {
				Extensions.DoPopUp("Cannot Destroy own tiles", 2);
			}
		}

		// We succesfully placed the tile in the store
		return true;
	}
	/// <summary>
	/// Pick up a tile from the store
	/// </summary>
	/// <param name="gridPos">Which tile we clicked</param>
	public override void PickupTile(Vector2 gridPos) {
		// During the place state, player cannot pick tiles up from the store	
		if ( StateManager.State == GameState.PLACE_STATE )
			return;

		// During the buy phase 
		ATile tile = grid[(int)gridPos.x, (int)gridPos.y];
		if ( tile == null )
			return;

		// The player can only pick tiles up if they have the funds for it
		if ( StateManager.ActivePlayer.CanAfford(tile.Price) ) {
			GivePlayerTile(tile.name);
		}
		else {
			Extensions.DoPopUp("Insufficient funds", 2);
		}
	}

	/// <summary>
	/// Picking a tile up from the store makes a new one of the correct type and color, and gives it to the player
	/// </summary>
	/// <param name="name">What kind of tile we want to make</param>
	public void GivePlayerTile(string name) {
		int playerNr = StateManager.ActivePlayerNumber;
		Player player = StateManager.ActivePlayer;

		// Only create tiles with a valid name
		if ( !Extensions.playerTiles[playerNr].ContainsKey(name) )
			return;

		// Create the tile
		ATile created = Instantiate(Extensions.playerTiles[playerNr][name]);
		created.storagePlace = null;
		created.tilePositionState = TilePositionState.Hand;
		created.owner = player;

		// Add the cost of the tile to the debt of the player
		Debug.Log("Adding Player Debt: " + StateManager.ActivePlayer.Debt + "\nAdding: " + created.Price);
		player.Debt -= created.Price;
		Debug.Log("Added Debt, debt remaining: " + StateManager.ActivePlayer.Debt);
	}
}