using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    private ComboManager _comboManager;
    private InputController _inputController;
    private GameManager _gameManager;
    private BuildingManager _buildingManager;

    public const float DEPLOY_TIME = 0.5f;

    public int index { get; private set; }

	public int seqLength;
	public ComboManager.Direction[] sequence;
	public int currentKey;

	public Vector3 centerPos;
	public GameObject arrows;

    public HeroManager heroManager;

    public List<Hero> heroList;

    private bool paused;

	public Player (int i) {
        paused = false;
		index = i;
		_comboManager = ComboManager.Instance;
		_inputController = InputController.Instance;
        _gameManager = GameManager.Instance;
        _buildingManager = BuildingManager.Instance;

		arrows = new GameObject ();
		arrows.name = "Player" + (index+1) + " Arrows";

		Camera cam = Camera.main;
		if (index == 0) {
			centerPos = new Vector3 (-cam.orthographicSize * cam.aspect / 2,
									 -cam.orthographicSize + 1, -5);
		} else if (index == 1) {
			centerPos = new Vector3 (cam.orthographicSize * cam.aspect / 2,
									 -cam.orthographicSize + 1, -5);
		}

        heroManager = new HeroManager(index, _gameManager.PLAYER_HERO_CENTER[index]);

        heroList = new List<Hero>();
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void addHero(Hero hero)
    {
        heroList.Add(hero);
    }

    public void assignKeys(KeyCode[] keys)
    {
        _inputController.registerTrigger(() => triggerDirection(index, ComboManager.Direction.UP), keys[0]);
        _inputController.registerTrigger(() => triggerDirection(index, ComboManager.Direction.DOWN), keys[1]);
        _inputController.registerTrigger(() => triggerDirection(index, ComboManager.Direction.LEFT), keys[2]);
        _inputController.registerTrigger(() => triggerDirection(index, ComboManager.Direction.RIGHT), keys[3]);
	    _inputController.registerTrigger(() => _comboManager.LockIn(index), keys[4]);
    }

    public void triggerDirection(int player, ComboManager.Direction dir)
    {
        if (!paused)
        {
            if (_gameManager.gameState.Equals(GameManager.GameState.Playing))
            {
                _comboManager.CheckKey(player, dir);
                heroManager.UpdatePose(dir);
            }
        }
    }

    public void ComboResult(bool pass)
    {
		// currentKey is how many keys the player has gotten correct
        // If it's above the min req number (4), ComboManager sends true, else it sends false
        paused = true;
        if (pass)
        {
            
            switch (currentKey)
            {
                case 4:
                    heroManager.PowerUp(HeroManager.HERO_POWER.POWER_1);
                    break;
                case 5:
                    heroManager.PowerUp(HeroManager.HERO_POWER.POWER_2);
                    break;
                case 6:
                    heroManager.PowerUp(HeroManager.HERO_POWER.POWER_3);
                    break;
                case 7:
                    heroManager.PowerUp(HeroManager.HERO_POWER.POWER_4);
                    break;
                case 8:
                    heroManager.PowerUp(HeroManager.HERO_POWER.POWER_4);
                    break;
                default:
                    heroManager.PowerUp(HeroManager.HERO_POWER.POWER_1);
                    break;
            }

        }
        else
        {
            heroManager.PowerUp(HeroManager.HERO_POWER.POWER_SHIT);
        }

        deploy();
        //Invoke("deploy", DEPLOY_TIME);

		// Reset this
		currentKey = 0;
		if (pass) {
			// TODO: change seqLength according to whatever pattern
			_comboManager.generateSeq (index, seqLength);	
		} else {
			_comboManager.generateSeq (index, seqLength);
		}

	}

    // Debug purposes

    public void powerUp()
    {
        heroManager.PowerUp(HeroManager.HERO_POWER.POWER_4);
    }

    public void deploy()
    {
        heroManager.SendOutHero();
        paused = false;
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
