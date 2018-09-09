using UnityEngine;
using System.Collections;

public class StringProcessScript : MonoBehaviour {
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
    public bool AccidentStringProcess(Accident accident)
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
        switch(accident.type){
            case AccidentType.rail:
                {
                    
                    break;
                }
            case AccidentType.airport:
                {
                    dictOut[0] = accident.location.ToString();
                    break;
                }
        }
        for(int i = 0; i <= dictIn.Length; i++)
        {
            accident.text.title.Replace(dictIn[i], dictOut[i]);
            accident.text.description.Replace(dictIn[i], dictOut[i]);
        }
        return (true);
    }
   public bool EventStringProcess()
    {
        return (true);
    }
}
