using UnityEngine;
using System.Collections;
using System;

public enum AccidentType
{
    rail,
    airport
}

public class Accident
{
    public int location;
    public AccidentType type;
    public DateTime starttime;
    public int duration;
    public AccidentText text;
}
public class AccidentText
{
    public AccidentType type;
    public string title;
    public string description;
}

public class AcciedntManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void GenerateAccident()
    {

    }
}
