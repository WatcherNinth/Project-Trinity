using UnityEngine;
using System.Collections;
using Lucky;

public class StringProcessScript : BaseInstance<StringProcessScript> {
    public enum StringType
    {
        Accident,
        Event
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public Accident AccidentStringProcess(Accident accident)
    {
        string title = accident.text.title;
        string description = accident.text.description;
        string[] dictIn, dictOut;
        int duration = accident.duration;
        string eDate = accident.starttime.AddMinutes(duration).Date.ToString();
        string eTime = accident.starttime.AddMinutes(duration).TimeOfDay.ToString();
        duration = duration / 60;
        
        dictIn = new string[7] {"<Spos>","<Epos>", "<Sdate>","<Edate>","<Stime>","<Etime>","<Duration>" };
        dictOut = new string[7] { "Spos", "Epos", accident.starttime.Date.ToString(), eDate, accident.starttime.Date.ToString(), eTime, duration.ToString() };
        Debug.Log(accident.type);
        switch(accident.type){
            case AccidentType.rail:
                {
                    dictOut[0] = CityUtil.Instance.GetEdgeCity(accident.location).start_node;
                    dictOut[1] = CityUtil.Instance.GetEdgeCity(accident.location).end_node;
                    break;
                }
            case AccidentType.airport:
                {
                    dictOut[0] = CityUtil.Instance.GetCityName(accident.location);
                    break;
                }
        }
        for(int i = 0; i < dictIn.Length; i++)
        {
            accident.text.title = accident.text.title.Replace(dictIn[i], dictOut[i]);
            Debug.Log(accident.text.title);
            accident.text.description = accident.text.description.Replace(dictIn[i], dictOut[i]);
        }
        return (accident);
    }
   public bool EventStringProcess()
    {
        return (true);
    }
}
