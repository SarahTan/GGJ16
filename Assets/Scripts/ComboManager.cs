using UnityEngine;
using System.Collections;

public class ComboManager : Singleton<ComboManager> {

	private GameManager _gameManager;

	int minNum = 4;
	int maxNum = 8;
	int numPlayers = 2;
	int minReqKeys = 1;

	GameObject arrowPrefab;
	float arrowWidth;
	float arrowGap = 0.1f;

    private Direction[] mappings = { Direction.UP, Direction.LEFT, Direction.DOWN, Direction.RIGHT }; 

    public enum Direction
    {
        UP,
        LEFT,
        DOWN,
        RIGHT
    }

	void Awake() {
		_gameManager = GameManager.Instance;

		arrowPrefab = Resources.Load ("Prefabs/UI arrow") as GameObject;
		arrowWidth = arrowPrefab.transform.localScale.x;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.Space)) {
			Test ();
		}
	}

	// For testing
	void Test() {
		int playerNum = 0;

		// Generate a player's current sequence
		GenerateSeq (playerNum, 6);
		DrawArrows (playerNum);

		// Check if the key being pressed is correct
		Direction[] testInput = new Direction[6] {Direction.LEFT, 
								Direction.RIGHT, 
								Direction.UP, 
								Direction.DOWN, 
								Direction.DOWN, 
								Direction.DOWN};
		for (int i = 0; i < testInput.Length; i++) {
			Debug.Log ("Current key: " + _gameManager.players [playerNum].currentKey + "," +
				" i: " + i);
			if (i == _gameManager.players [playerNum].currentKey) {
				CheckKey (playerNum, testInput [i]);
			} else {
				break;
			}
		}


		// Submit a sequence
		LockIn(playerNum);
	}


	// Draw the arrow keys on screen
	void DrawArrows (int playerNum) {
		Vector3 pos = _gameManager.players [playerNum].arrowKeysPos;

		foreach (Direction direction in _gameManager.players[playerNum].sequence) {
			GameObject arrow = Instantiate (arrowPrefab, pos,
											Quaternion.Euler(
											new Vector3(0, 0, 90*(int)direction)))
											as GameObject;
			arrow.transform.parent = _gameManager.players [playerNum].arrows.transform;

			pos.x += arrowWidth + arrowGap;
		}
	}


	// Generate a sequence of arrows during game play
	public void GenerateSeq (int playerNum, int numKeys) {
		if (numKeys < minNum || numKeys > maxNum) {
			return;
		}

		// Destroy all the previous arrows
		foreach (Transform arrow in _gameManager.players [playerNum].arrows.transform) {
			Destroy (arrow.gameObject);
		}
		Debug.Log ("Done destroying");

		// Generate new arrows
		_gameManager.players [playerNum].seqLength = numKeys;
		_gameManager.players[playerNum].sequence = new Direction[numKeys];

		for (int i = 0; i < numKeys; i++) {
			_gameManager.players[playerNum].sequence[i] = mappings[Random.Range(0, 4)];
		}

		_gameManager.players [playerNum].currentKey = 0;

		Debug.Log ("Player 0's sequence: " + _gameManager.players [0].sequence[0] + " " +
											_gameManager.players [0].sequence[1] + " " +
											_gameManager.players [0].sequence[2] + " " +
											_gameManager.players [0].sequence[3] + " " +
											_gameManager.players [0].sequence[4] + " " +
											_gameManager.players [0].sequence[5]);

	}


	// Called by InputController
	// Checks if the correct key is pressed
	public void CheckKey (int player, Direction dir) 
    {

        if (_gameManager.players[player].sequence == null || _gameManager.players[player].sequence.Length == 0)
        {
            return;
        }

		bool pass = (dir == _gameManager.players [player].sequence [
								_gameManager.players [player].currentKey]);

		// Tell Player to turn into a pile of shit :D
		if (!pass) {
			Debug.Log ("Wrong key!");

			// Set animation
			_gameManager.players [player].arrows.GetComponentsInChildren<Animator>() [
								_gameManager.players [player].currentKey].SetBool(
								"Wrong", true);
			Debug.Log (_gameManager.players [player].arrows.GetComponentsInChildren<Animator> ().Length);
			_gameManager.players [player].ComboResult (false);
		
		} else {
			Debug.Log ("Right key!");

			// Set animation
			_gameManager.players [player].arrows.GetComponentsInChildren<Animator>() [
								_gameManager.players [player].currentKey].SetBool(
								"Correct", true);
			_gameManager.players [player].currentKey++;
		}
	}


	// Called by InputController
	// Submits the keys the player has pressed for that particular sequence
	public void LockIn (int playerNum) {
		if (_gameManager.players [playerNum].currentKey >= minReqKeys) {
			_gameManager.players [playerNum].ComboResult (true);
		} else {
			_gameManager.players [playerNum].ComboResult (false);
		}
	}

}
