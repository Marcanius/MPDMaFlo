using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : ABoard {

	public Tile[] prefabs;

	protected override void Start() {
		base.Start();
		width = 3;
		height = 2;
		grid = new Tile[width, height];

		tileWidth = 1.25f;
		tileHeight = 1.2f;

		GenerateTiles();
	}

	void GenerateTiles() {
		for ( int i = 0; i < prefabs.Length; i++ ) {
			Vector2 pos = new Vector2(i % width, i/ width);
			Tile tile = GenerateTile(i, pos);
			grid[(int)pos.x, (int)pos.y] = tile;
		}
	}

	Tile GenerateTile(int i, Vector2 pos) {
		Tile tile = Instantiate(prefabs[i]);
		tile.transform.position = GridPosToWorld(pos, transform.position.y);
		tile.storagePlace = this;
		tile.gridPos = pos;
		tile.tilePositionState = TilePositionState.Store;
		return tile;
	}

	public override void PlaceTile(Tile tile) {
		return;
	}

	public void GivePlayerTile(string name) {
		int player = StateManager.ActivePlayerNumber;

		Tile created = Instantiate(Extensions.playerTiles[player][name]);
		created.storagePlace = null;
		created.tilePositionState = TilePositionState.Hand;
	}

	public override void PickupTile(Vector2 gridPos) {
		Tile tile = grid[(int)gridPos.x, (int)gridPos.y];
		if ( tile == null )
			return;

		if ( StateManager.ActivePlayer.CanAfford(tile.price) )
			GivePlayerTile(tile.name);
		else {
			StartCoroutine(Extensions.PopUp("Insufficient funds", 2));
		}
		//TODO: game mechanics(costs etc)
	}

}
