using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
	BUY_STATE,
	PLACE_STATE
}

public static class StateManager {

	public static GameState state = GameState.BUY_STATE;
	public static GameController controller =  new GameController();

	// GameState vars
	static int activePlayer = 0;
	static int numPlayers = 2;
	static int playStateCounter = 0;
	static int actionCounter = 4;
	public static int weekCounter = 0;

	public static int ActivePlayerNumber {
		get { return activePlayer; }
	}

	public static Player ActivePlayer {
		get { return controller.ActivePlayer; }
	}

	public static int NumberOfPlayer {
		get { return numPlayers; }
	}

	public static void ChangeState(GameState newState) {
		// Only change state if we want to go somewhere else
		if ( state == newState )
			return;

		state = newState;

		// Do some setup based on the new state.
		if ( state == GameState.PLACE_STATE ) {
			playStateCounter = 0;
			activePlayer = controller.RichestPlayer;
		}
		else {
			activePlayer = 0;
			weekCounter++;
		}
	}

	public static void NextPlayer() {
		activePlayer++;
		playStateCounter++;
		actionCounter = 4;

		if ( activePlayer >= numPlayers ) {
			activePlayer = 0;
			if ( state == GameState.BUY_STATE )
				ChangeState(GameState.PLACE_STATE);
		}

		if ( state == GameState.PLACE_STATE )
			if ( playStateCounter >= numPlayers )
				ChangeState(GameState.BUY_STATE);
	}
}

