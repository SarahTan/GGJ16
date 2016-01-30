﻿using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

    public Player[] players { get; private set; }

    void Awake()
    {
        Player player1 = new Player();
        Player player2 = new Player();
        players = new Player[2];
        players[0] = player1;
        players[1] = player2;

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}