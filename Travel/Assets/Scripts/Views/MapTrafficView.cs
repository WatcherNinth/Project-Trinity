using UnityEngine;
using System.Collections;

public class MapTrafficView : MonoBehaviour {

    private static MapTrafficView _instance;

    public static MapTrafficView instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AirPlaneFly(TicketParam tp)
    {

    }

    public void TrainGo(TicketParam tp)
    {

    }

    public void ShowAccident(BaseAccident ba)
    {
        if(ba.GetType() == typeof(Accident))
        {

        }
        else if(ba.GetType() == typeof(AccidentWarning))
        {

        }
    }
}
