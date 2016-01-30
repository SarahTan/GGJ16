using UnityEngine;
using System.Collections;

public class BuildingManager : Singleton<BuildingManager> {

    private Building[,] buildings;

    private int _buildingCount;
    private float _buildingFootprint;
    
    public const int DEFAULT_BUILDING_COUNT = 5;
    public const float DEFAULT_BUILDING_FOOTPRINT = 5;

    public Vector3 leftBuildingAnchor = new Vector3(-8f, 2f, 0);
    public Vector3 rightBuildingAnchor = new Vector3(8f, 2f, 0);
    
    void Awake()
    {

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


        buildings = new Building[2,_buildingCount];

	}

    public void generateBuildings()
    {

    }

	// Update is called once per frame
	void Update () {
	
	}
}
