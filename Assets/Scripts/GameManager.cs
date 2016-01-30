﻿using UnityEngine;
using System.Collections;

public class GameManager : Singleton<GameManager> {

    private BuildingManager _buildingManager;

    public Player[] players { get; private set; }

    public const float BUILDING_Z_INDEX = 0;
    public const float PLAYERS_Z_INDEX = -1;

    void Awake()
    {
        Player player1 = new Player();
        Player player2 = new Player();
        players = new Player[2];
        players[0] = player1;
        players[1] = player2;

        _buildingManager = BuildingManager.Instance;

    }

	// Use this for initialization
	void Start () {
        _buildingManager.generateBuildings();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
