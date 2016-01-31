using UnityEngine;
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
        Player player1 = new Player(0);
        Player player2 = new Player(1);
        players = new Player[2];
        players[0] = player1;
        players[1] = player2;

        _config = Config.Instance;
        _buildingManager = BuildingManager.Instance;
        _fightSimulator = FightSimulator.Instance;
		_comboManager = ComboManager.Instance;
        _eventManager = EventManager.Instance;
		_soundManager = SoundManager.Instance;
    }

	// Use this for initialization
    void Start()
    {
        gameState = GameState.Menu;
	}

    public void startGame()
    {
        _buildingManager.generateBuildings();
        _comboManager.generateSeq(0, 8);
        _comboManager.generateSeq(1, 8);

        gameState = GameState.Playing;
		_soundManager.bgmPlay (BGMStage.BGM);
    }

    public void gameOver(int loser)
    {
		endGame();

		// Handle UI
		Transform canvas = GameObject.Find("EndGame Canvas").transform;

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
