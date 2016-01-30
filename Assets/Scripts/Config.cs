using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;

public class Config : Singleton<Config> {
    public string filename = "config.ini";

    private GameManager _gameManager;

    void Awake()
    {
        //Initialize Singletons
        _gameManager = GameManager.Instance;

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
                                    String[] keys = data[1].Trim().Split(',');
                                    //TODO: Eugene
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
