using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A Monobehavior that will display messages to the players during the game.
/// </summary>
public class MessageManager : MonoBehaviour {
	public bool shown, skippable;

	/// <summary>
	/// We start the game with a pop-up up, so reflect that.
	/// </summary>
	public void Awake() {
		skippable = true;
		shown = true;
	}

	/// <summary>
	/// Displays pop-up message.
	/// </summary>
	/// <param name="mssg">The message to display.</param>
	/// <param name="delay">The duration of the pop-up; -1 to quit the game after displaying the message.</param>
	public void DoPopUp(string mssg, float delay) {
		if ( delay >= 0 ) { StartCoroutine(PopUp(mssg, this, delay)); }
		else { StartCoroutine(PopUp(mssg, this, 5, true)); }
	}

	/// <summary>
	/// Shows the pop-up panel and message.
	/// </summary>
	public void Show() {
		if ( shown )
			return;
		shown = true;
		Extensions.popUpPanel.enabled = true;
		Extensions.popUp.enabled = true;
	}

	/// <summary>
	/// Hides the pop-up panel and message.
	/// </summary>
	public void Hide() {
		if ( !shown )
			return;
		shown = false;
		Extensions.popUpPanel.enabled = false;
		Extensions.popUp.enabled = false;
	}

	/// <summary>
	/// Skips the pop-up panel and message if able.
	/// </summary>
	public void Skip() {
		if ( skippable )
			Hide();
	}

	/// <returns>Whether there is a pop-up on the screen.</returns>
	public bool IsPopUpUp() {
		return shown;
	}
	
	/// <summary>
	/// Coroutine for showing the pop-up, and then hiding it after a delay.
	/// </summary>
	/// <param name="mssg">The message to show.</param>
	/// <param name="reference">The message manager itself.</param>
	/// <param name="delay">The delay after which we hide the message.</param>
	public static IEnumerator PopUp(string mssg, MessageManager reference, float delay = 5) {
		Extensions.popUp.GetComponent<Text>().text = mssg;
		reference.skippable = false;
		reference.Show();
		yield return new WaitForSeconds(1);
		reference.skippable = true;
		yield return new WaitForSeconds(delay - 1);
		reference.Hide();
	}

	/// <summary>
	/// Coroutine for showing the pop-up, and then exiting the program.
	/// </summary>
	/// <param name="mssg">The message to show.</param>
	/// <param name="reference">The message manager itself.</param>
	/// <param name="delay">The delay after which we exit the game.</param>
	public static IEnumerator PopUp(string mssg, MessageManager reference, float delay = 5, bool ExitProgram = false) {
		Extensions.popUp.GetComponent<Text>().text = mssg;
		reference.Hide();
		reference.Show();
		reference.skippable = false;
		yield return new WaitForSeconds(delay);
		Application.Quit();
	}

}
