using UnityEngine;
using System.Collections;

public class ComboManager : Singleton<ComboManager> {

	private GameManager _gameManager;

	int minNum = 4;
	int maxNum = 8;
	int numPlayers = 2;

	void Awake() {
		_gameManager = GameManager.Instance;
	}

	// Use this for initialization
	void Start () {
		Invoke ("Test", 1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// For testing
	void Test() {
		int playerNum = 0;

		// Mapping of a player's keys
		KeyCode[] temp = new KeyCode[4]{KeyCode.LeftArrow, 
										KeyCode.RightArrow, 
										KeyCode.UpArrow, 
										KeyCode.DownArrow};
		AssignKeys (playerNum, temp);

		// Generate a player's current sequence
		GenerateSeq (playerNum, 6);

		// Check if the key being pressed is correct
		temp = new KeyCode[6] {KeyCode.LeftArrow, 
								KeyCode.RightArrow, 
								KeyCode.UpArrow, 
								KeyCode.DownArrow, 
								KeyCode.DownArrow, 
								KeyCode.DownArrow};
		for (int i = 0; i < temp.Length; i++) {
			CheckKey (temp [i]);
		}
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
					_gameManager.players [i].ComboResult (pass);
				} else {
					Debug.Log ("Right key!");
					_gameManager.players [i].currentKey++;
				}

				break;	// if it's in player 0, it won't be in player 1
			}
		}
	}

	public void LockIn (int playerNum) {

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
