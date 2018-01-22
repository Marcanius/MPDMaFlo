using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Tree : ATile {

	// Set variables
	protected override void Awake() {
		base.Awake();
		name = "Tree";
		price = 70;
		reward = 35;
	}

}
