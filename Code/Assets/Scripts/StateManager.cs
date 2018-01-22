using UnityEngine;

public enum GameState {
	BUY_STATE,
	PLACE_STATE
}

/// <summary>
/// This static class holds various variables and methods that are used in the game to represent and change the game state. It holds information on whose and which turn it is, and has methods to change these things.
/// </summary>
public static class StateManager {

	// Variables to control the game(state).
	public static GameState State = GameState.BUY_STATE;
	public static GameController Controller =  new GameController();

	// GameState vars
	static int numPlayers = 2;
	static int playStateCounter = 0;
	public static int ActionCounter = 4;
	public static int WeekCounter = 0;

	static int activePlayer = 0;
	public static int ActivePlayerNumber { get { return activePlayer; } }
	public static Player ActivePlayer { get { return Controller.ActivePlayer; } }
	public static int NumberOfPlayer { get { return numPlayers; } }

	/// <summary>
	/// Advance the turn to the next player, or next phase if applicable.
	/// </summary>
	public static void NextPlayer() {
		activePlayer++;
		playStateCounter++;
		ActionCounter = 4;

		// Loop around to the first player.
		if ( activePlayer >= numPlayers ) {
			activePlayer = 0;
			// If everyone has had a chance to buy tiles, go to the Place state.
			if ( State == GameState.BUY_STATE ) {
				// Apply the debt for every player
				foreach ( Player p in Controller.players ) {
					p.ApplyMoneyDelta();
				}
				Extensions.DoPopUp("PLACING STATE\n" +
									"Place your purchased tiles in the garden\n" +
									"Or remove tiles your opponent has already placed", 4);
				ChangeState(GameState.PLACE_STATE);
			}
		}

		// Go to the next turn after everyone has taken their turns to place/remove tiles.
		if ( State == GameState.PLACE_STATE ) {
			if ( playStateCounter >= numPlayers ) {
				// Apply the rewards for every player
				foreach ( Player p in Controller.players ) {
					p.ApplyMoneyDelta();
				}

				Extensions.DoPopUp("PURCHASE STATE\n" +
									"Buy new tiles from the store", 4);
				ChangeState(GameState.BUY_STATE);
			}
		}
	}

	/// <summary>
	/// Advance the turn to the next player.
	/// </summary>
	/// <param name="newState">The state to go to.</param>
	static void ChangeState(GameState newState) {
		// Only change state if we want to go somewhere else.
		if ( State == newState )
			return;

		State = newState;

		// Do some setup based on the new state.
		if ( State == GameState.PLACE_STATE ) {
			playStateCounter = 0;
			activePlayer = Controller.RichestPlayer;
		}
		else {
			activePlayer = 0;
			WeekCounter++;

			// After ten weeks, the game is over.
			if ( WeekCounter >= 10 ) {
				WeekCounter--;
				Extensions.GameOver();
			}
		}
	}

}

