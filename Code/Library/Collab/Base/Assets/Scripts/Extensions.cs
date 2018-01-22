using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Extensions : MonoBehaviour {
	// GUIText
	static Text popUp;
	public Text popUpElement;
	static Image gameOverPanel;
	public Image gameOverPanelElement;
	static Text winningPlayer;
	public Text winningPlayerElement;
	public Text[] scores;
	public Text turn;
	public Text player;
	public Text week;
	public Text actions;

	// Camera
	public static Camera camera;

	// Tile dictionary
	public Tile[] p1;
	public Tile[] p2;
	public Tile[] p3;
	public Tile[] p4;

	protected string[] names = new string[] { "Bench", "Flower", "Gazebo", "Pathway", "Tree"};
	public static Dictionary<int, Dictionary<string, Tile>> playerTiles;

	// InputSystem
	public static IInputSystem inputSystem;

	// Player greenhouses
	public Greenhouse[] gh;
	public static Greenhouse[] greenhouses;

	void Awake() {
		popUp = popUpElement;
		gameOverPanel = gameOverPanelElement;
		winningPlayer = winningPlayerElement;

		camera = Camera.main;
		inputSystem = InputManager.GetInputSystem();
		playerTiles = new Dictionary<int, Dictionary<string, Tile>>();

		greenhouses = gh;

		FillDict(p1, 0);
		FillDict(p2, 1);
		// TODO write more generic code
	}

	void FillDict(Tile[] array, int player) {
		Dictionary<string, Tile> dict = new Dictionary<string, Tile>();
		for ( int i = 0; i < array.Length; i++ ) {
			dict.Add(names[i], array[i]);
		}
		playerTiles.Add(player, dict);
	}


	void Start() {

	}

	// Help some member updating
	void Update() {
		UpdateInfo();
		StateManager.controller.ActivePlayer.Update();
	}

	void UpdateInfo() {
		for ( int i = 0; i < scores.Length; i++ ) {
			scores[i].text = StateManager.controller.players[i].Score.ToString();
		}

		turn.text = StateManager.state == GameState.BUY_STATE ? "Buying" : "Placing";

		player.text = ( StateManager.ActivePlayerNumber + 1 ).ToString();

		week.text = (StateManager.weekCounter + 1).ToString();

		actions.enabled = StateManager.state == GameState.PLACE_STATE ? true : false;
		actions.text = StateManager.actionCounter.ToString();

	}

	public void NextTurn() {
		StateManager.ActivePlayer.ApplyDebt();
		StateManager.NextPlayer();
	}

	public static IEnumerator PopUp(string mssg, float delay) {
		popUp.text = mssg;
		popUp.enabled = true;
		yield return new WaitForSeconds(delay);
		popUp.enabled = false;
	}

	public static void GameOver() {
		Debug.Log("GAME OVER");
		gameOverPanel.gameObject.SetActive(true);
		winningPlayer.text = "Player number " + StateManager.controller.RichestPlayer + 1;
	}
}
