using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{   
    public string L;
    public int M;
    public int S;
    public int Mus;

    public float Zoom;
    public int Lvl;

    public bool AdvOff;

    public bool Tut1;


    public PlayerData()
    {        
        L = ""; //prefered language
        M = 1; //mobile platform? 1 - true;
        S = 1; // sound on? 1 - true;        
        Mus = 1; // music
        Zoom = 0; //camera zoom
        Lvl = 0;
        AdvOff = true;
        Tut1 = false;

        Debug.Log("created PlayerData instance");
    }


}
