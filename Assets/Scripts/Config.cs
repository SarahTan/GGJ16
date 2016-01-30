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
