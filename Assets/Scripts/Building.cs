using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour {

    public float height { get; private set; }
    public float width { get; private set; }
    public int health { get; private set; }
    public int totalHealth { get; private set; }
    
    public GameObject[] buildingStates;

    public Building(float h, float w, int hp, bool flipped)
    {
        health = hp;
        totalHealth = health;
        height = h;
        width = w;
        if (flipped)
        {
            transform.localScale = new Vector3(-w, h, 1);
        }
        else
        {
            transform.localScale = new Vector3(w, h, 1);
        }
        setState(0);
    }

    public void setProperties(float h, float w, int hp, bool flipped)
    {
        health = hp;
        totalHealth = health;
        height = h;
        width = w;
        if (flipped)
        {
            transform.localScale = new Vector3(-w, h, 1);
        }
        else
        {
            transform.localScale = new Vector3(w, h, 1);
        }
        setState(0);
    }

    private void setState(int state)
    {
        for (int i = 0; i < buildingStates.Length; i++)
        {
            if (i == state)
            {
                buildingStates[i].SetActive(true);
            }
            else
            {
                buildingStates[i].SetActive(false);
            }
        }
    }

    public void place(Vector3 pos)
    {
        transform.position = pos;
    }

    public void lowerHealth(int hp)
    {
        Debug.Log("Health: " + health + ", total: " + totalHealth);
        health -= hp;
        if (health < 0)
        {
            health = 0;
        }
        float percent = 1.0f * health / totalHealth;
        Debug.Log("Percent: " + percent);
        int tier = Mathf.CeilToInt(percent * (buildingStates.Length - 1));
        setState((buildingStates.Length - 1) - tier);
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
