using UnityEngine;

public class Board : ABoard {

	// Set variables at the Start
	protected override void Start() {
		base.Start();
		width = 7;
		height = 7;
		grid = new ATile[width, height];

		tileWidth = 1.333f;
		tileHeight = tileWidth;
	}

	/// <summary>
	/// Place a tile onto the board
	/// </summary>
	/// <param name="tile">The tile to be place onto the board</param>
	/// <returns></returns>
	public override bool PlaceTile(ATile tile) {
		// Prevent illegal placing
		if ( StateManager.State == GameState.BUY_STATE || !base.PlaceTile(tile) ) {
			return false;
		}

		// Placing a tile takes an action
		StateManager.ActionCounter--;

		tile.tilePositionState = TilePositionState.Board;

		// Tiles can be freely picked up if they were placed by the player this turn. Otherwise you can only pick up opponents tiles to destroy them.
		if ( tile.owner == StateManager.ActivePlayer )
			tile.placedWeek = StateManager.WeekCounter;

		// Add the reward of the placed tile to the rewards that player gets every turn.
		tile.owner.Reward += tile.Reward;

		// We succeeded in placing the tile
		return true;
	}

	/// <summary>
	/// Pick a tile up from the board
	/// </summary>
	/// <param name="gridPos">The position in the grid of the tile we want to pick up</param>
	public override void PickupTile(Vector2 gridPos) {
		ATile tile = grid[(int)gridPos.x, (int)gridPos.y];
		if ( tile == null )
			return;

		// We cannot pick tiles up if we are in the buy phase, or if we own them, but didn't place them this week	
		if ( StateManager.State == GameState.BUY_STATE ||
				( tile.owner == StateManager.ActivePlayer &&
				  tile.placedWeek != StateManager.WeekCounter ) )
			return;

		// Taking a tile back takes back an action.
		if ( tile.owner == StateManager.ActivePlayer )
			StateManager.ActionCounter++;

		// Remove the reward of the placed tile from the rewards that player gets every turn.
		tile.owner.Reward -= tile.Reward;

		base.PickupTile(gridPos);
	}
}
