﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputController : Singleton<InputController> {

    private Dictionary<KeyCode, InputTrigger> _keyMap;

    public delegate void InputTrigger();

    void Awake()
    {
        _keyMap = new Dictionary<KeyCode, InputTrigger>();
    }

    public void registerTrigger(InputTrigger trigger, KeyCode key)
    {
        _keyMap.Add(key, trigger);
    }

    // Use this for initialization
    void Start()
    {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("q") && _keyMap.ContainsKey(KeyCode.Q))
        {
            _keyMap[KeyCode.Q]();
        }
        else if (Input.GetKeyDown("w") && _keyMap.ContainsKey(KeyCode.W))
        {
            _keyMap[KeyCode.W]();
        }
        else if (Input.GetKeyDown("e") && _keyMap.ContainsKey(KeyCode.E))
        {
            _keyMap[KeyCode.E]();
        }
        else if (Input.GetKeyDown("r") && _keyMap.ContainsKey(KeyCode.R))
        {
            _keyMap[KeyCode.R]();
        }
        else if (Input.GetKeyDown("t") && _keyMap.ContainsKey(KeyCode.T))
        {
            _keyMap[KeyCode.T]();
        }
        else if (Input.GetKeyDown("y") && _keyMap.ContainsKey(KeyCode.Y))
        {
            _keyMap[KeyCode.Y]();
        }
        else if (Input.GetKeyDown("u") && _keyMap.ContainsKey(KeyCode.U))
        {
            _keyMap[KeyCode.U]();
        }
        else if (Input.GetKeyDown("i") && _keyMap.ContainsKey(KeyCode.I))
        {
            _keyMap[KeyCode.I]();
        }
        else if (Input.GetKeyDown("o") && _keyMap.ContainsKey(KeyCode.O))
        {
            _keyMap[KeyCode.O]();
        }
        else if (Input.GetKeyDown("p") && _keyMap.ContainsKey(KeyCode.P))
        {
            _keyMap[KeyCode.P]();
        }
        else if (Input.GetKeyDown("a") && _keyMap.ContainsKey(KeyCode.A))
        {
            _keyMap[KeyCode.A]();
        }
        else if (Input.GetKeyDown("s") && _keyMap.ContainsKey(KeyCode.S))
        {
            _keyMap[KeyCode.S]();
        }
        else if (Input.GetKeyDown("d") && _keyMap.ContainsKey(KeyCode.D))
        {
            _keyMap[KeyCode.D]();
        }
        else if (Input.GetKeyDown("f") && _keyMap.ContainsKey(KeyCode.F))
        {
            _keyMap[KeyCode.F]();
        }
        else if (Input.GetKeyDown("g") && _keyMap.ContainsKey(KeyCode.G))
        {
            _keyMap[KeyCode.G]();
        }
        else if (Input.GetKeyDown("h") && _keyMap.ContainsKey(KeyCode.H))
        {
            _keyMap[KeyCode.H]();
        }
        else if (Input.GetKeyDown("j") && _keyMap.ContainsKey(KeyCode.J))
        {
            _keyMap[KeyCode.J]();
        }
        else if (Input.GetKeyDown("k") && _keyMap.ContainsKey(KeyCode.K))
        {
            _keyMap[KeyCode.K]();
        }
        else if (Input.GetKeyDown("l") && _keyMap.ContainsKey(KeyCode.L))
        {
            _keyMap[KeyCode.L]();
        }
        else if (Input.GetKeyDown("z") && _keyMap.ContainsKey(KeyCode.Z))
        {
            _keyMap[KeyCode.Z]();
        }
        else if (Input.GetKeyDown("x") && _keyMap.ContainsKey(KeyCode.X))
        {
            _keyMap[KeyCode.X]();
        }
        else if (Input.GetKeyDown("c") && _keyMap.ContainsKey(KeyCode.C))
        {
            _keyMap[KeyCode.C]();
        }
        else if (Input.GetKeyDown("v") && _keyMap.ContainsKey(KeyCode.V))
        {
            _keyMap[KeyCode.V]();
        }
        else if (Input.GetKeyDown("b") && _keyMap.ContainsKey(KeyCode.B))
        {
            _keyMap[KeyCode.B]();
        }
        else if (Input.GetKeyDown("n") && _keyMap.ContainsKey(KeyCode.N))
        {
            _keyMap[KeyCode.N]();
        }
        else if (Input.GetKeyDown("m") && _keyMap.ContainsKey(KeyCode.M))
        {
            _keyMap[KeyCode.M]();
        }
        else if (Input.GetKeyDown("down") && _keyMap.ContainsKey(KeyCode.DownArrow))
        {
            _keyMap[KeyCode.DownArrow]();
        }
        else if (Input.GetKeyDown("up") && _keyMap.ContainsKey(KeyCode.UpArrow))
        {
            _keyMap[KeyCode.UpArrow]();
        }
        else if (Input.GetKeyDown("left") && _keyMap.ContainsKey(KeyCode.LeftArrow))        
        {
            _keyMap[KeyCode.LeftArrow]();
        }
        else if (Input.GetKeyDown("right") && _keyMap.ContainsKey(KeyCode.RightArrow))
        {
            _keyMap[KeyCode.RightArrow]();
        }
        else if (Input.GetMouseButtonDown(1) && _keyMap.ContainsKey(KeyCode.Mouse1))
        {
            _keyMap[KeyCode.Mouse1]();
        }
        else if (Input.GetMouseButtonDown(0) && _keyMap.ContainsKey(KeyCode.Mouse0))
        {
            _keyMap[KeyCode.Mouse0]();
        }
	}

}
