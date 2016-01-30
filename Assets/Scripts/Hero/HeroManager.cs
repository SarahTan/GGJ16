using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HeroManager {

    public enum HERO_POWER {
        POWER_SHIT,
        POWER_1,
        POWER_2,
        POWER_3,
        POWER_4,
        SIZE
    }

    private static int HERO_LIMIT = 5;

    private List<Hero> _heroList;
    private Hero _currentHero;
    private GameObject[] _heroPrefab;
    private int[] _powerLevelList;

    public HeroManager() {
        _heroPrefab = new GameObject[(int)Hero.HERO_TYPE.SIZE];
        _heroPrefab[(int)Hero.HERO_TYPE.TYPE_A] = Resources.Load("Prefabs/HeroA") as GameObject;
        _heroPrefab[(int)Hero.HERO_TYPE.TYPE_B] = Resources.Load("Prefabs/HeroA") as GameObject;

        _powerLevelList = new int[(int)HERO_POWER.SIZE];
        _powerLevelList[(int)HERO_POWER.POWER_SHIT] = -1;
        _powerLevelList[(int)HERO_POWER.POWER_1] = 10;
        _powerLevelList[(int)HERO_POWER.POWER_2] = 20;
        _powerLevelList[(int)HERO_POWER.POWER_3] = 40;
        _powerLevelList[(int)HERO_POWER.POWER_4] = 80;

        _heroList = new List<Hero>(HERO_LIMIT);
        _currentHero = GenerateHero();

        for(int i=0; i<HERO_LIMIT; i++) {
            AddHero();
        }
    }

    public void SendOutHero(HERO_POWER heroPower) {
        // Send out the hero based on whether its a success or failure
        _currentHero.PowerUp(_powerLevelList[(int)heroPower]);
        NextHero();
    }

    public void UpdatePose(Hero.HERO_POSE heroPose) {
        if(_currentHero != null) {
            _currentHero.UpdatePose(heroPose);
        }
    }

    private void NextHero() {
        // Remove first element in the list
        if(_heroList.Count > 0) {
            _currentHero = _heroList[0];
            _heroList.RemoveAt(0);
        }

        for(int i=0; i<_heroList.Count; i++) {
            _heroList[i].MoveQueuePosition();
        }

        AddHero(); // Add hero to the end of the queue
    }
    private void AddHero() {
        _heroList.Add(GenerateHero());
    }
    private Hero GenerateHero() {
        int randomHeroNumber = Random.Range(0, (int)Hero.HERO_TYPE.SIZE);

        GameObject heroObject = Object.Instantiate(_heroPrefab[randomHeroNumber]) as GameObject;
        Hero hero = heroObject.GetComponent<Hero>();
		hero.Init(_heroList.Count);
        return hero;
    }
}
