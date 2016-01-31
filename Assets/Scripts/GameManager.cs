﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager> {

	private ComboManager _comboManager;
    private BuildingManager _buildingManager;
    private FightSimulator _fightSimulator;
    private EventManager _eventManager;
    private Config _config;
	private SoundManager _soundManager;
    public Player[] players { get; private set; }

    public const float BUILDING_Z_INDEX = 0;
    public const float PLAYERS_Z_INDEX = -1;

    public Vector3[] PLAYER_HERO_CENTER;

    public GameState gameState;

	Transform canvas;

    public enum GameState
    {
        Menu,
        Ready,
        Playing,
        End,
        Pause
    }

    void Awake()
    {
        PLAYER_HERO_CENTER = new Vector3[]{new Vector3(-4, -1f, 0), new Vector3(4, -1f, 0) };
        Player player1 = new Player();
        Player player2 = new Player();
        players = new Player[2];
        players[0] = player1;
        players[1] = player2;

        _config = Config.Instance;
        _buildingManager = BuildingManager.Instance;
        _fightSimulator = FightSimulator.Instance;
		_comboManager = ComboManager.Instance;
        _eventManager = EventManager.Instance;
		_soundManager = SoundManager.Instance;

		canvas = GameObject.Find("EndGame Canvas").transform;
    }

	// Use this for initialization
    void Start()
    {
        gameState = GameState.Menu;
	}

    public void startGame()
    {
        players[0].init(0);
        players[1].init(1);
        _fightSimulator.startGame();
        _buildingManager.generateBuildings();
        _comboManager.generateSeq(0, 8);
		_comboManager.generateSeq(1, 8);
		_soundManager.bgmPlay (BGMStage.BGM);

        gameState = GameState.Playing;

		foreach (Transform child in canvas) {
			child.gameObject.SetActive (false);
		}
    }

    public void gameOver(int loser)
    {
		endGame();

		// The BG overlay
		canvas.GetChild(0).gameObject.SetActive(true);

		// Player 1 and 2
		for (int i = 0; i < 2; i++) {
			GameObject p = canvas.GetChild (i + 1).gameObject;
			p.SetActive (true);

			if (i == loser) {
				p.GetComponent<Text> ().text = "You lose!";
			} else {
				p.GetComponent<Text> ().text = "You win!";
			}
		}
    }

    public void endGame()
    {
        gameState = GameState.End;
    }

	// Update is called once per frame
	void Update () {

	}
}
