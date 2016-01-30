using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HeroManager {

    public enum HERO_TYPE {
        TYPE_1,
        TYPE_2,
        SIZE
    }
    public enum HERO_POWER {
        POWER_SHIT,
        POWER_1,
        POWER_2,
        POWER_3,
        POWER_4,
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

        MoveHeroPositions();
        _currentHero.MoveToCenter();
    }

    public void PowerUp(HERO_POWER heroPower)
    {
        _currentHero.PowerUp(_powerLevelList[(int)heroPower]);
    }

    public void SendOutHero() {
        // Send out the hero based on whether its a success or failure
        _currentHero.moveToPlayingField();
        _gameManager.players[_playerNum].heroList.Add(_currentHero);
        NextHero();
    }

    public void UpdatePose(ComboManager.Direction poseDirection) {
        if(_currentHero != null) {
            _currentHero.UpdatePose(poseDirection);
        }
    }
    
    private void NextHero() {
        // Remove first element in the list
        if(_heroList.Count > 0) {
            _currentHero = _heroList[0];
            _heroList.RemoveAt(0);
            _currentHero.MoveToCenter();
        }        
        AddHero(); // Add hero to the end of the queue
        MoveHeroPositions();
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
        return hero;
    }
}
