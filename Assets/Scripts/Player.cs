using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    private ComboManager _comboManager;
    private InputController _inputController;

    public int index { get; private set; }

	public ComboManager.Direction[] sequence;
	public int currentKey;

	public Vector3 arrowKeysPos;
	public GameObject arrows;

	public Player (int i) {
		index = i;
		_comboManager = ComboManager.Instance;
		_inputController = InputController.Instance;

		arrows = new GameObject ();
		arrows.name = "Player" + (index+1) + " Arrows";

		if (index == 0) {
			arrowKeysPos = Vector3.zero;
		} else if (index == 1) {

		}
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
    public void assignKeys(KeyCode[] keys)
    {
            _inputController.registerTrigger(() => _comboManager.CheckKey(index, ComboManager.Direction.UP), keys[0]);
            _inputController.registerTrigger(() => _comboManager.CheckKey(index, ComboManager.Direction.DOWN), keys[1]);
            _inputController.registerTrigger(() => _comboManager.CheckKey(index, ComboManager.Direction.LEFT), keys[2]);
            _inputController.registerTrigger(() => _comboManager.CheckKey(index, ComboManager.Direction.RIGHT), keys[3]);        
    }


    public void ComboResult(bool pass)
    {
		// currentKey is how many keys the player has gotten correct
		// If it's above the min req number (4), ComboManager sends true, else it sends false
		Debug.Log("Combo result: " + pass + "!");


		// Reset this
		currentKey = 0;
	}
}
