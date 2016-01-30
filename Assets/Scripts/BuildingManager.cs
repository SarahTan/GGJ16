using UnityEngine;
using System.Collections;

public class BuildingManager : Singleton<BuildingManager> {

    private Building[][] buildings;

    private int _buildingCount;

    public const int DEFAULT_BUILDING_COUNT = 5;

    void Awake()
    {
        
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



	}
	


	// Update is called once per frame
	void Update () {
	
	}
}
