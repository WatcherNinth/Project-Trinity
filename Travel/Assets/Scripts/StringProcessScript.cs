﻿using UnityEngine;
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

        string sDate = accident.starttime.Month.ToString()+"月"+accident.starttime.Day.ToString() + "日";
        string sTime = accident.starttime.TimeOfDay.ToString().Remove(5,3);
        string eDate = accident.starttime.AddMinutes(duration).Month.ToString() + "月"+accident.starttime.AddMinutes(duration).Day.ToString()+"日";
        string eTime = accident.starttime.AddMinutes(duration).TimeOfDay.ToString().Remove(5, 3);
        duration = duration / 60;
        
        dictIn = new string[7] {"<Spos>","<Epos>", "<Sdate>","<Edate>","<Stime>","<Etime>","<Duration>" };
        dictOut = new string[7] { "Spos", "Epos", sDate, eDate, sTime, eTime, duration.ToString() };
        //Debug.Log(accident.type);
        switch(accident.type){
            case AccidentType.rail:
                {
                    Debug.Log(accident.location);
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
        if (duration == 0) dictOut[6] = "不足一";
        for(int i = 0; i < dictIn.Length; i++)
        {
            accident.text.title = accident.text.title.Replace(dictIn[i], dictOut[i]);
            accident.text.description = accident.text.description.Replace(dictIn[i], dictOut[i]);
        }
        //Debug.Log(accident.text.description);
        return (accident);
    }
   public bool EventStringProcess()
    {
        return (true);
    }
}
