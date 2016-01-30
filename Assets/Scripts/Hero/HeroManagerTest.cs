using UnityEngine;
using System.Collections;

public class HeroManagerTest : MonoBehaviour {

    private HeroManager heroManager;

	// Use this for initialization
	void Start () {
        heroManager = new HeroManager(0, new Vector3(-5, -2, 0));
        HeroManager heroManager2 = new HeroManager(1, new Vector3(5, -2, 0));
    }
	
	// Update is called once per frame
	void Update () {
    }
}
