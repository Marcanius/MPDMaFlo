using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Flower : ATile {

	// Set variables
	protected override void Awake() {
		base.Awake();
		name = "Flower";
		price = 30;
		reward = 15;
	}

}
