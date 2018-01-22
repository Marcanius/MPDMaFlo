using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class which stores an inventory of tiles, has a pile of money.
/// It can perform the player actions: pick up tiles from the store and greenhouse and garden, place them in the garden or the greenhouse or the trash.
/// It should end the turn when there are no more actions remaining.
/// </summary>
public class Player {

	// Game-Surrounding stuff
	public int playerNumber;
	public int Score { get { return money; } }

	// In-Game Stuff
	int money, debt;
	Tile tileHeld;
	string tileOrigin;

	// Use this for initialization
	public Player() {
		money = 350;
	}

	// TODO: Call this, somewhere(maybe all players, maybe only active?)
	public void Update() {
		if ( StateManager.ActivePlayerNumber != playerNumber )
			return;

		// BUY PHASE
		if ( StateManager.state == GameState.BUY_STATE ) {
			// Place tile in greenhouse, add its cost to the debt to be paid at the end of the phase

			// Place tile in store, subtract its cost from the debt to be paid at the end of the phase

		}
		// PLACE PHASE
		else {
			// Pick up tiles from your greenhouse

			// Place tiles on board OR back in greenhouse

			// Pick up tiles from the board
		}
	}

	public void EndTurn() {
		
		StateManager.NextPlayer();
	}

	public void AddOrSubtractMoney(int amount) {
		if ( amount < 0 && money + amount < 0 ) {
			Debug.LogError("Insufficient Money Remaining");
			return;
		}
		Debug.Log("Adding " + amount + " Money to player " + playerNumber);
		money += amount;
		Debug.Log("Player " + playerNumber + " has " + money + " Money remaining");
	}

	public bool CanAfford(int price) {
		Debug.Log(money);
		return money >= price;
	}

	public void PickUp(Tile tile, string origin) { }


}
