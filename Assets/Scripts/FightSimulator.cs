using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightSimulator : Singleton<FightSimulator> {

    private GameManager _gameManager;
    List<Hero> player1Heroes;
    List<Hero> player2Heroes;
    void Awake()
    {
        _gameManager = GameManager.Instance;
    }

	// Use this for initialization
	void Start () {
        player1Heroes = _gameManager.players[0].heroList;
        player2Heroes = _gameManager.players[1].heroList;


	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < player1Heroes.Count; i++)
        {
            if (!player1Heroes[i].fighting)
            {
                player1Heroes[i].move();

                Vector2 h1Pos = player1Heroes[i].transform.position.toVector2();
                for (int j = 0; j < player2Heroes.Count; j++)
                {
                    Vector2 h2Pos = player2Heroes[j].transform.position.toVector2();
                    if ((h1Pos - h2Pos).magnitude < 0.5f)
                    {
                        player1Heroes[i].fighting = true;
                        player1Heroes[i].target = player2Heroes[j];
                        player2Heroes[j].fighting = true;
                        player2Heroes[j].target = player1Heroes[i];
                    }
                }

            }
            else
            {
                player1Heroes[i].attack();
            }
        }

        for (int i = 0; i < player2Heroes.Count; i++)
        {
            if (!player2Heroes[i].fighting)
            {
                player2Heroes[i].move();

                Vector2 h2Pos = player2Heroes[i].transform.position.toVector2();
                for (int j = 0; j < player1Heroes.Count; j++)
                {
                    Vector2 h1Pos = player1Heroes[j].transform.position.toVector2();
                    if ((h1Pos - h2Pos).magnitude < 0.5f)
                    {
                        player1Heroes[j].fighting = true;
                        player1Heroes[j].target = player2Heroes[i];
                        player2Heroes[i].fighting = true;
                        player2Heroes[i].target = player2Heroes[j];
                    }
                }

            }
            else
            {
                player2Heroes[i].attack();
            }
        }

	}
}
