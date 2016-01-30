using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

    public float height { get; private set; }
    public float width { get; private set; }
    public int health { get; private set; }
    public int totalHealth { get; private set; }
    
    public GameObject[] buildingStates;

    public Building(float h, float w, int hp)
    {
        health = hp;
        totalHealth = health;
        height = h;
        width = w;
        transform.localScale = new Vector3(w, h, 1);
    }

    public void setProperties(float h, float w, int hp)
    {
        health = hp;
        totalHealth = health;
        height = h;
        width = w;
        transform.localScale = new Vector3(w, h, 1);
    }

    public void place(Vector3 pos)
    {
        transform.position = pos;
    }

    public void lowerHealth(int hp)
    {
        health -= hp;
        if (health < 0)
        {
            health = 0;
        }
        float percent = health / totalHealth;
        int tier = Mathf.CeilToInt(percent * buildingStates.Length);

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
