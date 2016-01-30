using UnityEngine;
using System.Collections;

public class HeroManagerTest : MonoBehaviour {

    private HeroManager heroManager;

	// Use this for initialization
	void Start () {
        heroManager = new HeroManager(0, new Vector3(-1, 0, 0));
        HeroManager heroManager2 = new HeroManager(1, new Vector3(1, 0, 0));
    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetButtonDown("Fire1")) {
            heroManager.SendOutHero(HeroManager.HERO_POWER.POWER_1);
        }
        if (Input.GetButtonDown("Fire2")) {
            heroManager.UpdatePose(ComboManager.Direction.UP);
        }
    }
}
