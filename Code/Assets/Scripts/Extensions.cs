using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This static class holds various variables and methods that are used by multiple classes, to keep those classes loosely coupled from eachother.
/// </summary>
public class Extensions : MonoBehaviour {
	// GUI elements
	public static Text popUp;
	public Text popUpElement;
	public static Image popUpPanel;
	public Image popUpPanelElement;
	public static MessageManager mssgMan;
	public MessageManager mssgManElement;
	public Text[] scores;
	public Text turn;
	public Text player;
	public Text week;
	public Text actions;

	// Camera
	public static Camera camera;

	// Tile dictionary
	public ATile[] p1;
	public ATile[] p2;
	public ATile[] p3;
	public ATile[] p4;

	// Order of tiles
	protected string[] names = new string[] { "Bench", "Flower", "Gazebo", "Pathway", "Tree"};
	public static Dictionary<int, Dictionary<string, ATile>> playerTiles;

	// input system
	public static IInputSystem inputSystem;

	// Player greenhouses
	public Greenhouse[] gh;
	public static Greenhouse[] greenhouses;

	void Awake() {
		// Create static class from the in-unity defined object.
		popUp = popUpElement;
		popUpPanel = popUpPanelElement;
		mssgMan = mssgManElement;

		// Assign basic variables.
		camera = Camera.main;
		inputSystem = InputManager.GetInputSystem();
		playerTiles = new Dictionary<int, Dictionary<string, ATile>>();
		greenhouses = gh;

		// Fill player dictionaries with prefabs.
		FillDict(p1, 0);
		FillDict(p2, 1);
	}

	void Update() {
		UpdateInfo();
		StateManager.Controller.ActivePlayer.Update();
	}

	/// <summary>
	/// Fill the dictionary with the provided array for the player.
	/// </summary>
	/// <param name="array">Array of tiles to use.</param>
	/// <param name="player">Intended player</param>
	void FillDict(ATile[] array, int player) {
		Dictionary<string, ATile> dict = new Dictionary<string, ATile>();
		for ( int i = 0; i < array.Length; i++ ) {
			dict.Add(names[i], array[i]);
		}
		playerTiles.Add(player, dict);
	}

	/// <summary>
	/// Update all UI text info to match the game system.
	/// </summary>
	void UpdateInfo() {
		for ( int i = 0; i < scores.Length; i++ ) {
			scores[i].text = StateManager.Controller.players[i].Money.ToString();
		}

		turn.text = StateManager.State == GameState.BUY_STATE ? "Buying" : "Placing";

		player.text = " Player " + ( StateManager.ActivePlayerNumber + 1 ).ToString();

		week.text = ( StateManager.WeekCounter + 1 ).ToString();

		actions.text = StateManager.State == GameState.PLACE_STATE ? StateManager.ActionCounter.ToString() : "-";
	}

	public void NextTurn() {
		StateManager.NextPlayer();
	}

	public static void DoPopUp(string mssg, float delay) {
		mssgMan.DoPopUp(mssg, delay);
	}

	/// <returns>Whether there is a pop-up on the screen.</returns>
	public static bool IsPopUpUp() {
		return mssgMan.IsPopUpUp();
	}

	/// <summary>
	/// Skip the pop-up that is on screen.
	/// </summary>
	public static void SkipPopUp() {
		mssgMan.Skip();
	}

	/// <summary>
	/// Initiate game over pop-up.
	/// </summary>
	public static void GameOver() {
		Debug.Log("GAME OVER");
		string text = "GAME OVER\n" +
					"And The Winner is...\n" +
					"Player Number " + ( StateManager.Controller.RichestPlayer + 1 );

		DoPopUp(text, -1);
	}
}
