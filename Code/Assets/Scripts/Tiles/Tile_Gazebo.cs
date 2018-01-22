using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Gazebo : ATile {

	// Set variables
	protected override void Awake() {
		base.Awake();
		name = "Gazebo";
		price = 60;
		reward = 30;
	}

}
