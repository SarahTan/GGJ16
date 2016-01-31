using UnityEngine;
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

    public KeyCode[] parseKeys(string[] keys)
    {
        KeyCode[] codes = new KeyCode[keys.Length];
        for(int i = 0; i < keys.Length; i++)
        {
            codes[i] = parseString(keys[i]);
        }
        return codes;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("q") && _keyMap.ContainsKey(KeyCode.Q))
        {
            _keyMap[KeyCode.Q]();
        }
        if (Input.GetKeyDown("w") && _keyMap.ContainsKey(KeyCode.W))
        {
            _keyMap[KeyCode.W]();
        }
        if (Input.GetKeyDown("e") && _keyMap.ContainsKey(KeyCode.E))
        {
            _keyMap[KeyCode.E]();
        }
        if (Input.GetKeyDown("r") && _keyMap.ContainsKey(KeyCode.R))
        {
            _keyMap[KeyCode.R]();
        }
        if (Input.GetKeyDown("t") && _keyMap.ContainsKey(KeyCode.T))
        {
            _keyMap[KeyCode.T]();
        }
        if (Input.GetKeyDown("y") && _keyMap.ContainsKey(KeyCode.Y))
        {
            _keyMap[KeyCode.Y]();
        }
        if (Input.GetKeyDown("u") && _keyMap.ContainsKey(KeyCode.U))
        {
            _keyMap[KeyCode.U]();
        }
        if (Input.GetKeyDown("i") && _keyMap.ContainsKey(KeyCode.I))
        {
            _keyMap[KeyCode.I]();
        }
        if (Input.GetKeyDown("o") && _keyMap.ContainsKey(KeyCode.O))
        {
            _keyMap[KeyCode.O]();
        }
        if (Input.GetKeyDown("p") && _keyMap.ContainsKey(KeyCode.P))
        {
            _keyMap[KeyCode.P]();
        }
        if (Input.GetKeyDown("a") && _keyMap.ContainsKey(KeyCode.A))
        {
            _keyMap[KeyCode.A]();
        }
        if (Input.GetKeyDown("s") && _keyMap.ContainsKey(KeyCode.S))
        {
            _keyMap[KeyCode.S]();
        }
        if (Input.GetKeyDown("d") && _keyMap.ContainsKey(KeyCode.D))
        {
            _keyMap[KeyCode.D]();
        }
        if (Input.GetKeyDown("f") && _keyMap.ContainsKey(KeyCode.F))
        {
            _keyMap[KeyCode.F]();
        }
        if (Input.GetKeyDown("g") && _keyMap.ContainsKey(KeyCode.G))
        {
            _keyMap[KeyCode.G]();
        }
        if (Input.GetKeyDown("h") && _keyMap.ContainsKey(KeyCode.H))
        {
            _keyMap[KeyCode.H]();
        }
        if (Input.GetKeyDown("j") && _keyMap.ContainsKey(KeyCode.J))
        {
            _keyMap[KeyCode.J]();
        }
        if (Input.GetKeyDown("k") && _keyMap.ContainsKey(KeyCode.K))
        {
            _keyMap[KeyCode.K]();
        }
        if (Input.GetKeyDown("l") && _keyMap.ContainsKey(KeyCode.L))
        {
            _keyMap[KeyCode.L]();
        }
        if (Input.GetKeyDown("z") && _keyMap.ContainsKey(KeyCode.Z))
        {
            _keyMap[KeyCode.Z]();
        }
        if (Input.GetKeyDown("x") && _keyMap.ContainsKey(KeyCode.X))
        {
            _keyMap[KeyCode.X]();
        }
        if (Input.GetKeyDown("c") && _keyMap.ContainsKey(KeyCode.C))
        {
            _keyMap[KeyCode.C]();
        }
        if (Input.GetKeyDown("v") && _keyMap.ContainsKey(KeyCode.V))
        {
            _keyMap[KeyCode.V]();
        }
        if (Input.GetKeyDown("b") && _keyMap.ContainsKey(KeyCode.B))
        {
            _keyMap[KeyCode.B]();
        }
        if (Input.GetKeyDown("n") && _keyMap.ContainsKey(KeyCode.N))
        {
            _keyMap[KeyCode.N]();
        }
        if (Input.GetKeyDown("m") && _keyMap.ContainsKey(KeyCode.M))
        {
            _keyMap[KeyCode.M]();
		}
		if (Input.GetKeyDown("space") && _keyMap.ContainsKey(KeyCode.Space))
		{
			_keyMap[KeyCode.Space]();
		}
		if (Input.GetKeyDown("return") && _keyMap.ContainsKey(KeyCode.Return))
		{
			_keyMap[KeyCode.Return]();
		}
        if (Input.GetKeyDown("down") && _keyMap.ContainsKey(KeyCode.DownArrow))
        {
            _keyMap[KeyCode.DownArrow]();
        }
        if (Input.GetKeyDown("up") && _keyMap.ContainsKey(KeyCode.UpArrow))
        {
            _keyMap[KeyCode.UpArrow]();
        }
        if (Input.GetKeyDown("left") && _keyMap.ContainsKey(KeyCode.LeftArrow))        
        {
            _keyMap[KeyCode.LeftArrow]();
        }
        if (Input.GetKeyDown("right") && _keyMap.ContainsKey(KeyCode.RightArrow))
        {
            _keyMap[KeyCode.RightArrow]();
        }
        if (Input.GetMouseButtonDown(1) && _keyMap.ContainsKey(KeyCode.Mouse1))
        {
            _keyMap[KeyCode.Mouse1]();
        }
        if (Input.GetMouseButtonDown(0) && _keyMap.ContainsKey(KeyCode.Mouse0))
        {
            _keyMap[KeyCode.Mouse0]();
        }
	}

    public KeyCode parseString(string key)
    {
        switch (key)
        {
            case "Q":
            case "q":
                return KeyCode.Q;
            case "W":
            case "w":
                return KeyCode.W;
            case "E":
            case "e":
                return KeyCode.E;
            case "R":
            case "r":
                return KeyCode.R;
            case "T":
            case "t":
                return KeyCode.T;
            case "Y":
            case "y":
                return KeyCode.Y;
            case "u":
            case "U":
                return KeyCode.U;
            case "I":
            case "i":
                return KeyCode.I;
            case "O":
            case "o":
                return KeyCode.O;
            case "P":
            case "p":
                return KeyCode.P;
            case "A":
            case "a":
                return KeyCode.A;
            case "S":
            case "s":
                return KeyCode.S;
            case "D":
            case "d":
                return KeyCode.D;
            case "F":
            case "f":
                return KeyCode.F;
            case "G":
            case "g":
                return KeyCode.G;
            case "H":
            case "h":
                return KeyCode.H;
            case "J":
            case "j":
                return KeyCode.J;
            case "K":
            case "k":
                return KeyCode.K;
            case "L":
            case "l":
                return KeyCode.L;
            case "Z":
            case "z":
                return KeyCode.Z;
            case "X":
            case "x":
                return KeyCode.X;
            case "C":
            case "c":
                return KeyCode.C;
            case "V":
            case "v":
                return KeyCode.V;
            case "B":
            case "b":
                return KeyCode.B;
            case "N":
            case "n":
                return KeyCode.N;
            case "m":
            case "M":
                return KeyCode.M;
            case "DOWN":
            case "Down":
            case "down":
                return KeyCode.DownArrow;
            case "UP":
            case "Up":
            case "up":
                return KeyCode.UpArrow;
            case "LEFT":
            case "Left":
            case "left":
                return KeyCode.LeftArrow;
            case "RIGHT":
            case "Right":
            case "right":
                return KeyCode.RightArrow;
            case "ENTER":
            case "enter":
            case "Enter":
            case "RETURN":
            case "return":
            case "Return":
                return KeyCode.Return;
            case "Spacebar":
            case "SPACEBAR":
            case "spacebar":
            case "SPACE":
            case "space":
            case "Space":
                return KeyCode.Space;
            case "MOUSE RIGHT":
            case "Mouse Right":
            case "mouse right":
                return KeyCode.Mouse1;
            case "MOUSE LEFT":
            case "Mouse Left":
            case "mouse left":
                return KeyCode.Mouse0;
            default:
                return KeyCode.Q;
        }
    }
}
