using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Bench : ATile {

	// Set variables
	protected override void Awake() {
		base.Awake();
		name = "Bench";
		price = 60;
		reward = 30;
	}
}
