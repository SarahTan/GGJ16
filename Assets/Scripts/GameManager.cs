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
    public bool paused { get; private set; }

    public const float BUILDING_Z_INDEX = 0;
    public const float PLAYERS_Z_INDEX = -1;

    public Vector3[] PLAYER_HERO_CENTER;

    public GameState gameState;

    // Handle UI
    Transform pauseCanvas;
    Transform endCanvas;
    Transform startCanvas;
    public string restartKey;

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

        pauseCanvas = GameObject.Find("PauseGame Canvas").transform;
        endCanvas = GameObject.Find("EndGame Canvas").transform;
        startCanvas = GameObject.Find("StartGame Canvas").transform;
        StartCoroutine(BlinkingStartGame());
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

        HideStartCanvas();
        
        foreach (Transform child in pauseCanvas) {
			child.gameObject.SetActive (false);
		}
    }

    public void gameOver(int loser)
    {
		endGame();

		// The BG overlay
		endCanvas.GetChild(0).gameObject.SetActive(true);

		// Player 1 and 2
		for (int i = 0; i < 2; i++) {
			GameObject p = endCanvas.GetChild (i + 1).gameObject;
			p.SetActive (true);

			if (i == loser) {
				p.GetComponent<Text> ().text = "You lose!";
			} else {
				p.GetComponent<Text> ().text = "You win!";
			}
		}
    }

    public void restart()
    {
        if (paused)
        {
            pauseGame();
        }
        startGame();
    }

    public void pauseGame()
    {
        if (paused)
        {   
            // The BG overlay
            pauseCanvas.GetChild(0).gameObject.SetActive(false);
            GameObject gMT = pauseCanvas.FindChild("main text").gameObject;
            GameObject sMT = pauseCanvas.FindChild("secondary text").gameObject;
            gMT.SetActive(false);
            sMT.SetActive(false);

            Time.timeScale = 1.0f;
            paused = false;
        }
        else
        {   
            // The BG overlay
            pauseCanvas.GetChild(0).gameObject.SetActive(true);
            GameObject gMT = pauseCanvas.FindChild("main text").gameObject;
            GameObject sMT = pauseCanvas.FindChild("secondary text").gameObject;
            sMT.GetComponent<Text>().text = "Press " + restartKey + " to restart";
            gMT.SetActive(true);
            sMT.SetActive(true);

            Time.timeScale = 0.0f;
            paused = true;
        }
    }

    public void endGame()
    {
        gameState = GameState.End;
    }

    private IEnumerator BlinkingStartGame() {
        Transform startText = startCanvas.FindChild("StartGame");
        if(startText != null) {
            while (gameState == GameState.Menu) {
                startText.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.5f);
                startText.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
            }
        }
        yield break;
    }
    private void HideStartCanvas() {
        StopCoroutine(BlinkingStartGame());
        foreach (Transform child in startCanvas) {
            child.gameObject.SetActive(false);
        }
    }

	// Update is called once per frame
	void Update () {

	}
}
