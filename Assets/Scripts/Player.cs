using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public int index { get; private set; }

	public KeyCode[] mapping;
	public KeyCode[] sequence;
	public int currentKey;

    public Player(int i)
    {
        index = i;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool KeyIsInMapping (KeyCode key) {
		foreach (KeyCode mapKey in mapping) {
			if (key == mapKey) {
				return true;
			}
		}
		return false;
	}

	public void ComboResult (bool pass) {
		// currentKey is how many keys the player has gotten correct
		// If it's above the min req number (4), ComboManager sends true, else it sends false
		Debug.Log("Combo result: " + pass + "!");


		// Reset this
		currentKey = 0;
	}
}
