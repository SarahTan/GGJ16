﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

    private ComboManager _comboManager;
    private InputController _inputController;
    private GameManager _gameManager;
    private BuildingManager _buildingManager;
	private SoundManager _soundManager;
    private EventManager _eventManager;

    public const float DEPLOY_TIME = 0.5f;

    public int index { get; private set; }

	public int seqLength;
	public ComboManager.Direction[] sequence;
	public int currentKey;

	public Vector3 centerPos;
	public GameObject arrows;

    public HeroManager heroManager;

    public List<Hero> heroList;

    public bool paused;

    public Player()
    {
		_soundManager = SoundManager.Instance;
		_comboManager = ComboManager.Instance;
		_inputController = InputController.Instance;
        _gameManager = GameManager.Instance;
        _buildingManager = BuildingManager.Instance;
        _eventManager = EventManager.Instance;
        heroManager = new HeroManager();
    }

    public void init(int i)
    {
        index = i;
        heroManager.init(index, _gameManager.PLAYER_HERO_CENTER[index]);
        heroList = new List<Hero>();

        paused = false;

        arrows = new GameObject();
        arrows.name = "Player" + (index + 1) + " Arrows";

        Camera cam = Camera.main;
        if (index == 0)
        {
            centerPos = new Vector3(-cam.orthographicSize * cam.aspect / 2,
                                     -cam.orthographicSize + 1, -5);
        }
        else if (index == 1)
        {
            centerPos = new Vector3(cam.orthographicSize * cam.aspect / 2,
                                     -cam.orthographicSize + 1, -5);
        }

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
	    _inputController.registerTrigger(() => triggerLockedIn(index), keys[4]);
    }

    public void triggerLockedIn(int player)
    {
        if (!paused)
        {
            if (_gameManager.gameState.Equals(GameManager.GameState.Playing))
            {
                _comboManager.LockIn(player);
            }
        }
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
        Debug.Log("Pass: " + pass);
        if (pass)
        {
			_soundManager.sfxPlay (SFXType.SYSTEM_EXTCORRECT);

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
                    heroManager.PowerUp(HeroManager.HERO_POWER.POWER_5);
                    break;
                default:
                    heroManager.PowerUp(HeroManager.HERO_POWER.POWER_SHIT);
                    break;
            }

        }
        else
        {
			_soundManager.sfxPlay (SFXType.SYSTEM_ERROR);
            heroManager.PowerUp(HeroManager.HERO_POWER.POWER_SHIT);
        }

         _eventManager.addEvent(deploy, Constants.HERO_SENDING_DELAY, true);

		// Reset this
		currentKey = 0;

	}

    public void powerUp()
    {
        heroManager.PowerUp(HeroManager.HERO_POWER.POWER_SHIT);
    }

    public void deploy()
    {
        _comboManager.generateSeq(index, seqLength);
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
