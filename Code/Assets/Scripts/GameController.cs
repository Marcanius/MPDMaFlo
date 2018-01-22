using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Stores and tracks the players
/// </summary>
public class GameController {

	// Player variables
	public int numPlayers = 2;
	public Player[] players;

	// Properties
	public Player ActivePlayer { get { return players[StateManager.ActivePlayerNumber]; } }
	public int RichestPlayer {
		get {
			Player player = players[0];
			int money = player.Money;
			foreach ( Player p in players ) {
				if ( p.Money > money ) {
					player = p;
					money = p.Money;
				}
			}
			return player.playerNumber;
		}
	}

	/// <summary>
	/// Construct the game controller class
	/// </summary>
	public GameController() {
		players = new Player[numPlayers];
		// Iterate through all players and assign variables
		for ( int i = 0; i < numPlayers; i++ ) {
			players[i] = new Player();
			players[i].playerNumber = i;
			Debug.Log("greenhouses: " + Extensions.greenhouses.Length + "\nplayers: " + players.Length);
			Extensions.greenhouses[i].owner = players[i];
		}
	}
}
