using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This static provides the game with the correct input method, depending on the platform for which the game was built.
/// </summary>
public class InputManager : MonoBehaviour {
	static IInputSystem instance;

	public static IInputSystem GetInputSystem() {
		if ( instance == null ) { instance = CreateInstance(); }

		return instance;
	}

	// Create platform specific input system.
	static IInputSystem CreateInstance() {
#if UNITY_ANDROID
		return new AndroidInputSystem();
#elif UNITY_STANDALONE
		return new UnityInputSystem();
#endif
	}
}

/// <summary>
/// An interface for all the methods an input system needs to provide.
/// </summary>
public interface IInputSystem {
	Vector3 GetCursorPosition();
	Vector3 GetCursorScreenPosition(float depth = 0);
	bool GetClick();
}

/// <summary>
/// The input system used for windows builds.
/// </summary>
public class UnityInputSystem : IInputSystem {

	/// <returns>The mouse cursor position</returns>
	public Vector3 GetCursorPosition() {
		return Input.mousePosition;
	}

	/// <summary>
	/// Get the cursor position in world coordinates.
	/// </summary>
	/// <param name="depth">The distance from the camera.</param>
	/// <returns>The world position of the cursor.</returns>
	public Vector3 GetCursorScreenPosition(float depth = 0) {
		Vector3 cursorPos = GetCursorPosition();
		return Extensions.camera.ScreenToWorldPoint(new Vector3(cursorPos.x, cursorPos.y, depth));
	}

	/// <returns>Whether the first mouse button was pushed down.</returns>
	public bool GetClick() {
		return Input.GetMouseButtonDown(0);
	}
}

/// <summary>
/// The input system used for android builds.
/// </summary>
public class AndroidInputSystem : IInputSystem {

	// Since we won't always be touching the screen we need to store the cursor position.
	Vector3 cursorPosition;


	/// <returns>Whether the user started or stopped touching the screen this frame.</returns>
	public bool GetClick() {
		Touch[] touches = Input.touches;
		if ( touches.Length == 0 )
			return false;
		Touch touch = touches[0];
		return touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Ended;
	}

	/// <returns>The last position at which the user touched the screen.</returns>
	public Vector3 GetCursorPosition() {
		Touch[] touches = Input.touches;
		if ( touches.Length > 0 ) {
			cursorPosition = Input.touches[0].position;
		}
		return cursorPosition;
	}

	/// <summary>
	/// Get the cursor position in world coordinates.
	/// </summary>
	/// <param name="depth">The distance from the camera.</param>
	/// <returns>The world position of the cursor.</returns>
	public Vector3 GetCursorScreenPosition(float depth = 0) {
		Vector3 cursorPos = GetCursorPosition();
		return Extensions.camera.ScreenToWorldPoint(new Vector3(cursorPos.x, cursorPos.y, depth));
	}
}