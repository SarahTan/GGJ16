using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightSimulator : Singleton<FightSimulator> {

    private GameManager _gameManager;
    private BuildingManager _buildingManager;

    public static float heroDamageMultiplier;

	GameObject[] cityHealth;
	Vector3[] healthScale;
	float halfScreenWidth;
	float healthFactor;

    private bool spawnedGodzilla;
    public Godzilla godzilla;
    private float rollCooldown = 5f;
    private float lastRollTime;
    private bool spawnedBefore;

    List<Hero> player1Heroes;
    List<Hero> player2Heroes;
    void Awake()
    {
        _buildingManager = BuildingManager.Instance;
        _gameManager = GameManager.Instance;

		halfScreenWidth = Camera.main.orthographicSize * Camera.main.aspect;
		GameObject healthBar = Resources.Load ("Prefabs/HealthBar") as GameObject;
		Vector3 scale = healthBar.transform.localScale;
		scale.x = halfScreenWidth /
			healthBar.GetComponent<SpriteRenderer> ().sprite.bounds.size.x;
		healthBar.transform.localScale = scale;

		GameObject healthBar1 = Instantiate (healthBar,
			                    new Vector3 (-halfScreenWidth / 2, -3, -5),
								Quaternion.identity) as GameObject;
		GameObject healthBar2 = Instantiate (healthBar,
								new Vector3 (halfScreenWidth / 2, -3, -5),
								Quaternion.identity) as GameObject;
		cityHealth = new GameObject[2] {healthBar1, healthBar2};
		healthScale = new Vector3[2] { healthBar1.transform.localScale, healthBar2.transform.localScale };
        lastRollTime = Time.time;
    }

	// Use this for initialization
	void Start () {
        
	}

    public void startGame()
    {
        spawnedBefore = false;
        player1Heroes = _gameManager.players[0].heroList;
        player2Heroes = _gameManager.players[1].heroList;

		cityHealth [0].transform.localScale = healthScale[0];
		cityHealth [1].transform.localScale = healthScale[1];
    }

    public void checkBuildingHealth(int player)
    {
        float health = _buildingManager.getHealth(player);
        if (health <= 0)
        {
            _gameManager.gameOver(player);
        }
			
		Vector3 scale = new Vector3 (health / BuildingManager.GLOBAL_HEALTH, 1f, 1f);
		cityHealth [player].transform.localScale = scale;
    }

    public void spawnGodzilla(Hero.Side side)
    {
        GameObject gObj = Instantiate(Resources.Load("Prefabs/Godzilla"), Vector3.zero, Quaternion.identity) as GameObject;
        godzilla = gObj.GetComponent<Godzilla>();
        if (side.Equals(Hero.Side.LEFT))
        {
            godzilla.place(_buildingManager.leftBuildingAnchor + Vector3.up * 2, side);
        }
        else
        {
            godzilla.place(_buildingManager.rightBuildingAnchor + Vector3.up * 2, side);
        }
    }

    public int getHealthDiscrepancy()
    {
        int health1 = _buildingManager.getHealth(0);
        int health2 = _buildingManager.getHealth(1);
        return health1 - health2;
    }

	// Update is called once per frame
	void Update () {
        if (_gameManager.paused)
        {
            return;
        }
        switch(_gameManager.gameState) {
            case GameManager.GameState.Playing:
                //Godzilla stuff
                if (!spawnedBefore && !spawnedGodzilla && lastRollTime + rollCooldown < Time.time)
                {
                    lastRollTime = Time.time;
                    int discrepancy = getHealthDiscrepancy();
                    int totalHealth = BuildingManager.GLOBAL_HEALTH;
                    float percent = 1.0f * Mathf.Abs(discrepancy) / totalHealth;
                    Debug.Log("PERCENT: " + percent);
                    if (percent > 0.3)
                    {
                        float rand = Random.Range(0, 1);
                        if (rand < percent)
                        {
                            if (discrepancy < 0)
                            {
                                spawnGodzilla(Hero.Side.LEFT);
                                Debug.Log("Spawned on Left");
                            }
                            else
                            {
                                spawnGodzilla(Hero.Side.RIGHT);
                                Debug.Log("Spawned on right");
                            }
                            spawnedBefore = true;
                            spawnedGodzilla = true;
                        }
                    }
                }

                if (spawnedGodzilla)
                {
                    switch (godzilla.state)
                    {
                        case Hero.State.Dead:
                            godzilla = null;
                            spawnedGodzilla = false;
                            break;
                        case Hero.State.Attacking:
                            godzilla.attackBuilding();
                            break;
                        case Hero.State.Fighting:
                            godzilla.attack();
                            break;
                        default:
                        case Hero.State.Idle:
                            godzilla.move();
                            Vector2 gPos = godzilla.transform.position.toVector2();

                            if (godzilla.side.Equals(Hero.Side.LEFT))
                            {

                                for (int j = 0; j < player2Heroes.Count; j++)
                                {
                                    Vector2 h2Pos = player2Heroes[j].transform.position.toVector2();
                                    if (Mathf.Abs(gPos.x - h2Pos.x) < 0.5f)
                                    {
                                        godzilla.state = Hero.State.Fighting;
                                        godzilla.target = player2Heroes[j];
                                        player2Heroes[j].state = Hero.State.Fighting;
                                        player2Heroes[j].target = godzilla;
                                    }
                                }

                                float xPos = _buildingManager.rightBuildingBase.x;
                                if (Mathf.Abs(gPos.x - xPos) < 0.5f)
                                {
                                    godzilla.state = Hero.State.Attacking;
                                }

                            }
                            else
                            {

                                for (int j = 0; j < player1Heroes.Count; j++)
                                {
                                    Vector2 h1Pos = player1Heroes[j].transform.position.toVector2();
                                    if (Mathf.Abs(gPos.x - h1Pos.x) < 0.5f)
                                    {
                                        godzilla.state = Hero.State.Fighting;
                                        godzilla.target = player1Heroes[j];
                                        player1Heroes[j].state = Hero.State.Fighting;
                                        player1Heroes[j].target = godzilla;
                                    }
                                }

                                float xPos = _buildingManager.leftBuildingBase.x;
                                if (Mathf.Abs(gPos.x - xPos) < 0.5f)
                                {
                                    godzilla.state = Hero.State.Attacking;
                                }

                            }


                            break;
                        case Hero.State.Moving:
                            break;
                    }
                }

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

                            if (spawnedGodzilla)
                            {
                                Vector2 gPos = godzilla.transform.position;
                                if (Mathf.Abs(gPos.x - h1Pos.x) < 0.75f)
                                {
                                    player1Heroes[i].state = Hero.State.Fighting;
                                    player1Heroes[i].target = godzilla;
                                    godzilla.state = Hero.State.Fighting;
                                    godzilla.target = player1Heroes[i];
                                    break;
                                }
                            }

                            for (int j = 0; j < player2Heroes.Count; j++)
                            {
                                    Vector2 h2Pos = player2Heroes[j].transform.position.toVector2();
                                    if (Mathf.Abs(h1Pos.x - h2Pos.x) < 0.5f)
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

                            if (spawnedGodzilla)
                            {
                                Vector2 gPos = godzilla.transform.position;
                                if (Mathf.Abs(gPos.x - h2Pos.x) < 0.75f)
                                {
                                    player2Heroes[i].state = Hero.State.Fighting;
                                    player2Heroes[i].target = godzilla;
                                    godzilla.state = Hero.State.Fighting;
                                    godzilla.target = player2Heroes[i];
                                    break;
                                }
                            }

                            for (int j = 0; j < player1Heroes.Count; j++)
                            {
                                    Vector2 h1Pos = player1Heroes[j].transform.position.toVector2();
                                    if (Mathf.Abs(h1Pos.x - h2Pos.x) < 0.5f)
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
