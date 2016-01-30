using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

    public float height { get; private set; }
    public float width { get; private set; }
    public int health { get; private set; }
    public int totalHealth { get; private set; }

    GameObject buildingSprite;

    //temp
    int healthTiers = 4;

    public Building(float height, float width, int hp)
    {
        health = hp;
        totalHealth = health;
        buildingSprite = Instantiate(Resources.Load("Building")) as GameObject;
    }

    public void lowerHealth(int hp)
    {
        health -= hp;
        float percent = totalHealth / health;
        int tier = Mathf.CeilToInt(percent * healthTiers);

    }

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
