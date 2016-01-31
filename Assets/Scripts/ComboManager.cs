using UnityEngine;
using System.Collections;

public class ComboManager : Singleton<ComboManager> {

	private GameManager _gameManager;
	private SoundManager _soundManager;

	int minNum = 4;
	int maxNum = 8;
	int numPlayers = 2;
	int minReqKeys = 1;

	GameObject arrowPrefab;
	float arrowWidth;
	float arrowGap = -0.03f;
	float bonusGap = 0.2f;

    GameObject keysBG;

    private Direction[] mappings = { Direction.UP, Direction.LEFT, Direction.DOWN, Direction.RIGHT }; 

    public enum Direction {
        UP,
        LEFT,
        DOWN,
        RIGHT
    }

	void Awake() {
		_gameManager = GameManager.Instance;
		_soundManager = SoundManager.Instance;

        keysBG = Instantiate(Resources.Load("Prefabs/KeysBG"),
                     new Vector3(0, -Camera.main.orthographicSize + 1, -4),
                     Quaternion.identity) as GameObject;
		arrowPrefab = Resources.Load ("Prefabs/UI arrow") as GameObject;
		arrowWidth = arrowPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
	}
    
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}
		

	// Draw the arrow keys on screen
	void DrawArrows (int playerNum) {
		Vector3 pos = _gameManager.players [playerNum].centerPos;
		pos.x -= ((arrowWidth * (_gameManager.players[playerNum].seqLength-1)) + (arrowGap * 3) + 
				 bonusGap * (_gameManager.players [playerNum].seqLength-4)) / 2;

		int i = 1;
		foreach (Direction direction in _gameManager.players[playerNum].sequence) {
			GameObject arrow = Instantiate (arrowPrefab, pos,
											Quaternion.Euler(
											new Vector3(0, 0, 90*(int)direction)))
											as GameObject;
			arrow.transform.parent = _gameManager.players [playerNum].arrows.transform;

			if (i >= 4) {
				pos.x += arrowWidth + bonusGap;

				if (i > 4) {
					arrow.GetComponent<Animator> ().SetBool ("Bonus", true);
				}
			} else {
				pos.x += arrowWidth + arrowGap;
			}
			i++;
		}
	}

	public void generateSeq(int playerNum, int numKeys) 
	{
		StartCoroutine (GenerateSeq (playerNum, numKeys));
	}

	// Generate a sequence of arrows during game play
	IEnumerator GenerateSeq (int playerNum, int numKeys) {
		if (numKeys < minNum || numKeys > maxNum) {
			Debug.LogError ("You can only generate between 4-8 keys!");
			yield break;
		}

		// Destroy all the previous arrows
		foreach (Transform arrow in _gameManager.players [playerNum].arrows.transform) {
			Destroy (arrow.gameObject);
		}
		yield return null;	// Because actual destruction only takes place at end of frame

		// Generate new arrows
		_gameManager.players [playerNum].seqLength = numKeys;
		_gameManager.players [playerNum].sequence = new Direction[numKeys];

		for (int i = 0; i < numKeys; i++) {
			_gameManager.players [playerNum].sequence [i] = mappings [Random.Range (0, 4)];
		}

		_gameManager.players [playerNum].currentKey = 0;
		DrawArrows (playerNum);
	}


	// Called by InputController
	// Checks if the correct key is pressed
	public void CheckKey (int playerNum, Direction dir) {
        if (_gameManager.players[playerNum].sequence == null ||
				_gameManager.players[playerNum].sequence.Length == 0) {
            return;
        }
		// They finished it but pressed wrong key instead of locking in
		if (_gameManager.players [playerNum].currentKey >=
				_gameManager.players [playerNum].seqLength) {
			_gameManager.players[playerNum].ComboResult(false);
			return;
		}

		bool pass = (dir == _gameManager.players [playerNum].sequence [
								_gameManager.players [playerNum].currentKey]);

		// Tell Player to turn into a pile of shit :D
		Animator[] anim = _gameManager.players [playerNum].arrows.
							GetComponentsInChildren<Animator> ();
		if (anim.Length > 0) {
			if (!pass) {
				anim[_gameManager.players [playerNum].currentKey].SetBool("Wrong", true);
				_gameManager.players [playerNum].ComboResult (false);

			} else {
				anim[_gameManager.players [playerNum].currentKey].SetBool("Correct", true);			
				_gameManager.players [playerNum].currentKey++;
			}
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
