using UnityEngine;
using System.Collections;

public class MapTrafficView : MonoBehaviour {

    public AirLineView airline;
    public GameObject airplane;

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
        Debug.Log("airplane fly"+ tp.rt.GetBeginTime());
        Vector3 start = Vector3.zero;
        LocationsModel.cityslocation.TryGetValue(tp.rt.GetRoutineStartNode(), out start);
        Vector3 stop = Vector3.zero;
        LocationsModel.cityslocation.TryGetValue(tp.rt.GetEndNode(), out stop);
        airline.Show(start, stop);
    }

    public void TrainGo(TicketParam tp)
    {
        Debug.Log("train go" + tp.rt.GetBeginTime());
        Debug.Log("airplane fly" + tp.rt.GetBeginTime());
        Vector3 start = Vector3.zero;
        LocationsModel.cityslocation.TryGetValue(tp.rt.GetRoutineStartNode(), out start);
        Vector3 stop = Vector3.zero;
        LocationsModel.cityslocation.TryGetValue(tp.rt.GetEndNode(), out stop);
        airline.Show(start, stop);
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
