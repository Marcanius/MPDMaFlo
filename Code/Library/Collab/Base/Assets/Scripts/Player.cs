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
	public int Debt {
		get { return debt; }
		set { debt = value; }
	}

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

		// React to a click
		if ( Extensions.inputSystem.GetClick() ) {
			// Cast a ray at the cursor position
			Ray ray = Extensions.camera.ScreenPointToRay(Extensions.inputSystem.GetCursorPosition());
			RaycastHit hit;

			// If we hit a clickable tile, activate its OnClick
			if ( Physics.Raycast(ray, out hit) ) {
				if ( hit.collider.tag == "Clickable" )
					hit.collider.GetComponent<Tile>().OnClick();
			}
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
		return ( money + debt ) >= price;
	}

	public void ApplyDebt() {
		// Only apply the debt at the end of a players Buy phase
		if ( StateManager.state == GameState.PLACE_STATE || StateManager.ActivePlayer != this ) {
			return;
		}
		AddOrSubtractMoney(debt);
		debt = 0;
	}

	public void PickUp(Tile tile, string origin) { }

	public void PlaceTile(Tile tile) { }
}
