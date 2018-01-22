using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greenhouse : ABoard {
	public Player owner;

	// Set variables at the Start
    protected override void Start() {
        base.Start();
		width = 3;
		height = 2;
		grid = new ATile[width, height];

		tileWidth = 1.25f;
		tileHeight = 1.2f;
	}

	/// <summary>
	/// Place a tile onto the board
	/// </summary>
	/// <param name="tile">The tile to be place onto the board</param>
	/// <returns></returns>
	public override bool PlaceTile(ATile tile){
		// Prevent illegal moves
		if(tile.owner != owner || owner != StateManager.ActivePlayer || !base.PlaceTile(tile))
			return false;

		// Set tile to greenhouse
		tile.tilePositionState = TilePositionState.GreenHouse;
		return true;
	}

	/// <summary>
	/// Pick a tile up from the board
	/// </summary>
	/// <param name="gridPos">The position in the grid of the tile we want to pick up</param>
	public override void PickupTile(Vector2 gridPos){
		// Prevent illegal moves
		if(owner != StateManager.ActivePlayer)
			return;
		
		// Handle the rest with the base
		base.PickupTile(gridPos);
	}

}
