using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HeroManager {

    public enum HERO_TYPE {
        TYPE_1,
        TYPE_2,
        TYPE_3,
        TYPE_4,
        TYPE_5,
        SIZE
    }
    public enum HERO_POWER {
        POWER_SHIT,
        POWER_1,
        POWER_2,
        POWER_3,
        POWER_4,
        POWER_5,
        SIZE
    }

    private GameManager _gameManager;

    public static int HERO_LIMIT = 15;

    private List<Hero> _heroList;
    private Hero _currentHero;

    private Transform _heroParent;
    private GameObject[] _heroPrefab;
    private int[] _powerLevelList;

    private int _playerNum;
    private Vector3 _centerPos;

    public HeroManager(int playerNum, Vector3 centerPos) {
        _gameManager = GameManager.Instance;
        _playerNum = playerNum; // Determine which side to generate from
        _centerPos = centerPos;

        GameObject heroParent = new GameObject();
        heroParent.name = "Hero Parent";
        _heroParent = heroParent.transform;
        _heroParent.position = centerPos;
        // Player 2 should be inverted
        if (playerNum == 1) {
            _heroParent.localScale = new Vector3(-1, 1, 1);
        }

        _heroPrefab = new GameObject[(int)HERO_TYPE.SIZE];
        _heroPrefab[(int)HERO_TYPE.TYPE_1] = Resources.Load("Prefabs/Hero1") as GameObject;
        _heroPrefab[(int)HERO_TYPE.TYPE_2] = Resources.Load("Prefabs/Hero2") as GameObject;
        _heroPrefab[(int)HERO_TYPE.TYPE_3] = Resources.Load("Prefabs/Hero3") as GameObject;
        _heroPrefab[(int)HERO_TYPE.TYPE_4] = Resources.Load("Prefabs/Hero4") as GameObject;
        _heroPrefab[(int)HERO_TYPE.TYPE_5] = Resources.Load("Prefabs/Hero5") as GameObject;

        _powerLevelList = new int[(int)HERO_POWER.SIZE];
        _powerLevelList[(int)HERO_POWER.POWER_SHIT] = -1;
        _powerLevelList[(int)HERO_POWER.POWER_1] = 100;
        _powerLevelList[(int)HERO_POWER.POWER_2] = 120;
        _powerLevelList[(int)HERO_POWER.POWER_3] = 140;
        _powerLevelList[(int)HERO_POWER.POWER_4] = 180;
        _powerLevelList[(int)HERO_POWER.POWER_5] = 200;

        _heroList = new List<Hero>(HERO_LIMIT);
        _currentHero = GenerateHero();

        for(int i=0; i<HERO_LIMIT; i++) {
            AddHero();
        }

        _currentHero.SetToCenter();
    }

    public void PowerUp(HERO_POWER heroPower)
    {
        _currentHero.PowerUp((int)(_powerLevelList[(int)heroPower] * Random.Range(0.85f, 1.15f)));
    }
    public void UpdatePose(ComboManager.Direction poseDirection) {
        if (_currentHero != null) {
            _currentHero.UpdatePose(poseDirection, _playerNum);
        }
    }

    public void SendOutHero() {
        // Send out the hero based on whether its a success or failure
        if (_playerNum == 0)
        {
            _currentHero.moveToPlayingField(Hero.Side.LEFT);
        }
        else
        {
            _currentHero.moveToPlayingField(Hero.Side.RIGHT);
        }
        _gameManager.players[_playerNum].heroList.Add(_currentHero);
        NextHero();
    }    
    private void NextHero() {
        // Remove first element in the list
        if(_heroList.Count > 0) {
            _currentHero = _heroList[0];
            _heroList.RemoveAt(0);
            _currentHero.MoveToCenter();
        }
        MoveHeroPositions();
        AddHero(); // Add hero to the end of the queue        
    }
    private void MoveHeroPositions() {
        for (int i = 0; i < _heroList.Count; i++) {
            _heroList[i].MoveQueuePosition();
        }
    }
    private void AddHero() {
        _heroList.Add(GenerateHero());
    }
    private Hero GenerateHero() {
        int randomHeroNumber = Random.Range(0, (int)HERO_TYPE.SIZE);

        GameObject heroObject = Object.Instantiate(_heroPrefab[randomHeroNumber]) as GameObject;
        heroObject.transform.SetParent(_heroParent);        
        Hero hero = heroObject.GetComponent<Hero>();
		hero.Init(_heroList.Count);
        hero.SetQueuePosition();
        return hero;
    }
}
