using UnityEngine;
using System.Collections;
using Lucky;
using System;
using System.Collections.Generic;

public class TrainState
{
    public int id;
    public string name;
    public DateTime StartTime;
    public DateTime EndTime;
    public string startLocation;
    public string stopLocation;
};

public class TrainManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CancelTrain()
    {

    }

    public void DelayStartTrain()
    {

    }

    public void DelayStopTrain()
    {

    }

    private void RegisterMsg(bool isOn)
    {
        if (isOn)
        {
            MessageBus.Register<AccidentMessage>(HandleAccident);
        }
        else
        {
            MessageBus.UnRegister<AccidentMessage>(HandleAccident);
        }
    }

    private bool HandleAccident(AccidentMessage am)
    {
        FindStartInfluence(am);
        FindDrivingInfluence(am);
        FindStopInfluence(am);
        return false;
    }

    private List<TrainState> FindStartInfluence(AccidentMessage am)
    {
        return null;
    }

    private List<TrainState> FindDrivingInfluence(AccidentMessage am)
    {
        return null;
    }

    private List<TrainState> FindStopInfluence(AccidentMessage am)
    {
        return null;
    }

}
