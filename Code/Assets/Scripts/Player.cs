using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class which stores an inventory of tiles, has a pile of money.
/// It can perform the player actions: pick up tiles from the store and greenhouse and garden, place them in the garden or the greenhouse or the trash.
/// </summary>
public class Player {

	// Game-Surrounding variables.
	public int playerNumber;

	// In-Game variables.
	int money, debt, reward;
	public int Money { get { return money; } }
	public int Debt {
		get { return debt; }
		set { debt = value; }
	}
	public int Reward {
		get { return reward; }
		set { reward = value; }
	}

	/// <summary>
	/// Construct the player class.
	/// </summary>
	public Player() {
		money = 350;
	}

	public void Update() {
		if ( StateManager.ActivePlayerNumber != playerNumber )
			return;

		// React to a click
		if ( Extensions.inputSystem.GetClick() ) {
			// Cast a ray at the cursor position.
			Ray ray = Extensions.camera.ScreenPointToRay(Extensions.inputSystem.GetCursorPosition());
			RaycastHit hit;

			// If we hit a clickable tile, activate its OnClick.
			if ( Physics.Raycast(ray, out hit) ) {
				if ( hit.collider.tag == "Clickable" )
					hit.collider.GetComponent<ATile>().OnClick();
			}
		}

		// Clear pop-up
		if ( Extensions.inputSystem.GetClick() ) {
			Extensions.SkipPopUp();
		}
	}

	/// <summary>
	/// Adds a money delta to the players wallet.
	/// </summary>
	/// <param name="amount">The amount to change the coffers by.</param>
	void AddOrSubtractMoney(int amount) {
		if ( amount < 0 && money + amount < 0 ) {
			Debug.LogError("Insufficient Money Remaining");
			return;
		}

		money += amount;
	}

	/// <summary>
	/// Determine whether a player can afford a purchase.
	/// </summary>
	/// <param name="price">The price of the purchase.</param>
	/// <returns>Whether the player can afford the price.</returns>
	public bool CanAfford(int price) {
		return ( money + debt ) >= price;
	}

	/// <summary>
	/// At the end of every phase, players either have to pay for their purchases, 
	/// or get rewarded for the tiles they have in the garden.
	/// </summary>
	public void ApplyMoneyDelta() {
		// Only apply the reward at the end of a players Place phase.
		if ( StateManager.State == GameState.PLACE_STATE ) {
			AddOrSubtractMoney(reward);
		}

		// Only apply the debt at the end of a players Buy phase.
		else {
			AddOrSubtractMoney(debt);
			debt = 0;
		}

	}
}
