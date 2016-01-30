using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public int index { get; private set; }

	public KeyCode[] mapping;
	public KeyCode[] sequence;
	public int currentKey;
	public bool seqFail;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool KeyIsInMapping (KeyCode key) {

		return true;
	}

	public void ComboResult (bool pass) {

	}
}
