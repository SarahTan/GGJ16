using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public int index { get; private set; }

	public KeyCode[] mapping;
	public KeyCode[] sequence;
	public int currentKey;

	public Vector3 arrowKeysPos;
	public GameObject arrows;

	public Player (int i) {
		index = i;

		arrows = new GameObject ();
		arrows.name = "Player" + (index+1) + " Arrows";

		if (index == 0) {
			arrowKeysPos = Vector3.zero;
		} else if (index == 1) {

		}
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ComboResult (bool pass) {
		// currentKey is how many keys the player has gotten correct
		// If it's above the min req number (4), ComboManager sends true, else it sends false
		Debug.Log("Combo result: " + pass + "!");


		// Reset this
		currentKey = 0;
	}
}
