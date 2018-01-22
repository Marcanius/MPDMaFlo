using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Pathway : ATile {

	// Set variables
	protected override void Awake() {
		base.Awake();
		name = "Pathway";
		price = 40;
		reward = 20;
	}

}
