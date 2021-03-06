﻿using UnityEngine;
using System.Collections;

public class HeroManagerTest : MonoBehaviour {

    private HeroManager heroManager;

	// Use this for initialization
	void Start () {
        heroManager = new HeroManager();
        heroManager.init(0, new Vector3(-5, -2, 0));
        HeroManager heroManager2 = new HeroManager();
        heroManager.init(1, new Vector3(5, -2, 0));
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("Fire1")) {
            heroManager.SendOutHero();
        }
    }
}
