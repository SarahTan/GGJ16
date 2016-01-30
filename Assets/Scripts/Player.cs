using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    private ComboManager _comboManager;
    private InputController _inputController;
    private GameManager _gameManager;
    private BuildingManager _buildingManager;

    public int index { get; private set; }

	public int seqLength;
	public ComboManager.Direction[] sequence;
	public int currentKey;

	public Vector3 centerPos;
	public GameObject arrows;

    public HeroManager heroManager;

    private List<Hero> _heroList;

	public Player (int i) {
		index = i;
		_comboManager = ComboManager.Instance;
		_inputController = InputController.Instance;
        _gameManager = GameManager.Instance;
        _buildingManager = BuildingManager.Instance;

		arrows = new GameObject ();
		arrows.name = "Player" + (index+1) + " Arrows";

		Camera cam = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		if (index == 0) {
			centerPos = cam.ScreenToWorldPoint (new Vector3 (Screen.width / 4,
													Screen.height / 10, 9));
		} else if (index == 1) {
			centerPos = cam.ScreenToWorldPoint (new Vector3 (Screen.width / 4 * 3,
													Screen.height / 10, 9));
		}

        Debug.Log(_gameManager.PLAYER_HERO_CENTER[index]);
        heroManager = new HeroManager(index, _gameManager.PLAYER_HERO_CENTER[index]);

        _heroList = new List<Hero>();
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
    public void assignKeys(KeyCode[] keys)
    {
            _inputController.registerTrigger(() => triggerDirection(index, ComboManager.Direction.UP), keys[0]);
            _inputController.registerTrigger(() => triggerDirection(index, ComboManager.Direction.LEFT), keys[1]);
            _inputController.registerTrigger(() => triggerDirection(index, ComboManager.Direction.DOWN), keys[2]);
            _inputController.registerTrigger(() => triggerDirection(index, ComboManager.Direction.RIGHT), keys[3]);
            _inputController.registerTrigger(() => _comboManager.LockIn(index), keys[4]);
    }

    public void triggerDirection(int player, ComboManager.Direction dir)
    {
        Debug.Log("Trigger");
        _comboManager.CheckKey(player, dir);
        heroManager.UpdatePose(dir);
    }

    public void ComboResult(bool pass)
    {
		// currentKey is how many keys the player has gotten correct
		// If it's above the min req number (4), ComboManager sends true, else it sends false
		Debug.Log("Combo result: " + pass + "!");


		// Reset this
		currentKey = 0;
	}

    // Debug purposes

    public void powerUp()
    {
        heroManager.PowerUp(HeroManager.HERO_POWER.POWER_4);
    }

    public void deploy()
    {
        heroManager.SendOutHero();
    }

    public void attack()
    {
        if (index == 0) {
            _buildingManager.damageBuildings(1, 10); 
        } else {
            _buildingManager.damageBuildings(0, 10); 
        }
    }


}
