using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightSimulator : Singleton<FightSimulator> {

    private GameManager _gameManager;
    private BuildingManager _buildingManager;

    public static float heroDamageMultiplier;

    List<Hero> player1Heroes;
    List<Hero> player2Heroes;
    void Awake()
    {
        _buildingManager = BuildingManager.Instance;
        _gameManager = GameManager.Instance;
    }

	// Use this for initialization
	void Start () {
        
	}

    public void startGame()
    {
        player1Heroes = _gameManager.players[0].heroList;
        player2Heroes = _gameManager.players[1].heroList;
        
    }

    public void checkBuildingHealth(int player)
    {
        float health = _buildingManager.getHealth(player);
        if (health <= 0)
        {
            _gameManager.gameOver(player);
        }
    }

	// Update is called once per frame
	void Update () {
        if (_gameManager.paused)
        {
            return;
        }
        switch(_gameManager.gameState) {
            case GameManager.GameState.Playing:
                for (int i = 0; i < player1Heroes.Count; i++)
                {
                    switch (player1Heroes[i].state)
                    {
                        case Hero.State.Dead:
                            _buildingManager.damageBuildings(0, (int)(player1Heroes[i].totalPowerLevel * heroDamageMultiplier));
                            player1Heroes.RemoveAt(i);
                            checkBuildingHealth(0);
                            break;
                        case Hero.State.Attacking:
                            player1Heroes[i].attackBuilding();
                            break;
                        case Hero.State.Fighting:
                            player1Heroes[i].attack();
                            break;
                        case Hero.State.Idle:
                            player1Heroes[i].move();
                            Vector2 h1Pos = player1Heroes[i].transform.position.toVector2();
                            
                            for (int j = 0; j < player2Heroes.Count; j++)
                            {
                                    Vector2 h2Pos = player2Heroes[j].transform.position.toVector2();
                                    if ((h1Pos - h2Pos).magnitude < 0.5f)
                                    {
                                        player1Heroes[i].state = Hero.State.Fighting;
                                        player1Heroes[i].target = player2Heroes[j];
                                        player2Heroes[j].state = Hero.State.Fighting;
                                        player2Heroes[j].target = player1Heroes[i];
                                    }
                            }

                            float xPos = _buildingManager.rightBuildingBase.x;
                            if (Mathf.Abs(h1Pos.x - xPos) < 0.5f)
                            {
                                player1Heroes[i].state = Hero.State.Attacking;
                            }

                            break;
                        case Hero.State.Moving:
                            break;
                        default:
                            break;
                    }
                }

                for (int i = 0; i < player2Heroes.Count; i++)
                {
                    switch (player2Heroes[i].state)
                    {
                        case Hero.State.Dead:
                            _buildingManager.damageBuildings(1, (int)(player2Heroes[i].totalPowerLevel * heroDamageMultiplier));
                            player2Heroes.RemoveAt(i);
                            checkBuildingHealth(1);
                            break;
                        case Hero.State.Attacking:
                            player2Heroes[i].attackBuilding();
                            break;
                        case Hero.State.Fighting:
                            player2Heroes[i].attack();
                            break;
                        case Hero.State.Idle:
                            player2Heroes[i].move();

                            Vector2 h2Pos = player2Heroes[i].transform.position.toVector2();
                            for (int j = 0; j < player1Heroes.Count; j++)
                            {
                                    Vector2 h1Pos = player1Heroes[j].transform.position.toVector2();
                                    if ((h1Pos - h2Pos).magnitude < 0.5f)
                                    {
                                        player1Heroes[j].state = Hero.State.Fighting;
                                        player1Heroes[j].target = player2Heroes[i];
                                        player2Heroes[i].state = Hero.State.Fighting;
                                        player2Heroes[i].target = player1Heroes[j];
                                    }
                            }
                            
                            float xPos = _buildingManager.leftBuildingBase.x;
                            if (Mathf.Abs(h2Pos.x - xPos) < 0.5f)
                            {
                                player2Heroes[i].state = Hero.State.Attacking;
                            }

                            break;
                        case Hero.State.Moving:
                            break;
                        default:
                            break;
                    }

                }
                break;
            default:
                break;
        }
	}
}
