﻿using UnityEngine;
using System.Collections;

public class BuildingManager : Singleton<BuildingManager> {

    private Building[,] buildings;

    private int _buildingCount;
    private float _buildingFootprint;
    private float _maxBuildingHeight;
    private float _minBuildingHeight;
    private int _globalHealth;

    public const int DEFAULT_BUILDING_COUNT = 5;
    public const float DEFAULT_BUILDING_FOOTPRINT = 5;
    public const float DEFAULT_MAX_BUILDING_HEIGHT = 0.4f;
    public const float DEFAULT_MIN_BUILDING_FOOTPRINT = 0.15f;
    public const int GLOBAL_HEALTH = 1000;

    public Vector3 leftBuildingAnchor = new Vector3(-8f, 2f, 0);
    public Vector3 rightBuildingAnchor = new Vector3(8f, 2f, 0);
    
    void Awake()
    {

    }

    public void damageBuildings(int player, int damage)
    {
        for (int i = 0; i < _buildingCount; i++)
        {
            if (buildings[player,i].health > 0)
            {
                buildings[player, i].lowerHealth(damage);
                return;
            }
        }
    }

    public int getHealth(int player)
    {
        int totalHealth = 0;
        for (int i = 0; i < _buildingCount; i++)
        {
            totalHealth += buildings[player, i].health;
        }
        return totalHealth;
    }

    public void setBuildingHeights(float max, float min) 
    {
        _maxBuildingHeight = max;
        _minBuildingHeight = min;
    }

    public void setBuildingFootprint(float footprint)
    {

        if (footprint <= 0)
        {
            _buildingFootprint = DEFAULT_BUILDING_COUNT;
        }
        else
        {
            _buildingFootprint = footprint;
        }
    }

    public void setBuildingCount(int count)
    {
        if (count <= 0)
        {
            _buildingCount = DEFAULT_BUILDING_COUNT;
        }
        else
        {
            _buildingCount = count;
        }
    }

    public void setGlobalHealth(int hp)
    {
        _globalHealth = hp;
    }

	// Use this for initialization
	void Start () {
        if (_buildingCount <= 0)
        {
            _buildingCount = DEFAULT_BUILDING_COUNT;
        }
        if (_buildingFootprint <= 0)
        {
            _buildingFootprint = DEFAULT_BUILDING_FOOTPRINT;
        }
        if (_maxBuildingHeight <= 0.15 || _maxBuildingHeight <= _minBuildingHeight)
        {
            _maxBuildingHeight = DEFAULT_MAX_BUILDING_HEIGHT;
        }
        if (_minBuildingHeight >= 0.4 || _maxBuildingHeight <= _minBuildingHeight)
        {
            _minBuildingHeight = DEFAULT_MIN_BUILDING_FOOTPRINT;
        }
        if (_globalHealth < 100)
        {
            _globalHealth = GLOBAL_HEALTH;
        }

        buildings = new Building[2,_buildingCount];

	}

    public void generateBuildings()
    {
        float buildingWidth = _buildingFootprint / _buildingCount;
        if (_buildingCount == 1)
        {
            buildings[0,0] = new Building(_maxBuildingHeight, buildingWidth, _globalHealth);
            buildings[0, 0].place(leftBuildingAnchor);
            buildings[1,0] = new Building(_maxBuildingHeight, buildingWidth, _globalHealth);
            buildings[1, 0].place(rightBuildingAnchor);
        }
        else if (_buildingCount < 5)
        {
            float currentProgress = 0;
            for (int i = 0; i < _buildingCount; i++)
            {
                float currentBuildingWidth = Random.Range(buildingWidth * 1.1f, buildingWidth * 1.5f);
                float currentBuildingHeight = Random.Range(_maxBuildingHeight, _minBuildingHeight);
                float currentBuildingDepth = Random.Range(0f, 0.2f);
                buildings[0, i] = new Building(currentBuildingHeight, currentBuildingWidth, _globalHealth / _buildingCount);
                buildings[0, i].place(leftBuildingAnchor + currentProgress * Vector3.right + currentBuildingDepth * Vector3.forward);
                buildings[1, i] = new Building(currentBuildingHeight, currentBuildingWidth, _globalHealth / _buildingCount);
                buildings[1, i].place(rightBuildingAnchor + currentProgress * Vector3.left + currentBuildingDepth * Vector3.forward);
                currentProgress += currentBuildingWidth * 0.8f;
            }
        }
        else 
        {
            float currentProgress = 0;
            for (int i = 0; i < _buildingCount / 2 + 1; i++)
            {
                float currentBuildingWidth = Random.Range(buildingWidth * 1.1f, buildingWidth * 1.5f);
                float currentBuildingHeight = Random.Range(_minBuildingHeight * 0.7f, _maxBuildingHeight * 0.7f);
                float currentBuildingDepth = Random.Range(0f, 0.2f);
                buildings[0, i] = new Building(currentBuildingHeight, currentBuildingWidth, _globalHealth / _buildingCount);
                buildings[0, i].place(leftBuildingAnchor + currentProgress * Vector3.right + currentBuildingDepth * Vector3.forward);
                buildings[1, i] = new Building(currentBuildingHeight, currentBuildingWidth, _globalHealth / _buildingCount);
                buildings[1, i].place(rightBuildingAnchor + currentProgress * Vector3.left + currentBuildingDepth * Vector3.forward);
                currentProgress += currentBuildingWidth * 0.8f;
            }
            currentProgress = 0;
            for (int i = _buildingCount / 2 + 1; i < _buildingCount; i++)
            {
                float currentBuildingDepth = Random.Range(0.25f, 0.45f);
                float currentBuildingWidth = Random.Range(buildingWidth * 1.1f, buildingWidth * 1.5f);
                float currentBuildingHeight = Random.Range(_maxBuildingHeight, _minBuildingHeight);
                buildings[0, i] = new Building(currentBuildingHeight, currentBuildingWidth, _globalHealth / _buildingCount);
                buildings[0, i].place(leftBuildingAnchor + currentProgress * Vector3.right + currentBuildingDepth * Vector3.forward);
                buildings[1, i] = new Building(currentBuildingHeight, currentBuildingWidth, _globalHealth / _buildingCount);
                buildings[1, i].place(rightBuildingAnchor + currentProgress * Vector3.left + currentBuildingDepth * Vector3.forward);
                currentProgress += currentBuildingWidth * 0.8f;
            }
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
