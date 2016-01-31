using UnityEngine;
using System.Collections;

public class BuildingManager : Singleton<BuildingManager> {

    private Building[,] buildings;

    private int _buildingCount;
    private float _buildingFootprint;
    private float _maxBuildingHeight;
    private float _minBuildingHeight;
    private int _globalHealth;

    public const int DEFAULT_BUILDING_COUNT = 15;
    public const int BUILDING_MULTIPLIER = 7;
    public const float DEFAULT_BUILDING_FOOTPRINT = 6;
    public const float DEFAULT_MAX_BUILDING_HEIGHT = 1.5f;
    public const float DEFAULT_MIN_BUILDING_FOOTPRINT = 1f;
    public const int GLOBAL_HEALTH = 4000;

    public Vector3 leftBuildingAnchor = new Vector3(-7.5f, 0.5f, 0);
    public Vector3 rightBuildingAnchor = new Vector3(7.5f, 0.5f, 0);
    public Vector3 leftBuildingBase = new Vector3(-3.5f, 0.5f, 0);
    public Vector3 rightBuildingBase = new Vector3(3.5f, 0.5f, 0);

	GameObject Player1Buildings;
	GameObject Player2Buildings;

    private int[] _damageDistribution;
    
    void Awake()
    {
		Player1Buildings = new GameObject ();
		Player1Buildings.name = "Player1 Buildings";
		Player2Buildings = new GameObject ();
		Player2Buildings.name = "Player2 Buildings";
        if (_buildingCount <= 0)
        {
            _buildingCount = DEFAULT_BUILDING_COUNT;
        }
        if (_buildingFootprint <= 0)
        {
            _buildingFootprint = DEFAULT_BUILDING_FOOTPRINT;
        }
        if (_maxBuildingHeight > 1 || _maxBuildingHeight <= _minBuildingHeight)
        {
            _maxBuildingHeight = DEFAULT_MAX_BUILDING_HEIGHT;
        }
        if (_minBuildingHeight <= 0.5 || _maxBuildingHeight <= _minBuildingHeight)
        {
            _minBuildingHeight = DEFAULT_MIN_BUILDING_FOOTPRINT;
        }
        if (_globalHealth < 100)
        {
            _globalHealth = GLOBAL_HEALTH;
        }

        buildings = new Building[2, _buildingCount];
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

    private void generateDamageDistribution()
    {
        _damageDistribution = new int[GLOBAL_HEALTH * 2];
        for (int i = 0; i < _damageDistribution.Length; i++)
        {
            //_damageDistribution[i] = Random.Range(
        }
    }

    // Use this for initialization
    void Start()
    {
        
	}

    public void generateBuildings()
    {
        if (_buildingCount == 1)
        {
            buildings[0, 0] = new Building(_maxBuildingHeight, _buildingFootprint, _globalHealth, false);
            buildings[0, 0].place(leftBuildingAnchor);
            buildings[1, 0] = new Building(_maxBuildingHeight, _buildingFootprint, _globalHealth, false);
            buildings[1, 0].place(rightBuildingAnchor);
        }
        else
        {
            int numberOfLayers = _buildingCount / (BUILDING_MULTIPLIER + 1) + 1;
            int buildingsPerLayer = _buildingCount / numberOfLayers;
            float buildingWidth = _buildingFootprint / buildingsPerLayer;

            float currentProgress = 0;
            for (int i = 0; i < _buildingCount / 2 + 1; i++)
            {
                bool flipped = (Random.RandomRange(1, 10) % 2 == 0);
                float currentBuildingWidth = Random.Range(buildingWidth * 1f, buildingWidth * 1.3f);
                float currentBuildingHeight = Random.Range(_minBuildingHeight * 0.7f, _maxBuildingHeight * 0.7f);
                float currentBuildingDepth = Random.Range(0f, 0.2f);
                GameObject bObj = Instantiate(Resources.Load("Prefabs/Building"), Vector3.zero, Quaternion.identity) as GameObject;
				bObj.transform.parent = Player1Buildings.transform;
				Building building = bObj.GetComponent<Building>();
                building.setProperties(currentBuildingHeight, currentBuildingWidth, _globalHealth / _buildingCount, flipped);
                buildings[0, i] = building;
                buildings[0, i].place(leftBuildingAnchor + currentProgress * Vector3.right + currentBuildingDepth * Vector3.forward + currentBuildingHeight * Vector3.up * 1.5f);
                GameObject bObj2 = Instantiate(Resources.Load("Prefabs/Building"), Vector3.zero, Quaternion.identity) as GameObject;
				bObj2.transform.parent = Player2Buildings.transform;
				Building building2 = bObj2.GetComponent<Building>();
                building2.setProperties(currentBuildingHeight, currentBuildingWidth, _globalHealth / _buildingCount, flipped);
                buildings[1, i] = building2;
                buildings[1, i].place(rightBuildingAnchor + currentProgress * Vector3.left + currentBuildingDepth * Vector3.forward + currentBuildingHeight * Vector3.up * 1.5f);
                currentProgress += currentBuildingWidth * 0.9f;
            }
            currentProgress = buildingWidth/2;
            for (int i = _buildingCount / 2 + 1; i < _buildingCount; i++)
            {
                bool flipped = (Random.RandomRange(1, 10) % 2 == 0);
                float currentBuildingDepth = Random.Range(0.25f, 0.45f);
                float currentBuildingWidth = Random.Range(buildingWidth * 1f, buildingWidth * 1.3f);
                float currentBuildingHeight = Random.Range(_maxBuildingHeight, _minBuildingHeight);
                GameObject bObj = Instantiate(Resources.Load("Prefabs/Building"), Vector3.zero, Quaternion.identity) as GameObject;
				bObj.transform.parent = Player1Buildings.transform;
				Building building = bObj.GetComponent<Building>();
                building.setProperties(currentBuildingHeight, currentBuildingWidth, _globalHealth / _buildingCount, flipped);
                buildings[0, i] = building;
                buildings[0, i].place(leftBuildingAnchor + currentProgress * Vector3.right + currentBuildingDepth * Vector3.forward + currentBuildingHeight * Vector3.up * 1.5f);
                GameObject bObj2 = Instantiate(Resources.Load("Prefabs/Building"), Vector3.zero, Quaternion.identity) as GameObject;
				bObj2.transform.parent = Player2Buildings.transform;
				Building building2 = bObj2.GetComponent<Building>();
                building2.setProperties(currentBuildingHeight, currentBuildingWidth, _globalHealth / _buildingCount, flipped);
                buildings[1, i] = building2;
                buildings[1, i].place(rightBuildingAnchor + currentProgress * Vector3.left + currentBuildingDepth * Vector3.forward + currentBuildingHeight * Vector3.up * 1.5f);
                currentProgress += currentBuildingWidth * 0.9f;
            }
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
