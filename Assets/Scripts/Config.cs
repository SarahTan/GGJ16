using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;

public class Config : Singleton<Config> {
    public string filename = "config.ini";

    private InputController _inputController;
    private GameManager _gameManager;
    private BuildingManager _buildingManager;

    void Awake()
    {
        //Initialize Singletons
        _inputController = InputController.Instance;
        _gameManager = GameManager.Instance;
        _buildingManager = BuildingManager.Instance;

        loadFile(filename);
        // Other stuff 
    }

    void loadFile(string filename)
    {
        if (!File.Exists(filename))
        {
            File.CreateText(filename);
            return;
        }

        try
        {
            string line;
            StreamReader sReader = new StreamReader(filename, Encoding.Default);
            do
            {
                line = sReader.ReadLine();
                if (line != null)
                {
                    // Lines with # are for comments
                    if (!line.Contains("#"))
                    {
                        // Value property identified by string before the colon.
                        string[] data = line.Split(':');
                        if (data.Length == 2)
                        {
                            switch (data[0].Trim().ToLower())
                            {
                                case "restart key":
                                    _gameManager.restartKey = data[1].Trim();
                                    KeyCode restart = _inputController.parseString(_gameManager.restartKey);
                                    _inputController.registerTrigger(_gameManager.restart, restart);
                                    break;
                                case "pause key":
                                    KeyCode pause = _inputController.parseString(data[1].Trim());
                                    _inputController.registerTrigger(_gameManager.pauseGame, pause);
                                    break;
                                case "start key":
                                    KeyCode start = _inputController.parseString(data[1].Trim());
                                    _inputController.registerTrigger(_gameManager.startGame, start);
                                    break;
                                case "player 1":
                                    String[] keys1 = data[1].Trim().Split(',');
                                    KeyCode[] codes1 = _inputController.parseKeys(keys1);
                                    _gameManager.players[0].assignKeys(codes1);
                                    break;
                                case "player 2":
                                    String[] keys2 = data[1].Trim().Split(',');
                                    KeyCode[] codes2 = _inputController.parseKeys(keys2);
                                    _gameManager.players[1].assignKeys(codes2);
                                    break;
                                case "building count":
                                    int buildingCount = int.Parse(data[1].Trim());
                                    _buildingManager.setBuildingCount(buildingCount);
                                    break;
                                case "building footprint":
                                    int buildingFootprint = int.Parse(data[1].Trim());
                                    _buildingManager.setBuildingFootprint(buildingFootprint);
                                    break;
                                case "player 1 power up":
                                    KeyCode p1p = _inputController.parseString(data[1].Trim());
                                    _inputController.registerTrigger(_gameManager.players[0].powerUp, p1p);
                                    break;
                                case "player 1 deploy":
                                    KeyCode p1d = _inputController.parseString(data[1].Trim());
                                    _inputController.registerTrigger(_gameManager.players[0].deploy, p1d);
                                    break;
                                case "player 1 attack":
                                    KeyCode p1a = _inputController.parseString(data[1].Trim());
                                    _inputController.registerTrigger(_gameManager.players[0].attack, p1a);
                                    break;
                                case "player 2 power up":
                                    KeyCode p2p = _inputController.parseString(data[1].Trim());
                                    _inputController.registerTrigger(_gameManager.players[1].powerUp, p2p);
                                    break;
                                case "player 2 deploy":
                                    KeyCode p2d = _inputController.parseString(data[1].Trim());
                                    _inputController.registerTrigger(_gameManager.players[1].deploy, p2d);
                                    break;
                                case "player 2 attack":
                                    KeyCode p2a = _inputController.parseString(data[1].Trim());
                                    _inputController.registerTrigger(_gameManager.players[1].attack, p2a);
                                    break;
                                case "hero power shit":
                                    int powerShit = int.Parse(data[1].Trim());
                                    Constants.HERO_POWER_SHIT = powerShit;
                                    break;
                                case "hero power 1":
                                    int powerLevel1 = int.Parse(data[1].Trim());
                                    Constants.HERO_POWER_1 = powerLevel1;
                                    break;
                                case "hero power 2":
                                    int powerLevel2 = int.Parse(data[1].Trim());
                                    Constants.HERO_POWER_2 = powerLevel2;
                                    break;
                                case "hero power 3":
                                    int powerLevel3 = int.Parse(data[1].Trim());
                                    Constants.HERO_POWER_3 = powerLevel3;
                                    break;
                                case "hero power 4":
                                    int powerLevel4 = int.Parse(data[1].Trim());
                                    Constants.HERO_POWER_4 = powerLevel4;
                                    break;
                                case "hero power 5":
                                    int powerLevel5 = int.Parse(data[1].Trim());
                                    Constants.HERO_POWER_5 = powerLevel5;
                                    break;
                                case "power decrease multiplier":
                                    float powerDecreaseMultiplier = float.Parse(data[1].Trim());
                                    Constants.POWER_DECREASE_MULTIPLIER = powerDecreaseMultiplier;
                                    break;
                                case "hero sending delay":
                                    float heroSendingDelay = float.Parse(data[1].Trim());
                                    Constants.HERO_SENDING_DELAY = heroSendingDelay;
                                    break;
                                case "building damage multiplier":
                                    float bdmg = float.Parse(data[1].Trim());
                                    Hero.buildingDamageMultiplier = bdmg;
                                    break;
                                case "hero damage multiplier":
                                    float hdmg = float.Parse(data[1].Trim());
                                    FightSimulator.heroDamageMultiplier = hdmg;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            while (line != null);
            sReader.Close();
            return;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
