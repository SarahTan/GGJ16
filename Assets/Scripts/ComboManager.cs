using UnityEngine;
using System.Collections;

public class ComboManager : Singleton<ComboManager> {

	private GameManager _gameManager;

	int minNum = 4;
	int maxNum = 8;
	int numPlayers = 2;
	int minReqKeys = 1;

	void Awake() {
		_gameManager = GameManager.Instance;
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

		// Mapping of a player's keys
		if (_gameManager.players [playerNum].mapping == null) {
			KeyCode[] testMapping = new KeyCode[4] {KeyCode.LeftArrow, 
											KeyCode.RightArrow, 
											KeyCode.UpArrow, 
											KeyCode.DownArrow};
			AssignKeys (playerNum, testMapping);
			Debug.Log ("test");
		}

		// Generate a player's current sequence
		GenerateSeq (playerNum, 6);

		// Check if the key being pressed is correct
		KeyCode[] testInput = new KeyCode[6] {KeyCode.LeftArrow, 
								KeyCode.RightArrow, 
								KeyCode.UpArrow, 
								KeyCode.DownArrow, 
								KeyCode.DownArrow, 
								KeyCode.DownArrow};
		for (int i = 0; i < testInput.Length; i++) {
			if (i == _gameManager.players [playerNum].currentKey) {
				CheckKey (testInput [i]);
			} else {
				break;
			}
		}


		// Submit a sequence
		LockIn(playerNum);
	}


	void GenerateSeq (int playerNum, int numKeys) {
		if (numKeys < minNum || numKeys > maxNum) {
			return;
		}

		_gameManager.players[playerNum].sequence = new KeyCode[numKeys];
		for (int i = 0; i < numKeys; i++) {
			_gameManager.players[playerNum].sequence[i] = 
				_gameManager.players[playerNum].mapping[Random.Range(0, 4)];
		}

		_gameManager.players [playerNum].currentKey = 0;

		Debug.Log ("Player 0's sequence: " + _gameManager.players [0].sequence[0] + " " +
											_gameManager.players [0].sequence[1] + " " +
											_gameManager.players [0].sequence[2] + " " +
											_gameManager.players [0].sequence[3] + " " +
											_gameManager.players [0].sequence[4] + " " +
											_gameManager.players [0].sequence[5]);

	}


	public void CheckKey (KeyCode key) {
		for (int i = 0; i < numPlayers; i++) {
			if (_gameManager.players [i].KeyIsInMapping(key)) {
				bool pass = (key == _gameManager.players [i].sequence [
										_gameManager.players [i].currentKey]);

				// Tell Player to turn into a pile of shit :D
				if (!pass) {
					Debug.Log ("Wrong key!");
					_gameManager.players [i].ComboResult (false);
				} else {
					Debug.Log ("Right key!");
					_gameManager.players [i].currentKey++;
				}

				break;	// if it's in player 0, it won't be in player 1
			}
		}
	}

	public void LockIn (int playerNum) {
		if (_gameManager.players [playerNum].currentKey >= minReqKeys) {
			_gameManager.players [playerNum].ComboResult (true);
		} else {
			_gameManager.players [playerNum].ComboResult (false);
		}
	}

	public void AssignKeys (int playerNum, KeyCode[] mapping) {
		_gameManager.players [playerNum].mapping = new KeyCode[4];
		_gameManager.players [playerNum].mapping = mapping;

		Debug.Log ("Player " + playerNum + "'s keys: " + _gameManager.players [0].mapping[0] + " " +
														_gameManager.players [0].mapping[1] + " " +
														_gameManager.players [0].mapping[2] + " " +
														_gameManager.players [0].mapping[3]);
	}
}
