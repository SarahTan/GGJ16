using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

	private ComboManager _comboManager;
    private BuildingManager _buildingManager;
    private FightSimulator _fightSimulator;
    private Config _config;

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
    }

    public void gameOver(int player)
    {
        endGame();
    }

    public void endGame()
    {
        gameState = GameState.End;
    }

	// Update is called once per frame
	void Update () {

	}
}
