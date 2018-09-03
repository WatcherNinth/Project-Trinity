using UnityEngine;
using System.Collections;
using Lucky;
using System.Collections.Generic;

public class AirPlaneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void CancelAirPlane()
    {

    }

    public void DelayStartAirPlane()
    {

    }

    public void DelayStopAirPlane()
    {

    }

    private void RegisterMsg(bool isOn)
    {
        if (isOn)
        {
            MessageBus.Register<Accident>(HandleAccident);
        }
        else
        {
            MessageBus.UnRegister<Accident>(HandleAccident);
        }
    }

    private bool HandleAccident(Accident am)
    {
        FindStartInfluence(am);
        FindDrivingInfluence(am);
        FindStopInfluence(am);
        return false;
    }

    private List<TrainState> FindStartInfluence(Accident am)
    {
        return null;
    }

    private List<TrainState> FindDrivingInfluence(Accident am)
    {
        return null;
    }

    private List<TrainState> FindStopInfluence(Accident am)
    {
        return null;
    }
}
